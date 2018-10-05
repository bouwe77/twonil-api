//using System;
//using System.Collections.Generic;
//using System.Text;
//using TwoNil.Data;
//using TwoNil.Logic.Calendar;

//namespace TwoNil.Logic.Matches.PostMatches
//{
//    public class GameDateTimeHandler : IPostMatchesHandler
//    {
//        private GameDateTimeMutationManager _gameDateTimeMutationManager;

//        public GameDateTimeHandler(ITransactionManager transactionManager, IRepositoryFactory repositoryFactory)
//        {
//            _gameDateTimeMutationManager = new GameDateTimeMutationManager(transactionManager, repositoryFactory);
//        }

//        public void Handle(PostMatchData postMatchData)
//        {
//            UpdateManagerPlaysMatch();
//            UpdateMatchStatus();
//        }

//        private void UpdateManagerPlaysMatch()
//        {
//            // regelen dat je uit de PostMatchData kunt halen dat er voor de manager nieuwe wedstrijd(en) bij zijn gekomen
//            // en dat je dat bij de GameDateTimes moet aangeven.
//            // op dit moment zijn dat alleen friendlies en cup wedstrijden.
//            // Ik denk dat beide wel bij elkaar in deze class kunnen.


//            //if (matchesNextRound.Any(m => m.TeamPlaysMatch(managersTeam)))
//            //    _gameDateTimeManager.UpdateManagerPlaysMatch(matchesNextRound.Select(m => m.Date).First());
//        }

//        private void UpdateMatchStatus()
//        {
//            // Update match status in the calendar.
//            var matchDateTime = rounds.Select(x => x.MatchDate).First();
//            _gameDateTimeMutationManager.UpdateMatchStatus(matchDateTime);
//        }
//    }
//}
