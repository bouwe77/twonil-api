﻿using System.Collections.Generic;

namespace TwoNil.Data.Repositories
{
    public interface ITeamNameRepository
    {
        string[] GetAll();
    }

    public class TeamNameRepository : ITeamNameRepository
    {
        public string[] GetAll()
        {
            return new List<string>
             {
                "Shadowhal United",
                "Falcondel",
                "Castleham Rangers",
                "Morfort City",
                "Marbleborough",
                "Dorfield Rovers",
                "Hedgecliff",
                "Nineby FC",
                "Bradchester United",
                "Carice",
                "Tutton Athletic",
                "Cocklebury FC",
                "Milethorpe United",
                "Balington",
                "Cruley United",
                "Morran Town",
             }.ToArray();
        }
    }
}
