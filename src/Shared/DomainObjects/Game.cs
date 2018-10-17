namespace TwoNil.Shared.DomainObjects
{
   public class Game : DomainObjectBase
   {
      public string UserId { get; set; }

      public override string ToString()
      {
         return $"[ Id = '{Id}', UserId = '{UserId}' ]";
      }
   }
}
