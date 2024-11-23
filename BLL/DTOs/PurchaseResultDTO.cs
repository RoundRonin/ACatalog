namespace BLL.DTOs
{
    public class PurchaseResultDTO
    {
        // Indicates whether the purchase was successful.
        public bool IsSuccess { get; set; }

        // The total cost of the purchased goods.
        public decimal TotalCost { get; set; }

        // Message providing additional information about the purchase result.
        public string Message { get; set; }
    }
}
