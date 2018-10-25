using System.Collections.Generic;
using System.Linq;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
    public interface ICompetitionRepository : IGenericRepository<Competition>
    {
        IEnumerable<Competition> GetByCompetitionType(CompetitionType competitionType);
        Competition GetFriendly();
        League GetLeague1();
        League GetLeague2();
        League GetLeague3();
        League GetLeague4();
        IEnumerable<League> GetLeagues();
        Competition GetNationalCup();
        Competition GetNationalSuperCup();
    }

    public class CompetitionRepository : MemoryRepository<Competition>, ICompetitionRepository
    {
        internal CompetitionRepository()
        {
            Entities = new List<Competition>
             {
                GetLeague1(),
                GetLeague2(),
                GetLeague3(),
                GetLeague4(),
                GetNationalCup(),
                GetFriendly()
             };
        }

        public League GetLeague1()
        {
            return InMemoryData.GetLeague1();
        }

        public League GetLeague2()
        {
            return InMemoryData.GetLeague2();
        }

        public League GetLeague3()
        {
            return InMemoryData.GetLeague3();
        }

        public League GetLeague4()
        {
            return InMemoryData.GetLeague4();
        }

        public Competition GetNationalCup()
        {
            return InMemoryData.GetNationalCup();
        }

        public Competition GetFriendly()
        {
            return InMemoryData.GetFriendly();
        }

        public Competition GetNationalSuperCup()
        {
            return InMemoryData.GetNationalSuperCup();
        }

        public IEnumerable<Competition> GetByCompetitionType(CompetitionType competitionType)
        {
            return Entities.Where(x => x.CompetitionType == competitionType).OrderBy(x => x.Order);
        }

        public IEnumerable<League> GetLeagues()
        {
            return GetByCompetitionType(CompetitionType.League).Cast<League>();
        }
    }
}
