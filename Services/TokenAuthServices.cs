﻿using System;
using System.Security.Cryptography;
using System.Text;
using DataAccess.UnitOfWork;
using Services.Interface;

namespace Services
{
    public class TokenAuthServices : ITokenAuthServices
    {
        #region Private member variables.
        private readonly UnitOfWork _unitOfWork;
        private const string _alg = "HmacSHA256";
        private static int _expirationMinutes = 10; // TODO:  should be in config or db
        #endregion

        #region Public constructor.
        /// <summary>
        /// Public constructor.
        /// </summary>
        public TokenAuthServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #endregion


        #region Public member methods.

        public bool IsTokenValid(string token,string userAgent)
        {
            bool result = false;

            try
            {
                // Base64 decode the string, obtaining the token:username:timeStamp.
                string key = Encoding.UTF8.GetString(Convert.FromBase64String(token));

                // Split the parts.
                string[] parts = key.Split(new char[] { ':' });
                if (parts.Length == 3)
                {
                    // Get the hash message, username, and timestamp.
                    string hash = parts[0];
                    string username = parts[1];
                    long ticks = long.Parse(parts[2]);
                    DateTime timeStamp = new DateTime(ticks);

                    // Ensure the timestamp is valid.
                    bool expired = Math.Abs((DateTime.UtcNow - timeStamp).TotalMinutes) > _expirationMinutes;
                    if (!expired)
                    {
                        //
                        // Lookup the user's account from the db.
                        //
                        var user = _unitOfWork.UserRepository.GetSingle(u => u.Username == username);

                        if (user != null)
                        {
                            var hashedPasword = user.Password;

                            // Hash the message with the key to generate a token.
                            string computedToken = GenerateToken(username, hashedPasword, userAgent, ticks);

                            // Compare the computed token with the one supplied and ensure they match.
                            result = (token == computedToken);
                        }
                    }
                }
            }
            catch
            {
            }

            return result;
        }

        /// <summary>
        /// Generates a token to be used in API calls.
        /// The token is generated by hashing a message with a key, using HMAC SHA256.
        /// The message is: username:ip:userAgent:timeStamp
        /// The key is: password:ip:salt
        /// The resulting token is then concatenated with username:timeStamp and the result base64 encoded.
        /// 
        /// API calls may then be validated by:
        /// 1. Base64 decode the string, obtaining the token, username, and timeStamp.
        /// 2. Ensure the timestamp is not expired.
        /// 2. Lookup the user's password from the db (cached).
        /// 3. Hash the username:ip:userAgent:timeStamp with the key of password:salt to compute a token.
        /// 4. Compare the computed token with the one supplied and ensure they match.
        /// </summary>
        /// TODO: add custom route
        public string GenerateToken(string username, string password, string userAgent, long ticks)
        {
            string hash = string.Join(":", new string[] { username, userAgent, ticks.ToString() });
            string hashLeft = "";
            string hashRight = "";

            using (HMAC hmac = HMACSHA256.Create(_alg))
            {
                hmac.Key = Encoding.UTF8.GetBytes(password); // Already hashed in db !! => Encoding.UTF8.GetBytes(GetHashedPassword(password));
                hmac.ComputeHash(Encoding.UTF8.GetBytes(hash));

                hashLeft = Convert.ToBase64String(hmac.Hash);
                hashRight = string.Join(":", new string[] { username, ticks.ToString() });
            }
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Join(":", hashLeft, hashRight)));
        }

        #endregion
    }
}