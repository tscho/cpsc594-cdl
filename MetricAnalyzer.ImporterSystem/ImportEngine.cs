using System;
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
using MetricAnalyzer.Common.Models;
using MetricAnalyzer.ImporterSystem.Metrics;
using Rework = MetricAnalyzer.Common.Models.Rework;
using VelocityTrend = MetricAnalyzer.Common.Models.VelocityTrend;
using System.Collections.ObjectModel;
using MetricAnalyzer.ImporterSystem.Gui;


namespace MetricAnalyzer.ImporterSystem
{

    class ImportEngine
    {
        private string _rootDirectory = null;                            // Root directory of the folders
        private string _rootArchiveDirectory = null;              // Archive directory for files after being read
        private string _productDataDirectory = null;              // Directory containing .xls product data files
        private int _archivePeriod;                               // Number of days the logfiles exist in the archive directory
        private string _outputDatabaseConnection;                 // Connection string to output the information to
        private DateTime _iterationStart;                         // Begin date of iteration
        private CodeCoverage _codeCoverageMetric;                 // Class that calculates code coverage
        private DefectMetrics _defectMetrics;                     // Class that calculates the injection rate and repair rate
        private TestEffectivenessMetric _testEffectivenessMetric; // Class that calculates the test effectiveness
        private ResourceUtilizationMetric _resourceUtilization;   // Class that calculates work hours per product
        private OutOfScopeWorkMetric _outOfScopeWork;             // Calculates out of scope work
        private VelocityTrendMetric _velocityTrend;               // Calculates velocity trend metric
        private ReworkMetric _rework;                             // Calculates rework
        private ConnectionStringSettings _outputDbSettings;       // Main database which stores all the metric data
        public enum Metrics { CodeCoverage, TestEffectiveness, DefectInjectionRate, DefectRepairRate, ResourceUtilization, OutOfScopeWork, Rework, VelocityTrend };
        List<Iteration> importedIterations = new List<Iteration>();

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
            //Create metric 7
            _rework  = new ReworkMetric();
            //create metric 8
            _velocityTrend = new VelocityTrendMetric();
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
            // _outputDatabase, _outputDbSettings
            ValidateOutputDatabaseConnection(ConfigurationManager.ConnectionStrings["MetricAnalyzerEntities"]);
            // _bugzillaDatabaseConnection, bugzillaDbSettings
            ValidateBugzillaDatabaseConnection(ConfigurationManager.ConnectionStrings["BugzillaDatabase"]);
            // _productDataDirectory
            ValidateProductDataDirectory(ConfigurationManager.AppSettings["ProductData"]);
            //_iterationStartDate
            ValidateIterationStart(ConfigurationManager.AppSettings["FirstIteration"]);
        }

