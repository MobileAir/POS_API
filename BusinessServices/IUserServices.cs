using BusinessEntities;

namespace BusinessServices
{
    public interface IUserServices
    {
        int Authenticate(string userName, string password);
        string Create(UserEntity userEntity);
    }
}
