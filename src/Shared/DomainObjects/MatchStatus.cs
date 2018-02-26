namespace TwoNil.Shared.DomainObjects
{
   public enum MatchStatus
   {
      /// <summary>
      /// The teams that will play in the match are not yet defined. This is used for future cup matches that have not been drawn yet.
      /// </summary>
      //TeamsUndefined,

      /// <summary>
      /// The match has not yet been played.
      /// </summary>
      NotStarted,

      /// <summary>
      /// The match has been played.
      /// </summary>
      Ended
   }
}
