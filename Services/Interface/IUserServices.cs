using Services.DTOs;

namespace Services.Interface
{
    public interface IUserServices
    {
        int Authenticate(string username, string password);
        string Create(UserDTO userDto);
        RegisterDTO Register(string username, string hash, string name, string email);
    }
}
