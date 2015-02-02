namespace Fanex.Ares.Core.Models
{
    using System;

    /// <summary>
    /// Result Model.
    /// </summary>
    [Serializable]
    public class Result
    {
        public Result()
        {
            ErrorCode = 0;
            ErrorMessage = string.Empty;
        }

        public int ErrorCode { get; set; }

        public string ErrorMessage { get; set; }
    }
}
