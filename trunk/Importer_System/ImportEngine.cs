using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;
using cpsc594_cdl.Common.Models;
using Importer_System.Metrics;


namespace Importer_System
{

    class ImportEngine
    {
        private string _rootDirectory;                           // Root directory of the folders
        private string _rootArchiveDirectory;                    // Archive directory for files after being read
        private int _archivePeriod;                              //number of days the logfiles exist in the archive directory
        private string _testDirectory;
        private string _outputDatabaseConnection;                // Connection string to output the information to
        private string _bugzillaDatabaseConnection;              // Connection string to the bugzilla database
        private int _iterationLength;                            //Iteration length in weeks
        private DateTime _iterationStart;                        //Begin date of iteration
        private CodeCoverage _codeCoverageMetric;                // Class that calculates code coverage
        private DefectMetrics _defectMetrics;                    // Class that calculates the injection rate and repair rate
        private TestEffectivenessMetric _testEffectivenessMetric;            // Class that calculates the test effectiveness
        private ConnectionStringSettings _outputDbSettings;      //
        private ConnectionStringSettings _bugzillaDbSettings;    //
        private Boolean computeMetricThree = true;               // Checks if the database for the metric is available, if not sets it to false
        private ProgressForm progressForm = null;

        /// <summary>
        ///     Customized constructor to initialize the behavior.
        /// </summary>
        public ImportEngine()
        {
            // Read in the configuration file for settings
            ReadConfigFile();
            // Test connection of output database
            EstablishConnectionToOutputDatabase();
            // Test existance of the root directory
            EstablishExistanceOfDirectory(_rootDirectory);
        }

        public static object RunStatic(Type classType, string method)
        {
            var methodInfo = classType.GetMethod(method,
            BindingFlags.NonPublic | BindingFlags.Static);
            return methodInfo.Invoke(null, null);
        }

        /// <summary>
        ///     Reads in the configuration file for the program and does the following:
        ///     1. Sets the engines rootDirectory and outputDatabaseConnection
        ///     2. Create all the coverage tools with the appropriate settings
        /// </summary>
        public void ReadConfigFile()
        {
            #pragma warning disable
            try { _archivePeriod = Int32.Parse(ConfigurationManager.AppSettings["ArchivePeriod"]);            
            }
            catch (Exception e)
            {
                _archivePeriod = -1;
                Reporter.AddMessageToReporter("ArchivePeriod key in configuration file is not a valid integer. Cannot update archive", true, false);
            }
            _rootDirectory = ConfigurationManager.AppSettings["RootDirectory"];
            _rootArchiveDirectory = ConfigurationManager.AppSettings["ArchiveDirectory"];

            _testDirectory = ConfigurationManager.AppSettings["TestDirectory"];

            _outputDbSettings = ConfigurationManager.ConnectionStrings["CPSC594Entities"];
            _outputDatabaseConnection = _outputDbSettings.ConnectionString;

            _bugzillaDbSettings = ConfigurationManager.ConnectionStrings["BugzillaDatabase"];
            _bugzillaDatabaseConnection = _bugzillaDbSettings.ConnectionString;

            _iterationStart = DateTime.Parse(ConfigurationManager.AppSettings["IterationStartDate"]);
            try { _iterationLength = Int32.Parse(ConfigurationManager.AppSettings["IterationLength"]);
            }catch{
                _archivePeriod = -1;
                Reporter.AddMessageToReporter("The iteration length key in the configuration file is not a valid integer.", true, false);
            }

            // CREATE METRIC 1
            _codeCoverageMetric = new CodeCoverage();

            //CREATE METRIC 2
            _testEffectivenessMetric = new TestEffectivenessMetric();

            // CREATE METRIC 3 and 4
            // Attempt a connection to the bugzilla database, if failed, set a flag to skip the metrics which require it
            _defectMetrics = new DefectMetrics(_bugzillaDatabaseConnection);
            if (!_defectMetrics.EstablishConnection())
            {
                Reporter.AddMessageToReporter("[Metric 3 and 4: DefectInjectionRate/DefectRepairRate] Could not make a connection to database with connection string " + _bugzillaDatabaseConnection + ". Skipping metrics 3 and 4", true, false);
                computeMetricThree = false;
            }
        }

        /// <summary>
        ///     Attempts to connect to the database connection string provided /outputDatabaseConnection/
        ///     If fail
        ///         throw new OutputDatabaseConnectionException
        /// </summary>
        /// <throws>
        ///     OutputDatabaseConnectionException
        /// </throws>
        public void EstablishConnectionToOutputDatabase()
        {
            Boolean connectionExists = false;
            // If failed connection
            try
            {
                if (DatabaseAccessor.Connection())
                    connectionExists = true;
            }
            catch (TypeInitializationException) { }
            if(!connectionExists)
                throw new OutputDatabaseConnectionException("Connection to the web-output database with string: " + _outputDatabaseConnection + " failed");
        }


