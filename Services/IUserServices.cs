using Services.DTOs;

namespace Services
{
    public interface IUserServices
    {
        int Authenticate(string username, string password);
        string Create(UserDTO userDto);
    }
}
