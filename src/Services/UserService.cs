using System;
using System.Linq;
using System.Text.RegularExpressions;
using TwoNil.Data;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Services
{
   public class UserService : ServiceBase
   {
      private const string _emailValidationPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
      private Regex _emailAddressRegex = new Regex(_emailValidationPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

      public User GetUser(string username, string password)
      {
         // Validation: both username and password are required.
         if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
         {
            throw new ValidationException("Invalid login request");
         }

         var user = GetByUsername(username);

         if (user != null)
         {
            //TODO Verify the password hash.
         }

         return user;
      }

      public User GetUser(string userId)
      {
         using (var userRepository = new RepositoryFactory().CreateUserRepository())
         {
            return userRepository.GetOne(userId);
         }
      }

      public User CreateUser(string firstname, string lastname, string username, string email, string password)
      {
         // Validation: all properties are required, the password must be at least 8 characters long and the email address must have a valid syntax.
         if (string.IsNullOrWhiteSpace(firstname)
            || string.IsNullOrWhiteSpace(lastname)
            || string.IsNullOrWhiteSpace(username)
            || string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(password)
            || password.Length < 8
            || !_emailAddressRegex.IsMatch(email))
         {
            throw new ValidationException("Invalid user properties provided");
         }

         // The username must be unique.
         var existingUser = GetByUsername(username);
         if (existingUser != null)
         {
            throw new ConflictException($"Username '{username}' already exists");
         }

         var user = new User()
         {
            Firstname = firstname,
            Lastname = lastname,
            Username = username,
            Email = email,
            //TODO create Password hash
            PasswordHash = "moio"
         };

         using (var repository = new RepositoryFactory().CreateTransactionManager())
         {
            repository.RegisterInsert(user);
            repository.Save();
         }

         return user;
      }

      private User GetByUsername(string username)
      {
         using (var userRepository = new RepositoryFactory().CreateUserRepository())
         {
            return userRepository.Find(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
         }
      }
   }
}