        /// <summary>
        ///     Attempts to check if the /rootDirectory/ exists
        ///     If fail
        ///         throw new NoExistanceOfDirectoryException
        /// </summary>
        /// <throws>
        ///     NoExistanceOfDirectoryException
        /// </throws>
        public void EstablishExistanceOfDirectory(string directory)
        {
            // If no root directory
            if(!Directory.Exists(_rootDirectory))
                throw new NoExistanceOfDirectoryException("Root directory : " + directory + " could not be found");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<String> GetListOfProjects()
        {
            List<String> tempList = new List<string>();
            DirectoryInfo initialDirectory = new DirectoryInfo(_rootDirectory);
            foreach (DirectoryInfo project in initialDirectory.GetDirectories())
            {
                tempList.Add(project.Name);
            }
            return tempList;
        }

        /// <summary>
        ///     Begin to calculate the metrics from the sources
        /// </summary>
        public void BeginImporting()
        {
            string currFile;
            int coverageID = -1;
            string uniqueFileName = "";
            Iteration currIteration;
            // Make directory structure
            DirectoryInfo initialDirectory = new DirectoryInfo(_rootDirectory);
            currIteration = UpdateIteration();
            // Start TimeStamp
            long startTime = (DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks)/TimeSpan.TicksPerMillisecond;

            // Iterate through the projects in the code coverage directory
            foreach (DirectoryInfo project in initialDirectory.GetDirectories())
            { 
                // Save the current projects name
                string currentProjectName = project.Name;
                
                // Update WinForm Status
                UpdateProjectStatus(currentProjectName, "Calculating");

                // Check if project is in DB
                if(!DatabaseAccessor.ProjectExists(currentProjectName))
                {
                    DatabaseAccessor.WriteProject(currentProjectName);
                }



                string projectDirectory = Path.Combine(_rootDirectory, currentProjectName);
                DirectoryInfo testFiles = new DirectoryInfo(projectDirectory);
                foreach (FileInfo testFile in testFiles.GetFiles())
                {
                    currFile = Path.Combine(projectDirectory, testFile.Name);
                    _testEffectivenessMetric.CalculateMetric(currFile, currIteration.IterationID);
                }

                
                // Iterate through the selected projects components);
                foreach (DirectoryInfo component in project.GetDirectories())
                {
                    // Save the current components name
                    string currentComponentName = component.Name;

                    // Check if component is in DB
                    if(!DatabaseAccessor.ComponentExists(currentProjectName,currentComponentName))
                    {
                        DatabaseAccessor.WriteComponent(currentProjectName, currentComponentName);
                    }

                    // ---------------------------------------------------------------------
                    // COMPUTE METRIC 1 - CODE COVERAGE
                    // ---------------------------------------------------------------------
                    string currentMetric1Directory = Path.Combine(_rootDirectory, currentProjectName, currentComponentName, "Metric1");
                    // Check if the code coverage folder exists
                    if(Directory.Exists(currentMetric1Directory))
                    {
                            DirectoryInfo logFiles = new DirectoryInfo(currentMetric1Directory);
                            // Iterate through each code coverage log file and calculate the metric
                            foreach (FileInfo logFile in logFiles.GetFiles())
                            {
                                currFile = Path.Combine(currentMetric1Directory, logFile.Name);
                                // Archive the log file if returned true
                                coverageID = _codeCoverageMetric.CalculateMetric(currentProjectName, currentComponentName, currFile, currIteration.IterationID);
                                if(coverageID >= 0)
                                {
                                    //archiveFile(currentProjectName, currentComponentName, logFile.Name);
                                    uniqueFileName = BuildUniqueFilename(logFile.Name, coverageID);
                                    RenameFile(currFile, Path.Combine(currentMetric1Directory, uniqueFileName));
                                    DatabaseAccessor.UpdateCoverage(coverageID, logFile.LastWriteTimeUtc.Date, uniqueFileName );
                                    ArchiveFile(currentProjectName, currentComponentName, uniqueFileName);
                                }
                                coverageID = -1;
                                uniqueFileName = "";
                            }
                    }
                    // --------------------------------------------------------------------
                    // END METRIC 1
                    // --------------------------------------------------------------------


                    // ---------------------------------------------------------------------
                    // COMPUTE METRIC 3 AND 4 - DEFECTINJECTIONRATE AND DEFECTREPAIRRATE
                    // ---------------------------------------------------------------------
                    if (computeMetricThree)
                        _defectMetrics.CalculateMetric(currentProjectName, currentComponentName, currIteration);
                    // --------------------------------------------------------------------
                    // END METRIC 3 AND 4
                    // --------------------------------------------------------------------
                }
                UpdateProjectStatus(currentProjectName, "Done");
                
            }

            initialDirectory = new DirectoryInfo(_testDirectory);


            //Loop through the project folders in the test directory
            //foreach (DirectoryInfo project in initialDirectory.GetDirectories())
            //{
                // Get the projects name
                //string currentProjectName = project.Name;

                // Update WinForm Status
                //UpdateProjectStatus(currentProjectName, "Calculating");
            
                //_testEffectivenessMetric.CalculateMetric();

                // Update WinForm Status
                //UpdateProjectStatus(currentProjectName, "Done");
            //}
            
            
            long endTime = (DateTime.UtcNow.Ticks - DateTime.Parse("01/01/1970 00:00:00").Ticks) / TimeSpan.TicksPerMillisecond;
            progressForm.SetFinishStatus(endTime - startTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Iteration UpdateIteration()
        {
            Iteration lastIteration = DatabaseAccessor.GetLastIteration();

            if (lastIteration == null)
            {
                DatabaseAccessor.WriteIteration(_iterationStart, _iterationStart.AddDays(_iterationLength * 7));
            }
            else
            {
                var endDate = (DateTime)lastIteration.EndDate;
                if (endDate.Date < DateTime.UtcNow.Date)
                {
                    DatabaseAccessor.WriteIteration(endDate.AddDays(1), endDate.AddDays((_iterationLength * 7) + 1));
                    lastIteration = DatabaseAccessor.GetLastIteration();
                }
            }

            return lastIteration;
        }

        /// <summary>
        ///     Renames the given file.
        /// </summary>
        /// <param name="currFile">Current file name</param>
        /// <param name="newFile">New file name</param>
        /// <returns></returns>
        public bool RenameFile(string currFile, string newFile)
        {
            if(File.Exists(currFile))
            {
                File.Copy(currFile, newFile);
                if (File.Exists(newFile))
                {
                    File.Delete(currFile);
                    return true;
                }
                else
                {
                    return false;
                }    
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///     Appends the filename with a unique id tag.
        /// </summary>
        /// <param name="fileName">Current file name</param>
        /// <param name="id">Unique ID</param>
        /// <returns></returns>
        // ReSharper disable MemberCanBeMadeStatic.Local
        public string BuildUniqueFilename(string fileName, int id)
        // ReSharper restore MemberCanBeMadeStatic.Local
        {
            string[] separatedFileName;
            string uniqueFileName = "";

            separatedFileName = fileName.Split('.');
            uniqueFileName = separatedFileName[0];
            for(int i = 1; i < separatedFileName.Length; i++)
            {
                if (i == separatedFileName.Length - 1)
                    uniqueFileName = uniqueFileName + "." + id.ToString();
                uniqueFileName = uniqueFileName + "." + separatedFileName[i];
            }

            return uniqueFileName;
        }

        /// <summary>
        ///     Archives the code coverage metric files.
        /// </summary>
        /// <param name="project">Current project</param>
        /// <param name="component">Current component</param>
        /// <param name="file">File to be saved</param>
        public bool ArchiveFile(string project, string component, string file)
        {
            string archMetric1Directory = Path.Combine(_rootArchiveDirectory, project, component, "Metric1");
            string currMetric1Directory = Path.Combine(_rootDirectory, project, component, "Metric1");

            if(File.Exists(Path.Combine(currMetric1Directory, file)))
            {
                //check if file already exists
                if (!File.Exists(Path.Combine(archMetric1Directory, file)))
                {
                    //check if directory needs to be created
                    if (Directory.Exists(archMetric1Directory))
                    {
                        //move file
                        File.Move(Path.Combine(currMetric1Directory, file), Path.Combine(archMetric1Directory, file));
                    }
                    else
                    {
                        //create archive directory for coverage
                        Directory.CreateDirectory(archMetric1Directory);
                        //move file
                        File.Move(Path.Combine(currMetric1Directory, file), Path.Combine(archMetric1Directory, file));
                    }
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        ///     Updates archive directory after x amount of days.
        /// </summary>
        public void UpdateArchiveDirectory()
        {
            // If we have an archive time, update archive
            if (_archivePeriod != -1 && Directory.Exists(_rootArchiveDirectory))
            {
                // Make directory structure
                DirectoryInfo initialDirectory = new DirectoryInfo(_rootArchiveDirectory);
                TimeSpan difference;

                // Iterate through the projects in the directory
                foreach (DirectoryInfo project in initialDirectory.GetDirectories())
                {
                    // Save the current projects name
                    string archProjectName = project.Name;
                    // Iterate through the selected projects components
                    foreach (DirectoryInfo component in project.GetDirectories())
                    {
                        // Save the current components name
                        string archComponentName = component.Name;

                        string archiveMetric1Directory = Path.Combine(_rootArchiveDirectory, archProjectName, archComponentName, "Metric1");

                        if (Directory.Exists(archiveMetric1Directory))
                        {
                            DirectoryInfo archivedLogFiles = new DirectoryInfo(archiveMetric1Directory);
                            foreach (FileInfo logFile in archivedLogFiles.GetFiles())
                            {
                                //currFile = archiveMetric1Directory + "\\" + logFile.Name;
                                difference = DateTime.UtcNow - logFile.LastWriteTimeUtc;

                                if (difference.Days > _archivePeriod)
                                {
                                    logFile.Delete();
                                }
                            }
                        }
                        // END METRIC 1
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="project"></param>
        /// <param name="status"></param>
        private void UpdateProjectStatus(string project, string status)
        {
            if (progressForm != null)
                progressForm.SetMetricStatus(project, status);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="progressForm"></param>
        public void SetProgressForm(ProgressForm progressForm)
        {
            this.progressForm = progressForm;
        }
    }
}
