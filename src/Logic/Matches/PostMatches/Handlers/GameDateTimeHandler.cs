using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoNil.Data;
using TwoNil.Logic.Calendar;

namespace TwoNil.Logic.Matches.PostMatches.Handlers
{
    public class GameDateTimeHandler : IPostMatchesHandler
    {
        private GameDateTimeMutationManager _gameDateTimeMutationManager;

        public GameDateTimeHandler(ITransactionManager transactionManager, IRepositoryFactory repositoryFactory)
        {
            _gameDateTimeMutationManager = new GameDateTimeMutationManager(transactionManager, repositoryFactory);
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
