namespace Fanex.Ares.Invoice.Models.Invoice
{
    using System;

    /// <summary>
    /// Product Class.
    /// </summary>
    [Serializable]
    public class Product
    {
        public int ProductID { get; set; }

        public string ProductName { get; set; }
    }
}
