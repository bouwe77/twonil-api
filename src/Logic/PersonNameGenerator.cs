using System.Collections.Generic;
using Randomization;
using TwoNil.Data;

namespace TwoNil.Logic
{
    public class PersonNameGenerator
    {
        private readonly INumberRandomizer _numberRandomizer;
        private readonly IUnitOfWorkFactory _uowFactory;

        public PersonNameGenerator(IUnitOfWorkFactory uowFactory, INumberRandomizer numberRandomizer)
        {
            _uowFactory = uowFactory;
            _numberRandomizer = numberRandomizer;
        }

        public string GetLastName()
        {
            using (var uow = _uowFactory.Create())
            {
                int maxValue = uow.Names.GetNumberOfNames() - 1;
                int randomNumber = _numberRandomizer.GetNumber(0, maxValue);
                return uow.Names.GetLastName(randomNumber);
            }
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
