namespace Services.DTOs
{
    public class UserDTO
    {
        public int UserId { get; set; }
        
        public string Username { get; set; }
        
        public string Salt { get; set; }
        
        public string Password { get; set; }
        
        public string Name { get; set; }

        public string Email { get; set; }

        public int RequestAllowed { get; set; }

        public bool IsSuperUser { get; set; }
    }
}
