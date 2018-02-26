namespace TwoNil.API.Resources
{
   public class RatingFactory
   {
      public static Rating Create(decimal ratingScore)
      {
         var rating = GetRatingForAverage(ratingScore);
         int score = (int)rating + 1;

         var ratingResource = new Rating
         {
            Name = rating.ToString(),
            Score = score
         };

         return ratingResource;
      }

      private static Shared.DomainObjects.Rating GetRatingForAverage(decimal average)
      {
         var rating = Shared.DomainObjects.Rating.Useless;

         if (average > 17) rating = Shared.DomainObjects.Rating.Ace;
         else if (average > 15) rating = Shared.DomainObjects.Rating.Supremo;
         else if (average > 13) rating = Shared.DomainObjects.Rating.Superb;
         else if (average > 11) rating = Shared.DomainObjects.Rating.Good;
         else if (average > 9) rating = Shared.DomainObjects.Rating.Average;
         else if (average > 7) rating = Shared.DomainObjects.Rating.Fair;
         else if (average > 5) rating = Shared.DomainObjects.Rating.Poor;
         else if (average > 3) rating = Shared.DomainObjects.Rating.Low;
         else if (average > 1) rating = Shared.DomainObjects.Rating.Bad;

         return rating;
      }
   }
}