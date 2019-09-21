namespace RMDataManager.Library.Models
{
    public class SaleDetailDBModel
    {
        // Matches the SaleDetail table.
        // Model is used to insert the data into a stored procedure.
        public int SaleId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal Tax { get; set; } 
    }
}
