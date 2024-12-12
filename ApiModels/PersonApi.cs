namespace MoviesApi.ApiModels
{
    public enum UserRole
    {
        RegularUser = 0,
        Admin = 1
    }
    // Base class for shared properties
    public class Person
    {
        public int Id { get; set; } // Use Id to generalize UserId and ActorId
        public string? Name { get; set; }
    }

    public class User : Person
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }

    public class Actor : Person
    {
        public string? PictureURL { get; set; }
    }
}
