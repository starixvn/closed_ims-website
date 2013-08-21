// -----------------------------------------------------------------------
// <copyright file="LoggingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Web;
using Fanex.Logging;

namespace Fanex.Ares.Core.Services.Impl
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LoggingHandler
    {
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
            Fanex.Logging.Logger.Log.Error(message);
        }

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

    public enum LogType: int
    {
        Info = 1,
        Error = 2
    }
        
}
