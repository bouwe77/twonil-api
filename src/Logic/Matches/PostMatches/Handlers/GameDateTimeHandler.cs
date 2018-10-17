using System.Linq;
using TwoNil.Logic.Calendar;

namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class GameDateTimeHandler : IPostMatchesHandler
    {
        private IGameDateTimeMutationManager _gameDateTimeMutationManager;

        public GameDateTimeHandler(IGameDateTimeMutationManager gameDateTimeMutationManager)
        {
            _gameDateTimeMutationManager = gameDateTimeMutationManager;
        }

        public void Handle(PostMatchData postMatchData)
        {
            if (postMatchData.NewManagerMatchDates.Any())
                UpdateManagerPlaysMatch(postMatchData);

            UpdateMatchStatus(postMatchData);
        }

        private void UpdateManagerPlaysMatch(PostMatchData postMatchData)
        {
            foreach (var matchDate in postMatchData.NewManagerMatchDates)
            {
                _gameDateTimeMutationManager.UpdateManagerPlaysMatch(matchDate);

            }
        }

        private void UpdateMatchStatus(PostMatchData postMatchData)
        {
            // Update match status in the calendar.
            _gameDateTimeMutationManager.UpdateMatchStatus(postMatchData.MatchDateTime);
        }
    }
}
