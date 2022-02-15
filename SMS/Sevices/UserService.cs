using SMS.Contracts;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Sevices
{
    public class UserService : IUserService
    {
        public (bool registered, string error) Register(RegisterViewModel model)
        {
            var(isValid,error)=ValidateRegisterModel(model);

            if (!isValid)
            {
                return (isValid, error);
            }
        }

        private (bool isValid, string error) ValidateRegisterModel(RegisterViewModel model)
        {
            bool isValid = true;

            StringBuilder error = null;

            if (model == null)
            {
                return (false, "Register model is required!");
            }

            if (String.IsNullOrWhiteSpace(model.Username) || model.Username.Length <= 5 || model.Username.Length >= 20)
            {
                isValid = false;
                error.AppendLine("Username must be between 5 and 20 charecters!");
            }

            if (String.IsNullOrWhiteSpace(model.Email))
            {
                isValid = false;
                error.AppendLine("The provided Email is not valid!");
            }

            if (String.IsNullOrWhiteSpace(model.Password) || model.Password.Length <= 6 || model.Password.Length >= 20)
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
