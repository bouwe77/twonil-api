using System.Collections.Generic;
using Randomization;
using TwoNil.Data;
using TwoNil.Data.Repositories;

namespace TwoNil.Logic
{
   public class PersonNameGenerator
   {
      private readonly NameRepository _nameRepository;
      private readonly INumberRandomizer _numberRandomizer;

      public PersonNameGenerator()
      {
         _nameRepository = new RepositoryFactory().CreateNameRepository();
         _numberRandomizer = new NumberRandomizer();
      }

      public string GetLastName()
      {
         int maxValue = _nameRepository.GetNumberOfNames() - 1;
         int randomNumber = _numberRandomizer.GetNumber(0, maxValue);
         return _nameRepository.GetLastName(randomNumber);
      }

      public string GetFirstName()
      {
         var firstnames = new List<string> { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

         int randomNumber = _numberRandomizer.GetNumber(0, 25);
         string firstname = firstnames[randomNumber];
         return firstname;
      }
   }
}
