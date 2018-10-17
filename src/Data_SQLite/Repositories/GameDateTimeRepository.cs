using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Data.Repositories
{
    public class GameDateTimeRepository : ReadRepository<GameDateTime>
    {
        internal GameDateTimeRepository(string databaseFilePath, string gameId)
           : base(databaseFilePath, gameId)
        {
        }

        public IEnumerable<GameDateTime> GetByStatus(GameDateTimeStatus status)
        {
            return Find(g => g.Status == status).OrderBy(g => g.DateTime);
        }
    }
}
