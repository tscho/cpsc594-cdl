using System;
using System.IO;

namespace Importer_System
{
    public static class Reporter
    {
        private static TextWriter tw = null;       // output log file

        /// <summary>
        ///     Adds a message to the report file and appends the date and time to it
        /// </summary>
        /// <param name="message"></param>
        public static void AddMessageToReporter(string message, Boolean error, Boolean terminate)
        {
            try
            {
                // write a line of text to the file
                string status = "Error";
                if (!error)
                    status = "Ok";
                tw.WriteLine(DateTime.Now + ", Status: " + status + ", Message: " + message + ", Terminate: " + terminate);
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
                AddMessageToReporter("Program successfully started", false, false);
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
                AddMessageToReporter("Program successfully terminated", false, false);
                // close the stream
                tw.Close();
            }
            catch (Exception) { }
        }
    }
}
