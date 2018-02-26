using System.Collections.Generic;
using Randomization;

namespace TwoNil.Logic.Functionality
{
   internal class TeamNameGenerator
   {
      private static ListRandomizer _listRandomizer = new ListRandomizer();

      public string GetTeamName()
      {
         //Bron: http://www.namegenerator.biz/place-name-generator.php

         var cities = new List<string>
         {
            "Falcondel",
            "Lochway",
            "Mallowice",
            "Whitemont",
            "Summerbutter",
            "Clearham",
            "Castleham",
            "Butterrock",
            "Westspring",
            "Westernesse",
            "Tenby",
            "Highview",
            "Dracmere",
            "Hollowston",
            "Wildesage",
            "Shadowbrook",
            "Buttercastle",
            "Silveroak",
            "Crystalcourt",
            "Springwinter",
            "Redwind",
            "Highfort",
            "Wyvernmere",
            "Whitemere",
            "Shadowgate",
            "Iceham",
            "Deeplake",
            "Freywyn",
            "Deeracre",
            "Shadowhal",
            "Deermead",
            "Starrydel",
            "Eastgriffin",
            "Marblewolf",
            "Woodmaple",
            "Edgeburn",
            "Highholt",
            "Coldfog",
            "Redash",
            "Hedgecliff",
            "Valhollow",
            "Byglass",
            "Westfay",
            "Greyhurst",
            "Fairfalcon",
            "Delland",
            "Glassmist",
            "Springbarrow",
            "Bykeep",
            "Grasslake",
            "Janlea",
            "Belwald",
            "Bellmare",
            "Brookden",
            "Hollowdel",
            "Iceacre",
            "Shadowlea",
            "Rockland",
            "Marbleborough",
            "Woodbeach",
            "Dorfield",
            "Lorbridge",
            "Bridgevale",
            "Alverton",
            "Larton",
            "Tunstead",
            "Wolfort",
            "Swanford",
            "Gormsey",
            "Deepbeech",
            "Woodfort",
            "Fayfield",
            "Edgeness",
            "Esterbeech",
            "Rosetown",
            "Hollowlyn",
            "Morfort",
            "Vertbrook",
            "Clearice",
            "Silveredge",
            "Mallowfield",
            "Fairmere",
            "Marblefield",
            "Norden",
            "Starryton",
            "Dracbarrow",
            "Wellden",
            "Crystalbeach",
            "Shadowmeadow",
            "Silverash",
            "Eastmarsh",
            "Pinevale",
            "Ericliff",
            "Janpond",
            "Raymont",
            "Marshfield",
            "Buttermeadow",
            "Bluedragon",
            "Deepburn",
            "Dracborough",
            "Estercrest",
            "Byway",
            "Mallowhedge",
            "Shorehal",
            "Fielddale",
            "Northfield",
         };

         var suffixes = new Dictionary<string, float>
         {
            { "Rangers", 1 },
            { "City", 2 },
            { "Rovers", 1 },
            { "FC", 3 },
            { "Town", 1 },
            { "United", 2 },
            { "Athletic", 1 },
            { "", 3 },
         };

         // Get random city.
         string teamName = _listRandomizer.GetItem(cities);

         // Combine city with (optional) suffix.
         string suffix = suffixes.RandomElementByWeight(x => x.Value).Key;
         if (!string.IsNullOrEmpty(suffix))
         {
            teamName = $"{teamName} {suffix}";
         }

         return teamName;
      }
   }
}
