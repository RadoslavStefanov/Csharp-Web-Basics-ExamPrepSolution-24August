using SMS.Contracts;
using SMS.Data.Common;
using SMS.Data.Models;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Sevices
{
    public class UserService : IUserService
    {
        private readonly IRepository repo;

        public UserService(IRepository _repo)
        { repo = _repo; }

        public string GetUsername(string userId)
        {
            return repo.All<User>()
                .FirstOrDefault(u => u.Id == userId)?.Username;
        }

        public string Login(LoginViewModel model)
        {
            var user = repo.All<User>()
                .Where(u => u.Username == model.Username)
                .Where(u => u.Password == CalculateHash(model.Password))
                .SingleOrDefault();

            return user?.Id;
        }

        public (bool registered, string error) Register(RegisterViewModel model)
        {
            bool registered = false; string error = null; 

            var(isValid,validationError)=ValidateRegisterModel(model);

            if (!isValid)
            {
                return (isValid, validationError);
            }

            Cart cart = new Cart();

            User user = new User()
            {
                Email = model.Email,
                Password = CalculateHash(model.Password),
                Username = model.Username,
                Cart = cart,
                CartId = cart.Id
            };

            try
            {
                repo.Add(user);
                repo.SaveChanges();
                registered = true;
            }
            catch (Exception)
            {
                error = "Couldn't save user in DB!";
            }

            return (registered, error);
        }

        private string CalculateHash(string password)
        {
            byte[] passworArray = Encoding.UTF8.GetBytes(password);

            using (SHA256 sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(sha256.ComputeHash(passworArray));
            }
        }

        private (bool isValid, string error) ValidateRegisterModel(RegisterViewModel model)
        {
            bool isValid = true;

            StringBuilder error = new StringBuilder();

            if (model == null)
            {
                return (false, "Register model is required!");
            }

            if (String.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 5 || model.Username.Length > 20)
            {
                isValid = false;
                error.AppendLine("Username must be between 5 and 20 charecters!");
            }

            if (String.IsNullOrWhiteSpace(model.Email))
            {
                isValid = false;
                error.AppendLine("The provided Email is not valid!");
            }

            if (String.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6 || model.Password.Length > 20)
            {
                isValid = false;
                error.AppendLine("Password must be between 6 and 20 charecters!");
            }

            if (model.Password != model.ConfirmPassword)
            {
                isValid = false;
                error.AppendLine("Passwords don't match!");
            }

            return (isValid, error.ToString());

        } 

    }
}
