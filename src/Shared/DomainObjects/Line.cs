namespace TwoNil.Shared.DomainObjects
{
   public class Line : DomainObjectBase
   {
      public string Name { get; private set; }
      public bool IsField { get; private set; }

      public Line(string name, bool isField)
      {
         Name = name;
         IsField = isField;
      }
   }
}
