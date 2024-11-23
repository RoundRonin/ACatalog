namespace BLL.DTOs;

public class ProductDTO
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }  
}
