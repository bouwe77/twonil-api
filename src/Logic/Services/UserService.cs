using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bouwe.Cryptography;
using TwoNil.Data;
using TwoNil.Logic.Exceptions;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
   public class UserService : ServiceBase
   {
      private const string _emailValidationPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
      private Regex _emailAddressRegex = new Regex(_emailValidationPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
      private const string _passwordHashPrefix = "$TwoNilHash$V1$";

      public User GetUser(string username, string password)
      {
         // Validation: both username and password are required.
         if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
         {
            throw new ValidationException("Invalid authentication properties provided");
         }

         var user = GetByUsername(username);

         if (user != null)
         {
            // Verify the password hashes.
            var passwordHasher = new PasswordHasher(_passwordHashPrefix);
            bool verified = passwordHasher.Verify(password, user.PasswordHash);
            if (!verified)
            {
               user = null;
            }
         }

         return user;
      }

      public User GetUser(string userId)
      {
         using (var userRepository = new MasterRepositoryFactory().CreateUserRepository())
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

         string passwordHash = new PasswordHasher(_passwordHashPrefix).Hash(password);

         var user = new User()
         {
            Firstname = firstname,
            Lastname = lastname,
            Username = username,
            Email = email,
            PasswordHash = passwordHash
         };

         using (var repository = new MasterRepositoryFactory().CreateTransactionManager())
         {
            repository.RegisterInsert(user);
            repository.Save();
         }

         return user;
      }

      private User GetByUsername(string username)
      {
         using (var userRepository = new MasterRepositoryFactory().CreateUserRepository())
         {
            return userRepository.Find(x => x.Username.Equals(username, StringComparison.OrdinalIgnoreCase)).SingleOrDefault();
         }
      }
   }
}
