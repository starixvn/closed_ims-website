namespace Fanex.Ares.Core.Services.Impl
{
    using System;
    using System.Web;
    using Fanex.Logging;

    public enum LogType : int
    {
        /// <summary>
        /// Log type - Default.
        /// </summary>
        None = 0,

        /// <summary>
        /// Log type - Info.
        /// </summary>
        Info = 1,

        /// <summary>
        /// Log type - Error.
        /// </summary>
        Error = 2
    }

    /// <summary>
    /// Logging Handler.
    /// </summary>
    public static class LoggingHandler
    {
        /// <summary>
        /// Write log.
        /// </summary>
        /// <param name="ex">Log - Exception.</param>
        public static void Log(Exception ex)
        {
            string message = ex.Message +
                             "<br>\r\nSOURCE: " + ex.Source +
                             "<br>\r\nDOMAIN: " + HttpContext.Current.Request.Headers["Host"] +
                             "<br>\r\nREFERRER: " + HttpContext.Current.Request.UrlReferrer +
                             "<br>\r\nFORM: " + HttpContext.Current.Request.Form.ToString() +
                             "<br>\r\nQUERYSTRING: " + HttpContext.Current.Request.QueryString.ToString() +
                             "<br>\r\nTARGETSITE: " + ex.TargetSite +
                             "<br>\r\nSTACKTRACE: " + ex.StackTrace;

            Logger.Log.Error(message);
        }

        /// <summary>
        /// Write log.
        /// </summary>
        /// <param name="message">Error message.</param>
        /// <param name="logType">The Log type.</param>
        public static void Log(string message = "", LogType logType = LogType.Error)
        {
            switch (logType)
            {
                case LogType.Info:
                    LogService.Info(message);
                    break;

                case LogType.Error:
                    LogService.Error(message);
                    break;
            }
        }
    }
}