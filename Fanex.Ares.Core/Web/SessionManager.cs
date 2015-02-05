namespace Fanex.Ares.Core.Web
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;

    /// <summary>
    /// Session Manager.
    /// </summary>
    public static class SessionManager
    {
        /// <summary>
        /// Set Value to session with specific type.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="sessionKey">Session key.</param>
        /// <param name="value">Object value.</param>
        public static void Set<T>(string sessionKey, T value)
        {
            HttpContext.Current.Session[sessionKey] = value;
        }

        /// <summary>
        /// Get Value from Session with specific Type.
        /// </summary>
        /// <typeparam name="T">The parameter type.</typeparam>
        /// <param name="sessionKey">Session key.</param>
        /// <returns>Return result.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Reviewed")]
        public static T Get<T>(string sessionKey)
        {
            try
            {
                T value = (T)HttpContext.Current.Session[sessionKey];
                return value;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// Set object to Session.
        /// </summary>
        /// <param name="sessionKey">Session key.</param>
        /// <param name="value">Object value.</param>
        public static void Set(string sessionKey, object value)
        {
            SessionManager.Set<object>(sessionKey, value);
        }

        /// <summary>
        /// Get value object from Session.
        /// </summary>
        /// <param name="sessionKey">Session key.</param>
        /// <returns>Return result.</returns>
        public static object Get(string sessionKey)
        {
            return SessionManager.Get<object>(sessionKey);
        }
    }
}