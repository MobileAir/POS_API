namespace BusinessEntities
{
    // TODO : Services should use this
    public class UserEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int RequestAllowed { get; set; }
    }
}