        private void ValidateIterationStart(string iteration)
        {
            string[] items = iteration.Split('/');
            try
            {
                _iterationStart = new DateTime(Int16.Parse(items[2]), Int16.Parse(items[1]), Int16.Parse(items[0]));
            }
            catch (IndexOutOfRangeException)
            {
                _iterationStart = DateTime.Now;
            }
            catch (NullReferenceException)
            {
                _iterationStart = DateTime.Now;
            }
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
        ///     Does not terminate not program if error.
        /// </summary>
        /// <param name="path"></param>
        private void ValidateRootDirectory(string path)
        {
            if (Directory.Exists(path))
                _rootDirectory = path;
            else
                Reporter.AddErrorMessageToReporter("Root directory does not exist or was not specified. The program will skip [Code Coverage, TestEffectives, DefectRepairRate, DefectInjectionRate]");
        }

        /// <summary>
        ///     ValidateRootArchiveDirectory - Checks whether the archive directory exists, if not set it to null.
        ///     Does not terminate program if error.
        /// </summary>
        /// <param name="path"></param>
        private void ValidateRootArchiveDirectory(string path)
        {
            // If the directory exists OR the directory is not specified
            if (Directory.Exists(path) || path.Length == 0)
                _rootArchiveDirectory = path;
            else
                Reporter.AddErrorMessageToReporter("Archive directory does not exist or was not specified. The program will skip the archiving process.");
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
        ///     Does not terminate if error.
        /// </summary>
        /// <param name="connectionSettings"></param>
        private void ValidateBugzillaDatabaseConnection(ConnectionStringSettings connectionSettings)
        {
            string connectionString = connectionSettings.ConnectionString;
            _defectMetrics.SetConnectionString(connectionString);
            if (!_defectMetrics.EstablishConnection())
                Reporter.AddErrorMessageToReporter("Could not establish a connection to the bugzilla database using connection string: " + connectionString + ". The program will skip Metrics [DefectInjectionRate, DefectRepairRate]");
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
        public void BeginImporting(ObservableCollection<DisplayMetric> metricList)
        {
            string currFile;
            int coverageID = -1;
            Iteration currIteration;
            currIteration = UpdateIteration();

            if (_rootDirectory != null)
            {
                // Make directory structure
                DirectoryInfo initialDirectory = new DirectoryInfo(_rootDirectory);

                // Iterate through the Products in the code coverage directory
                if (initialDirectory.GetDirectories().Count() != 0)
                {
                    foreach (DirectoryInfo product in initialDirectory.GetDirectories())
                    {
                        // Save the current Products name
                        string currentProductName = product.Name;

                        // Check if Product is in DB
                        if (!DatabaseAccessor.ProductExists(currentProductName))
                        {
                            DatabaseAccessor.WriteProduct(currentProductName);
                        }

                        // ---------------------------------------------------------------------
                        // COMPUTE METRIC 2 - Value for Tests
                        // ---------------------------------------------------------------------
                        DateTime fileDate = DateTime.MinValue;
                        string productDirectory = Path.Combine(_rootDirectory, currentProductName);
                        DirectoryInfo testFiles = new DirectoryInfo(productDirectory);

                        if (testFiles.GetFiles().Count() != 0)
                        {
                            foreach (FileInfo testFile in testFiles.GetFiles())
                            {
                                if (fileDate.CompareTo(testFile.LastWriteTime) == -1)
                                {
                                    fileDate = testFile.LastWriteTime;
                                    currFile = Path.Combine(productDirectory, testFile.Name);
                                    if (
                                        _testEffectivenessMetric.CalculateMetric(currFile, currIteration.IterationID,
                                                                                 currentProductName) == -1)
                                        upadateMetricStatus(metricList, (int) Metrics.TestEffectiveness, "Errors existed during value for tests import, see log file.");
                                }
                            }
                        }
                        else
                        {
                            // If the format of the excel file is not correct
                            Reporter.AddErrorMessageToReporter(
                                "No files exist to parse for value of tests in directory: " + productDirectory);
                            upadateMetricStatus(metricList, (int) Metrics.TestEffectiveness,
                                                "Errors existed during value for tests import, see log file.");
                        }


                        // Iterate through the selected Products components;
                        foreach (DirectoryInfo component in product.GetDirectories())
                        {
                            // Save the current components name
                            string currentComponentName = component.Name;

                            // Check if component is in DB
                            if (!DatabaseAccessor.ComponentExists(currentProductName, currentComponentName))
                            {
                                DatabaseAccessor.WriteComponent(currentProductName, currentComponentName);
                            }
                            // ---------------------------------------------------------------------
                            // COMPUTE METRIC 1 - CODE COVERAGE
                            // ---------------------------------------------------------------------
                            string currentMetric1Directory = Path.Combine(_rootDirectory, currentProductName,
                                                                          currentComponentName);
                            // Check if the code coverage folder exists
                            if (Directory.Exists(currentMetric1Directory))
                            {
                                DirectoryInfo logFiles = new DirectoryInfo(currentMetric1Directory);
                                fileDate = DateTime.MinValue;
                                // Iterate through each code coverage log file and calculate the metric
                                foreach (FileInfo logFile in logFiles.GetFiles())
                                {
                                    if (fileDate.CompareTo(logFile.LastWriteTime) == -1)
                                    {
                                        fileDate = logFile.LastWriteTime;
                                        currFile = Path.Combine(currentMetric1Directory, logFile.Name);
                                        // Archive the log file if returned true
                                        coverageID = _codeCoverageMetric.CalculateMetric(currentProductName,
                                                                                         currentComponentName, currFile,
                                                                                         currIteration.IterationID,
                                                                                         logFile.Name);
                                        if (coverageID >= 0)
                                        {
                                            // If proper directory is specified
                                            if (_rootArchiveDirectory != null)
                                                ArchiveFile(currentProductName, currentComponentName, logFile.Name);
                                            else
                                                logFile.Delete();
                                        }
                                        else
                                        {
                                            upadateMetricStatus(metricList, (int) Metrics.CodeCoverage,
                                                                "Errors existed during coverage import, see log file.");
                                        }
                                    }
                                    coverageID = -1;
                                }
                            }
                            // --------------------------------------------------------------------
                            // END METRIC 1
                            // --------------------------------------------------------------------
                        }
                    }
                }
                else
                {
                    upadateMetricStatus(metricList, (int)Metrics.CodeCoverage, "Errors existed during coverage import, no products within input directory.");
                    upadateMetricStatus(metricList, (int)Metrics.TestEffectiveness, "Errors existed during value for tests import, no products within input directory.");
                }
            }
            else
            {
                upadateMetricStatus(metricList, (int)Metrics.CodeCoverage, "Errors existed during coverage import, no input directory configured.");
                upadateMetricStatus(metricList, (int)Metrics.TestEffectiveness, "Errors existed during value for tests import, no input directory configured.");
            }


            // ---------------------------------------------------------------------
            // COMPUTE METRIC 5,6,7,8
            // ---------------------------------------------------------------------
            if (_productDataDirectory != null)
            {
                DirectoryInfo productDataList = new DirectoryInfo(_productDataDirectory);
               if(productDataList.GetFiles().Count() != 0)
               {
                   //Iterate through each product data .xls to find any new products before we parse for data in the file))
                   foreach (FileInfo productData in productDataList.GetFiles())
                   {
                       findNewProducts(Path.Combine(_productDataDirectory, productData.Name));
                   }

                   if (importedIterations.Count == 0)
                       importedIterations.Add(currIteration);

                   // Iterate through each product data .xls file to calculate resource utilization
                   foreach (FileInfo productData in productDataList.GetFiles())
                   {
                       foreach (var iter in importedIterations)
                       {
                           //_outOfScopeWork.CalculateMetric(Path.Combine(_productDataDirectory, productData.Name), iter);
                           _resourceUtilization.CalculateMetric(Path.Combine(_productDataDirectory, productData.Name), iter);
                           //_velocityTrend.CalculateMetric(Path.Combine(_productDataDirectory, productData.Name), iter);
                       }
                       foreach (var iter in importedIterations)
                       {
                           _rework.CalculateMetric(Path.Combine(_productDataDirectory, productData.Name), iter);
                       }
                   }
               }
               else
               {
                   upadateMetricStatus(metricList, (int)Metrics.ResourceUtilization, "Errors existed during Resource Utilization import, no input files within directory.");
                   upadateMetricStatus(metricList, (int)Metrics.OutOfScopeWork, "Errors existed during Out of Scope Work import, no input files within directory.");
                   upadateMetricStatus(metricList, (int)Metrics.Rework, "Errors existed during Rework import, no input files within directory.");
                   upadateMetricStatus(metricList, (int)Metrics.VelocityTrend, "Errors existed during Velocity Trend import, no input files within directory.");
               }    
            }
            else
            {
                upadateMetricStatus(metricList, (int)Metrics.ResourceUtilization, "Errors existed during Resource Utilization import, no input directory configured.");
                upadateMetricStatus(metricList, (int)Metrics.OutOfScopeWork, "Errors existed during Out of Scope Work import, no input directory configured.");
                upadateMetricStatus(metricList, (int)Metrics.Rework, "Errors existed during Rework import, no input directory configured.");
                upadateMetricStatus(metricList, (int)Metrics.VelocityTrend, "Errors existed during Velocity Trend import, no input directory configured.");
            }
            // ---------------------------------------------------------------------
            // END METRIC 5,6,7,8
            // --------------------------------------------------------------------- 

            // ---------------------------------------------------------------------
            // COMPUTE METRIC 3 AND 4 - DEFECTINJECTIONRATE AND DEFECTREPAIRRATE
            // ---------------------------------------------------------------------
            if (_defectMetrics.GetConnection() != null)
            {
                List<Product> productList = DatabaseAccessor.GetProducts();
                foreach (var currProduct in productList)
                {
                    List<Component> componentList = DatabaseAccessor.GetComponents(currProduct.ProductID);
                    foreach (var currComponent in componentList)
                    {
                        _defectMetrics.CalculateMetric(currProduct.ProductName, currComponent.ComponentName, currIteration);
                    }
                }
            }
            else
            {
                upadateMetricStatus(metricList, (int)Metrics.DefectInjectionRate, "Errors existed during Defect Injection Rate import, no connection to bugzilla database.");
                upadateMetricStatus(metricList, (int)Metrics.DefectRepairRate, "Errors existed during Defect Repair Rate import, no connection to bugzilla database.");
            }
            // --------------------------------------------------------------------
            // END METRIC 3 AND 4
            // --------------------------------------------------------------------
        }


        private void findNewProducts(string file)
        {
            // Excel connection string
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + file + ";Extended Properties=Excel 5.0";
            // Get excel reader
            ExcelReader xlsReader = new ExcelReader(connectionString);
            if (xlsReader.CheckConnection())
            {
                try
                {
                    string query = String.Concat("Select distinct [Product] from [Sheet1$]");

                    List<string[]> productList = xlsReader.SelectQuery(query);
                    foreach (string[] row in productList)
                    {
                        string productName = row[0];
                        if(!DatabaseAccessor.ProductExists(productName))
                        {
                            DatabaseAccessor.WriteProduct(productName);
                        }
                    }
                }
                catch
                {
                    // If the format of the excel file is not correct
                    Reporter.AddErrorMessageToReporter("Product data file cannot properly be parsed due to its columns " +  file);
                }
            }
            else
                Reporter.AddErrorMessageToReporter("Unable to open product data file " + file);
        }

        public void upadateMetricStatus(ObservableCollection<DisplayMetric> metricList, int id, string status)
        {
            metricList.ElementAt(id).Status = status;
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
                DateTime endOfIteration;
                do
                {
                    year = _iterationStart.Year;
                    yearText = year.ToString();
                    yearText = yearText.Substring(yearText.Length - 2);
                    iterationLabel = string.Concat(yearText, '-', DetermineIterationLetter(_iterationStart));
                    endOfIteration = GetIterationEnd(_iterationStart);
                    Iteration currentImportIteration = DatabaseAccessor.WriteIteration(_iterationStart, endOfIteration, iterationLabel);
                    if (currentImportIteration != null)
                        importedIterations.Add(currentImportIteration);

                    _iterationStart = GetIterationStart(endOfIteration);
                } while (endOfIteration < DateTime.Now);

                lastIteration = DatabaseAccessor.GetLastIteration();
            }
            else
            {
                var endPreviousIteration = (DateTime)lastIteration.EndDate;
                if (endPreviousIteration.Date < DateTime.UtcNow.Date)
                {
                    DateTime startDate;
                    DateTime endDate;
                    //determine beggining and end dates of current
                    do
                    {
                        startDate = GetIterationStart(endPreviousIteration);
                        endDate = GetIterationEnd(startDate);

                        //get year text
                        year = _iterationStart.Year;
                        yearText = year.ToString();
                        yearText = yearText.Substring(yearText.Length - 2);
                        iterationLabel = string.Concat(yearText, '-', DetermineIterationLetter(startDate));
                        Iteration currentImportIteration = DatabaseAccessor.WriteIteration(startDate, endDate, iterationLabel);
                        if (currentImportIteration != null)
                            importedIterations.Add(currentImportIteration);
                        endPreviousIteration = endDate;

                    } while (endDate < DateTime.Now);

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
            iterationNum =  (int)Math.Ceiling((double)weekNum/2);
            
            return alpha[iterationNum-1];
        }

        public static DateTime GetIterationStart(DateTime endOfLastIteration)
        {
            int i = 1;

            while (endOfLastIteration.AddDays(i).DayOfWeek != DayOfWeek.Monday && endOfLastIteration.AddDays(i).DayOfWeek != DayOfWeek.Tuesday && endOfLastIteration.AddDays(i).DayOfWeek != DayOfWeek.Wednesday && endOfLastIteration.AddDays(i).DayOfWeek != DayOfWeek.Thursday && endOfLastIteration.AddDays(i).DayOfWeek != DayOfWeek.Friday)
            {
                i++;
            }
            DateTime iterationStart = endOfLastIteration.AddDays(i);

            return iterationStart;
        }

        public static DateTime GetIterationEnd(DateTime beginOfCurrIteration)
        {
            int i = 1;
            DateTime endOfIteration = beginOfCurrIteration.AddDays(7);
            int currYear = beginOfCurrIteration.Year;
            if(endOfIteration.Year > beginOfCurrIteration.Year)
            {
                i = -1;
                while (endOfIteration.AddDays(i).DayOfWeek != DayOfWeek.Monday && endOfIteration.AddDays(i).DayOfWeek != DayOfWeek.Tuesday && endOfIteration.AddDays(i).DayOfWeek != DayOfWeek.Wednesday && endOfIteration.AddDays(i).DayOfWeek != DayOfWeek.Thursday && endOfIteration.AddDays(i).DayOfWeek != DayOfWeek.Friday)
                {
                    i--;
                }
            }
            else
            {
                while (endOfIteration.AddDays(i).DayOfWeek != DayOfWeek.Friday)
                {
                    if (endOfIteration.AddDays(i).Year > currYear)
                    {
                        i--;
                        break;
                    }
                    else
                        i++;
                }
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
