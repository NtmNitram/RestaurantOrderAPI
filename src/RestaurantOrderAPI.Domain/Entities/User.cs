namespace RestaurantOrderAPI.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Mesero"; // "Mesero" | "Dueño"
    public bool IsActive { get; set; } = true;
}
