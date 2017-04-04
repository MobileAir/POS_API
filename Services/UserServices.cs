using System;
using System.Security.Cryptography;
using DataAccess.Models;
using DataAccess.UnitOfWork;
using Services.DTOs;

namespace Services
{
    /// <summary>
    /// Offers services for user specific operations
    /// </summary>
    public class UserServices : IUserServices
    {
        private readonly UnitOfWork _unitOfWork;

        /// <summary>
        /// Public constructor.
        /// </summary>
        public UserServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Public method to authenticate user by user name and password.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int Authenticate(string userName, string password)
        {
            var user = _unitOfWork.UserRepository.Get(u => u.Username == userName /*&& u.Password == password*/);
            if (user != null && user.UserId > 0)
            {
                var storedPassword = user.Password;
                var salt = user.Salt;
                //Hash the given password and compare it to a stored password
                var hashPassword = HashPassword(password, salt);
                if (hashPassword == storedPassword)
                    return user.UserId;
            }
            return 0;
        }

        /// <summary>
        /// Creates a hash of a given password and salt
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        private static string HashPassword(string password, string salt)
        {
            var sha = System.Security.Cryptography.SHA512.Create();
            var computedHash = sha.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password + salt));
            return Convert.ToBase64String(computedHash);
        }
        /// <summary>
        /// Encode string to Base64
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string ToBase64(string value)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(value));
        }
        /// <summary>
        /// Decode Base64 string
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string FromBase64(string value)
        {
            var bytes = Convert.FromBase64String(value);
            return System.Text.Encoding.UTF8.GetString(bytes);
        }


        public string Create(UserDTO userDto)
        {
            try
            {
                var exist = _unitOfWork.UserRepository.GetFirst(x => x.Username == userDto.Username);
                if (exist != null) return "Username already exist";

                var user = new User();

                var salt = GenerateSaltValue();
                //salt = ToBase64(salt);
                var hashedPassword = HashPassword(userDto.Password, salt);

                user.Username = userDto.Username;
                user.Salt = salt;
                user.Password = hashedPassword;
                user.RequestAllowed = 10;
                user.Name = "Temporary Name";
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Save();

                return "Success";
            }
            catch (Exception e)
            {
                // Log
                Console.WriteLine(e);
            }

            return "Fail";
        }
        
        private string GenerateSaltValue()
        {
            var random = new RNGCryptoServiceProvider();

            // Maximum length of salt
            int max_length = 32;

            // Empty salt array
            byte[] salt = new byte[max_length];

            // Build the random bytes
            random.GetBytes(salt);

            // Return the string encoded salt
            // return Convert.ToBase64String(salt); /// this conversion doesn't work with the hashPassword logic
            return System.Text.Encoding.UTF8.GetString(salt);

            // work fine but less safe
            //var chars = @"0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ<>?,./;:[]{}|\!@#$%^&*()-_=+";
            //var r = new Random();
            //var saltLength = r.Next(10, 16);
            //var arr = new char[saltLength];
            //for (var j = 0; j < saltLength; j++)
            //{
            //    arr[j] = chars[r.Next(0, 90)];
            //}
            //return new string(arr);
        }
    }
}
