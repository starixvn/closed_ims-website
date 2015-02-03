namespace Fanex.Artist.Services
{
    using System;
    using System.Globalization;

    /// <summary>
    /// Data access exception.
    /// </summary>
    [Serializable]
    public class ServiceException : Exception
    {
        private readonly string _uspName;
        private readonly dynamic _uspParam;

        public ServiceException(string message, string uspName, dynamic uspParam)
            : base(message)
        {
            this._uspName = uspName;
            this._uspParam = uspParam;
        }

        public ServiceException(string message, string uspName)
            : base(message)
        {
            this._uspName = uspName;
        }

        public ServiceException(string message)
            : base(message)
        {
            this._uspName = message;
        }

        public ServiceException()
            : base()
        {
            this._uspName = string.Empty;
        }

        public ServiceException(string message, Exception exception)
            : base()
        {
            this._uspName = message;
            this._uspParam = exception.Data.ToString();
        }

        public override string Message
        {
            get
            {
                return string.IsNullOrEmpty(this._uspName)
                    ? string.Format(CultureInfo.CurrentCulture, "Exception: {0}", base.Message)
                    : this._uspParam == null
                        ? string.Format(CultureInfo.CurrentCulture, "Exception when call usp {0}. Exception: {1}", this._uspName, base.Message)
                        : string.Format(CultureInfo.CurrentCulture, "Exception when call usp {0}. Params: {1}. Exception: {2}", this._uspName, this._uspParam, base.Message);
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}