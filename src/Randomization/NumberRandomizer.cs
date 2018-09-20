using System;

namespace Randomization
{
    public interface INumberRandomizer
    {
        int GetNumber(int minimumValue, int maximumValue);
        int GetEvenNumber(int minimumValue, int maximumValue);
    }

    public class NumberRandomizer : INumberRandomizer
    {
        private static Randomizer _randomizer = new Randomizer();

        public int GetNumber(int minimumValue, int maximumValue)
        {
            return _randomizer.GetRandomNumber(minimumValue, maximumValue);
        }

        public int GetEvenNumber(int minimumValue, int maximumValue)
        {
            int randomEvenNumber = -1;

            const int MaxAttempts = 1000;
            int attempts = 0;
            while (attempts < MaxAttempts)
            {
                int randomNumber = _randomizer.GetRandomNumber(minimumValue, maximumValue);
                if (randomNumber % 2 == 0)
                {
                    randomEvenNumber = randomNumber;
                    break;
                }

                attempts++;
            }

            if (randomEvenNumber == -1)
            {
                throw new Exception("No even random number found");
            }

            return randomEvenNumber;
        }
    }
}
