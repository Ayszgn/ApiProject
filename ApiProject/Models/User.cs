namespace ApiProject.Models
{
    public class User
    {
        public int Id { get; set; }
        public required string UserName { get; set; } = null!;
        public required string Password { get; set; } = null!;
        public required string Role { get; set; } = null!;

    }
}
