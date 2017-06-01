using Services.DTOs;

namespace Services.Interface
{
    public interface ITokenAuthServices
    {
        #region Interface member methods.

        /// <summary>
        /// Checks if a token is valid.
        /// </summary>
        /// <param name="token">string - generated either by GenerateToken() or via client with cryptojs etc.</param>
        /// <param name="userAgent">string - user-agent of client, passed in by RESTAuthenticate attribute on controller.</param>
        /// <returns>bool</returns>
        UserDTO IsTokenValid(string token,string userAgent);

        #endregion
    }
}
