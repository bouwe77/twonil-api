namespace TwoNil.Shared.DomainObjects
{
   public class Line : DomainObjectBase
   {
      public string Name { get; set; }
      public bool IsField { get; set; }

      public Line(string name, bool isField)
      {
         Name = name;
         IsField = isField;
      }
   }
}
