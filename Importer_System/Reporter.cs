using System;
using System.IO;

namespace MetricAnalyzer.ImporterSystem
{
    public static class Reporter
    {
        private static TextWriter tw = null;       // output log file

        /// <summary>
        ///     Adds a success message to the report file and appends the date and time to it
        /// </summary>
        /// <param name="message"></param>
        public static void AddSuccessMessageToReporter(string message)
        {
            try
            {
                // write a line of text to the file
                tw.WriteLine(DateTime.Now + ",Status: Ok,Message: " + message);
            }
            catch (Exception) { }
        }

        /// <summary>
        ///     Adds an error message to the report file and appends the date and time to it
        /// </summary>
        /// <param name="message"></param>
        public static void AddErrorMessageToReporter(string message)
        {
            try
            {
                // write a line of text to the file
                tw.WriteLine(DateTime.Now + ",Status: Error,Message: " + message);
            }
            catch (Exception) { }
        }

        /// <summary>
        ///     Adds a terminating message to the report file and appends the date and time to it
        /// </summary>
        /// <param name="message"></param>
        public static void AddTerminateMessageToReporter(string message)
        {
            try
            {
                // write a line of text to the file
                tw.WriteLine(DateTime.Now + ",Status: Error,Message: " + message + " The program will now terminate.");
            }
            catch (Exception) { }
        }

        /// <summary>
        ///     Opens the report log file to write messages to.
        /// </summary>
        public static void OpenReporter()
        {
            try
            {
                // create a writer and open the file
                tw = new StreamWriter("metric.log", true);
                // indicate that it has started
                AddSuccessMessageToReporter("Program successfully started");
            }
            catch (Exception) { }
        }

        /// <summary>
        ///     Closes the report log file.
        /// </summary>
        public static void CloseReporter()
        {
            try
            {
                // indicate that it has stopped
                AddSuccessMessageToReporter("Program successfully terminated");
                // close the stream
                tw.Close();
            }
            catch (Exception) { }
        }
    }
}
