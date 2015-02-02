namespace Fanex.Ares.Core.Configurations
{
    using System.Configuration;

    /// <summary>
    /// Application settings.
    /// </summary>
    public static class AppSettings
    {
        static AppSettings()
        {
            IsLocalTest = ConfigurationManager.AppSettings["IsLocalTest"] == "1";
        }

        public static bool IsLocalTest { get; private set; }
    }
}