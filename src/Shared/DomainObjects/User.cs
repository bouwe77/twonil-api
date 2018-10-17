namespace TwoNil.Shared.DomainObjects
{
   public class User : DomainObjectBase
   {
      public string Username { get; set; }

      public string PasswordHash { get; set; }

      public string Firstname { get; set; }

      public string Lastname { get; set; }

      public string Email { get; set; }
   }
}
