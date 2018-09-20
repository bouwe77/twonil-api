using System.Collections.Generic;
using TwoNil.Data;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Services
{
    public class CompetitionService : ServiceBase
    {
        public IEnumerable<Competition> GetAll()
        {
            using (var competitionRepository = new RepositoryFactory().CreateCompetitionRepository())
            {
                var competitions = competitionRepository.GetAll();
                return competitions;
            }
        }

        public Competition Get(string competitionId)
        {
            using (var competitionRepository = new RepositoryFactory().CreateCompetitionRepository())
            {
                var competition = competitionRepository.GetOne(competitionId);
                return competition;
            }
        }

        public IEnumerable<Competition> GetByType(CompetitionType competitionType)
        {
            using (var competitionRepository = new RepositoryFactory().CreateCompetitionRepository())
            {
                var competitions = competitionRepository.GetByCompetitionType(competitionType);
                return competitions;
            }
        }
    }
}
