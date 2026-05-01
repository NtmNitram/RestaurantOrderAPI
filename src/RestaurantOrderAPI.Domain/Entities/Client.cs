namespace RestaurantOrderAPI.Domain.Entities;

public class Client
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Tipo { get; set; } = "Plaza";
    public string? LocalNumber { get; set; }
    public string? Referencia { get; set; }
    public string? PhoneNumber { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
