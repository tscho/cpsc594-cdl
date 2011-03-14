﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
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
        private string _rootDirectory;                            // Root directory of the folders
        private string _rootArchiveDirectory;                     // Archive directory for files after being read
        private string _productDataDirectory = null;              // Directory containing .xls product data files
        private int _archivePeriod;                               //number of days the logfiles exist in the archive directory
        private string _testDirectory;                            // ???
        private string _outputDatabaseConnection;                 // Connection string to output the information to
        private int _iterationLength;                             //Iteration length in weeks
        private DateTime _iterationStart;                         //Begin date of iteration
        private CodeCoverage _codeCoverageMetric;                 // Class that calculates code coverage
        private DefectMetrics _defectMetrics;                     // Class that calculates the injection rate and repair rate
        private TestEffectivenessMetric _testEffectivenessMetric; // Class that calculates the test effectiveness
        private ResourceUtilizationMetric _resourceUtilization;         // Class that calculates work hours per product
        private OutOfScopeWorkMetric _outOfScopeWork;             //Calculates out of scope work
        private ConnectionStringSettings _outputDbSettings;       //

        /// <summary>
        ///     Customized constructor to initialize the behavior.
        /// </summary>
        public ImportEngine()
        {
            // Create the metric classes that do the calculations
            CreateMetrics();
            // Read in the configuration file for settings
            ReadConfigFile();
        }

        public static object RunStatic(Type classType, string method)
        {
            var methodInfo = classType.GetMethod(method,
            BindingFlags.NonPublic | BindingFlags.Static);
            return methodInfo.Invoke(null, null);
        }

        /// <summary>
        ///     Create metrics needed for the program.
        /// </summary>
        private void CreateMetrics()
        {
            // CREATE METRIC 1
            _codeCoverageMetric = new CodeCoverage();
            //CREATE METRIC 2
            _testEffectivenessMetric = new TestEffectivenessMetric();
            // CREATE METRIC 3 and 4
            _defectMetrics = new DefectMetrics();
            // CREATE METRIC 5 
            _resourceUtilization = new ResourceUtilizationMetric();
            //CREATE METRIC 6
            _outOfScopeWork = new OutOfScopeWorkMetric();
        }

        /// <summary>
        ///     Reads in the configuration file for the program and does the following:
        ///     1. Sets the engines rootDirectory and outputDatabaseConnection
        ///     2. Create all the coverage tools with the appropriate settings
        /// </summary>
        private void ReadConfigFile()
        {   
            // _archivePeriod
            ValidateArchivePeriod(ConfigurationManager.AppSettings["ArchivePeriod"]);
            // _rootDirectory
            ValidateRootDirectory(ConfigurationManager.AppSettings["RootDirectory"]);
            // _rootArchiveDirectory
            ValidateRootArchiveDirectory(ConfigurationManager.AppSettings["ArchiveDirectory"]);
            // ???
            _testDirectory = ConfigurationManager.AppSettings["TestDirectory"];
            // _outputDatabase, _outputDbSettings
            ValidateOutputDatabaseConnection(ConfigurationManager.ConnectionStrings["CPSC594Entities"]);
            // _bugzillaDatabaseConnection, bugzillaDbSettings
            //ValidateBugzillaDatabaseConnection(ConfigurationManager.ConnectionStrings["BugzillaDatabase"]);
            // _productDataDirectory
            ValidateProductDataDirectory(ConfigurationManager.AppSettings["productData"]);
            // _iterationStart
            ValidateIterationStart(ConfigurationManager.AppSettings["IterationStartDate"]);
            // _iterationLength
            ValidateIterationLength(ConfigurationManager.AppSettings["IterationLength"]);
        }

        /// <summary>
        ///     ValidateArchivePeriod - If the input provided is not a number, default archive to -1
        ///     Does not terminate program if error.
        /// </summary>
        private void ValidateArchivePeriod(string strNum)
        {
            try {
                _archivePeriod = Int32.Parse(strNum);
            }
            catch
            {
                _archivePeriod = -1;
                Reporter.AddErrorMessageToReporter("ArchivePeriod key in configuration file is not a valid integer. Cannot update archive");
            }
        }

        /// <summary>
        ///     ValidateRootDirectory - Checks whether the directory exists or not.
        ///     Does terminate program if error.
        /// </summary>
        /// <param name="path"></param>
        private void ValidateRootDirectory(string path)
        {
            if (!Directory.Exists(path))
                throw new TerminateException("Root Directory does not exist.");
            _rootDirectory = path;
        }

        /// <summary>
        ///     ValidateRootArchiveDirectory - Checks whether the archive directory exists, if not set it to null.
        ///     Does not terminate program if error.
        /// </summary>
        /// <param name="path"></param>
        private void ValidateRootArchiveDirectory(string path)
        {
            // If the directory exists OR the directory is not specified
            if(Directory.Exists(path) || path.Length==0)
                _rootArchiveDirectory = path;
            else
                _rootArchiveDirectory = "";
        }

        /// <summary>
        ///     ValidateOutputDatabaseConnection - Attempts to establish a connection to the MS SQL database.
        ///     Terminates program if error.
        /// </summary>
        /// <param name="connectionSettings"></param>
        private void ValidateOutputDatabaseConnection(ConnectionStringSettings connectionSettings)
        {
            string connectionString = connectionSettings.ConnectionString;
            Boolean connectionExists = false;
            // If failed connection
            try
            {
                if (DatabaseAccessor.Connection())
                {
                    connectionExists = true;
                    _outputDatabaseConnection = connectionString;
                    _outputDbSettings = connectionSettings;
                }
            }
            catch (TypeInitializationException) { }
            if (!connectionExists)
                throw new TerminateException("Connection to output database with string: " + connectionString + " failed.");
        }

        /// <summary>
        ///     ValidateBugzillaDatabaseConnection - Attempts to establish a connection to the bugzilla MySQL database.
        ///     Does terminate if error.
        /// </summary>
        /// <param name="connectionSettings"></param>
        private void ValidateBugzillaDatabaseConnection(ConnectionStringSettings connectionSettings)
        {
            string connectionString = connectionSettings.ConnectionString;
            _defectMetrics.SetConnectionString(connectionString);
            if (!_defectMetrics.EstablishConnection())
                throw new TerminateException("Connection to bugzilla database with string: " + connectionString + " failed.");
        }

        /// <summary>
        ///     ValidateProductDataDirectory
        /// </summary>
        /// <param name="str"></param>
        private void ValidateProductDataDirectory(String str)
        {
            if(Directory.Exists(str))
                _productDataDirectory = str;
            else
                Reporter.AddErrorMessageToReporter("Could not calculate Resource Utilization Metric becauses the ProductData directory "+str+" could not be found.");
        }

        /// <summary>
        ///     ValidateIterationLength - If the input is not a number, default it to 2.
        ///     Does not terminate on fail.
        /// </summary>
        /// <param name="str"></param>
        private void ValidateIterationLength(string str)
        {
            try
            {
                _iterationLength = Int32.Parse(ConfigurationManager.AppSettings["IterationLength"]);
            }
            catch
            {
                _iterationLength = 2;
                Reporter.AddErrorMessageToReporter("The iteration length key in the configuration file is not a valid integer.");
            }
        }

        /// <summary>
        ///     ValidateIterationStart - If the input is not a date throw exception.
        ///     Terminates the program if date is wrong.
        /// </summary>
        /// <param name="str"></param>
        private void ValidateIterationStart(string str)
        {
            try
            {
                _iterationStart = DateTime.Parse(ConfigurationManager.AppSettings["IterationStartDate"]);
            }
            catch
            {
                throw new TerminateException("Iteration Start date is not a valid date format. Please use dd/mm/yyyy format.");
            }
        }

        /// <summary>
        ///     GetListOfProducts - returns the list of Products in the _rootDirectory
        /// </summary>
        /// <returns></returns>
        public List<String> GetListOfProducts()
        {
            List<String> tempList = new List<string>();
            DirectoryInfo initialDirectory = new DirectoryInfo(_rootDirectory);
            foreach (DirectoryInfo Product in initialDirectory.GetDirectories())
            {
                tempList.Add(Product.Name);
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

            // Iterate through the Products in the code coverage directory
            foreach (DirectoryInfo product in initialDirectory.GetDirectories())
            { 
                // Save the current Products name
                string currentProductName = product.Name;

                // Check if Product is in DB
                if(!DatabaseAccessor.ProductExists(currentProductName))
                {
                    DatabaseAccessor.WriteProduct(currentProductName);
                }

                // ---------------------------------------------------------------------
                // COMPUTE METRIC 2 - TEST EFFECTIVENESS
                // ---------------------------------------------------------------------
                string productDirectory = Path.Combine(_rootDirectory, currentProductName);
                DirectoryInfo testFiles = new DirectoryInfo(productDirectory);
                foreach (FileInfo testFile in testFiles.GetFiles())
                {
                    currFile = Path.Combine(productDirectory, testFile.Name);
                    _testEffectivenessMetric.CalculateMetric(currFile, currIteration.IterationID, currentProductName);
                }
                // Iterate through the selected Products components;
                foreach (DirectoryInfo component in product.GetDirectories())
                {
                    // Save the current components name
                    string currentComponentName = component.Name;

                    // Check if component is in DB
                    if(!DatabaseAccessor.ComponentExists(currentProductName,currentComponentName))
                    {
                        DatabaseAccessor.WriteComponent(currentProductName, currentComponentName);
                    }
                    // ---------------------------------------------------------------------
                    // COMPUTE METRIC 1 - CODE COVERAGE
                    // ---------------------------------------------------------------------
                    string currentMetric1Directory = Path.Combine(_rootDirectory, currentProductName, currentComponentName);
                    // Check if the code coverage folder exists
                    if(Directory.Exists(currentMetric1Directory))
                    {
                            DirectoryInfo logFiles = new DirectoryInfo(currentMetric1Directory);
                            // Iterate through each code coverage log file and calculate the metric
                            foreach (FileInfo logFile in logFiles.GetFiles())
                            {
                                currFile = Path.Combine(currentMetric1Directory, logFile.Name);
                                // Archive the log file if returned true
                                coverageID = _codeCoverageMetric.CalculateMetric(currentProductName, currentComponentName, currFile, currIteration.IterationID);
                                if(coverageID >= 0)
                                {
                                    uniqueFileName = BuildUniqueFilename(logFile.Name, coverageID);
                                    RenameFile(currFile, Path.Combine(currentMetric1Directory, uniqueFileName));
                                    DatabaseAccessor.UpdateCoverage(coverageID, logFile.LastWriteTimeUtc.Date, uniqueFileName );
                                    ArchiveFile(currentProductName, currentComponentName, uniqueFileName);
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
                        _defectMetrics.CalculateMetric(currentProductName, currentComponentName, currIteration);
                    // --------------------------------------------------------------------
                    // END METRIC 3 AND 4
                    // --------------------------------------------------------------------
                }
            }
            // ---------------------------------------------------------------------
            // COMPUTE METRIC 5 AND 6 - RESOURCE UTILIZATION AND OUT OF SCOPE WORK
            // ---------------------------------------------------------------------
            if(_productDataDirectory!=null)
            {
                DirectoryInfo productDataList = new DirectoryInfo(_productDataDirectory);
                // Iterate through each product data .xls file to calculate resource utilization
                foreach (FileInfo productData in productDataList.GetFiles())
                {
                    _resourceUtilization.CalculateMetric(Path.Combine(_productDataDirectory,productData.Name), currIteration);
                    _outOfScopeWork.CalculateMetric(Path.Combine(_productDataDirectory, productData.Name), currIteration);
                }
            }
            // ---------------------------------------------------------------------
            // END METRIC 5
            // ---------------------------------------------------------------------
            initialDirectory = new DirectoryInfo(_testDirectory);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Iteration UpdateIteration()
        {
            int year = DateTime.Today.Year;
            string yearText;
            string iterationLabel;

            Iteration lastIteration = DatabaseAccessor.GetLastIteration();

            if (lastIteration == null)
            {
                year = _iterationStart.Year;
                yearText = year.ToString();
                yearText = yearText.Substring(yearText.Length - 2);
                iterationLabel = string.Concat(yearText, '-', DetermineIterationLetter(_iterationStart));

                DatabaseAccessor.WriteIteration(_iterationStart, GetIterationEnd(_iterationStart), iterationLabel);
                lastIteration = DatabaseAccessor.GetLastIteration();
            }
            else
            {
                var endPreviousIteration = (DateTime)lastIteration.EndDate;
                if (endPreviousIteration.Date < DateTime.UtcNow.Date)
                {
                    //determine beggining and end dates of current
                    DateTime startDate = GetIterationStart(endPreviousIteration);
                    DateTime endDate = GetIterationEnd(startDate);

                    //get year text
                    year = _iterationStart.Year;
                    yearText = year.ToString();
                    yearText = yearText.Substring(yearText.Length - 2);
                    iterationLabel = string.Concat(yearText, '-', DetermineIterationLetter(startDate));
                    DatabaseAccessor.WriteIteration(startDate, GetIterationEnd(GetIterationStart(endDate)), iterationLabel);
                    lastIteration = DatabaseAccessor.GetLastIteration();
                }
            }

            return lastIteration;
        }

        public char DetermineIterationLetter(DateTime iterationStart)
        {
            char[] alpha = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            DateTime today = DateTime.Today;
            int weekNum = 1;
            int iterationNum = 1;

            weekNum = GetWeekNumber(iterationStart);

            iterationNum = weekNum/2;

            return alpha[iterationNum - 1];
        }

        public static DateTime GetIterationStart(DateTime endOfLastIteration)
        {
            int i = 1;
            while(endOfLastIteration.AddDays(i).DayOfWeek != DayOfWeek.Monday)
            {
                i++;
            }

            return endOfLastIteration.AddDays(i);
        }

        public static DateTime GetIterationEnd(DateTime beginOfCurrIteration)
        {
            int i = 1;
            DateTime endOfIteration = beginOfCurrIteration.AddDays(7);
            int currYear = beginOfCurrIteration.Year;
            while (endOfIteration.AddDays(i).DayOfWeek != DayOfWeek.Friday)
            {
                if (endOfIteration.AddDays(i).Year > currYear)
                    break;
                else
                    i++;
            }

            return endOfIteration.AddDays(i);
        }


        public static int GetWeekNumber(DateTime day)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            int weekNum = ciCurr.Calendar.GetWeekOfYear(day, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
            return weekNum;
        }

        public static int FirstMondayOfYear(int year)
        {
            int day = 0;

            while ((new DateTime(year, 01, ++day)).DayOfWeek != DayOfWeek.Monday) ;

            return day;
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
        /// <param name="product">Current product</param>
        /// <param name="component">Current component</param>
        /// <param name="file">File to be saved</param>
        public bool ArchiveFile(string product, string component, string file)
        {
            string archMetric1Directory = Path.Combine(_rootArchiveDirectory, product, component, "Metric1");
            string currMetric1Directory = Path.Combine(_rootDirectory, product, component, "Metric1");

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

                // Iterate through the products in the directory
                foreach (DirectoryInfo product in initialDirectory.GetDirectories())
                {
                    // Save the current products name
                    string archProductName = product.Name;
                    // Iterate through the selected products components
                    foreach (DirectoryInfo component in product.GetDirectories())
                    {
                        // Save the current components name
                        string archComponentName = component.Name;

                        string archiveMetric1Directory = Path.Combine(_rootArchiveDirectory, archProductName, archComponentName, "Metric1");

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
    }
}