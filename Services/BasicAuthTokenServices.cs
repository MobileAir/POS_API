using System;
using System.Configuration;
using System.Linq;
using DataAccess.Models;
using DataAccess.UnitOfWork;
using Services.DTOs;

namespace Services
{
    public class BasicAuthTokenServices : IBasicAuthTokenServices
    {
        #region Private member variables.
        private readonly UnitOfWork _unitOfWork;
        #endregion

        #region Public constructor.
        /// <summary>
        /// Public constructor.
        /// </summary>
        public BasicAuthTokenServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion


        #region Public member methods.

        /// <summary>
        ///  Function to generate unique token with expiry against the provided userId.
        ///  Also add a record in database for generated token.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public TokenDTO GenerateToken(int userId)
        {
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = DateTime.Now;

            // TODO set for 24hours?
            DateTime expiredOn = DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            var tokendomain = new Token
            {
                UserId = userId,
                AuthToken = token,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn,
                Request = 1
            };

            _unitOfWork.TokenRepository.Insert(tokendomain);
            _unitOfWork.Save();
            var tokenModel = new TokenDTO()
            {
                UserId = userId,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn,
                AuthToken = token,
                Request = 1
            };

            return tokenModel;
        }

        /// <summary>
        /// Method to validate token against expiry and existence in database.
        /// </summary>
        /// <param name="tokenId"></param>
        /// <returns></returns>
        public bool ValidateToken(string tokenId)
        {
            var token = _unitOfWork.TokenRepository.Get(t => t.AuthToken == tokenId && t.ExpiresOn > DateTime.Now);
            
            if (token != null && !(DateTime.Now > token.ExpiresOn))
            {
                var user = _unitOfWork.UserRepository.Get(x => x.UserId == token.UserId);

                if (user != null && token.Request <= user.RequestAllowed && !user.IsSuperUser)
                {
                    // this would add 15 min on every calls...with token
                    //token.ExpiresOn = token.ExpiresOn.AddSeconds(
                    //                              Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));

                    // TODO remove the below once set to 24 hours?
                    // this add configured min based on now when new resouirces requested su\ccessfully with token
                    token.ExpiresOn =
                        DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
                    token.Request = token.Request + 1; // SuperUser never get the increment!
                    _unitOfWork.TokenRepository.Update(token);
                    _unitOfWork.Save();
                    return true;
                }

                if (user != null && user.IsSuperUser) //  SuperUser never get the increment! => token.Request = token.Request + 1; 
                {
                    // this add configured min based on now when new resouirces requested su\ccessfully with token
                    token.ExpiresOn =
                        DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
                    _unitOfWork.TokenRepository.Update(token);
                    _unitOfWork.Save();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Method to kill the provided token id.
        /// </summary>
        /// <param name="tokenId">true for successful delete</param>
        public bool Kill(string tokenId)
        {
            _unitOfWork.TokenRepository.Delete(x => x.AuthToken == tokenId);
            _unitOfWork.Save();
            var isNotDeleted = _unitOfWork.TokenRepository.GetMany(x => x.AuthToken == tokenId).Any();
            if (isNotDeleted) { return false; }
            return true;
        }

        /// <summary>
        /// Delete tokens for the specific deleted user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>true for successful delete</returns>
        public bool DeleteByUserId(int userId)
        {
            _unitOfWork.TokenRepository.Delete(x => x.UserId == userId);
            _unitOfWork.Save();

            var isNotDeleted = _unitOfWork.TokenRepository.GetMany(x => x.UserId == userId).Any();
            return !isNotDeleted;
        }

        #endregion
    }
}