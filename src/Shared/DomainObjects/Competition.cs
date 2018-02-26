namespace TwoNil.Shared.DomainObjects
{
   public class Competition : DomainObjectBase
   {
      public string Name { get; set; }
      public CompetitionType CompetitionType { get; set; }
      public int Order { get; set; }
   }
}