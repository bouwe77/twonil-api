using System;
using System.Collections.Generic;
using System.Linq;
using Randomization;
using TwoNil.Data;
using TwoNil.Data.Memory;
using TwoNil.Shared.DomainObjects;

namespace TwoNil.Logic.Functionality.Players
{
   internal class PositionDeterminator : IDisposable
   {
      private readonly IPositionRepository _positionRepository;
      private readonly IListRandomizer _listRandomizer;

      public PositionDeterminator()
      {
         _listRandomizer = new ListRandomizer();
         _positionRepository = new MemoryRepositoryFactory().CreatePositionRepository();
      }

      internal PositionDeterminator(IPositionRepository positionRepository, IListRandomizer listRandomizer)
      {
         _positionRepository = positionRepository;
         _listRandomizer = listRandomizer;
      }

      public Position Determine(List<PlayerSkillScore> skillScores)
      {
         List<Position> positions;
         positions = _positionRepository.GetAll().ToList();

         var possiblePositions = new List<Position>();

         // Sort skill scores.
         skillScores = skillScores.OrderByDescending(x => x.Score).ThenBy(x => x.PlayerSkill.Name).ToList();

         // Create a list of positions per skill.
         foreach (var skillScore in skillScores)
         {
            // Create an empty anonymous list.
            var positionsWithPrimarySkill = Enumerable.Empty<object>()
                                                .Select(r => new { Position = new Position(), Index = 0 }) // prototype of anonymous type
                                                .ToList();

            // Add all positions to the anonymous list that have the skill as primary skill 
            // and also the index of the primary skill.
            foreach (var position in positions)
            {
               int index = position.PrimarySkills.IndexOf(skillScore.PlayerSkill);
               if (index != -1)
               {
                  positionsWithPrimarySkill.Add(new { Position = position, Index = index });
               }
            }

            if (positionsWithPrimarySkill.Any())
            {
               // Only get the positions with the lowest index value.
               int minIndex = positionsWithPrimarySkill.Min(x => x.Index);
               possiblePositions = positionsWithPrimarySkill.Where(x => x.Index == minIndex).Select(x => x.Position).ToList();

               // Quit searching if one remaining position is left.
               if (possiblePositions.Count == 1)
               {
                  break;
               }

               // For the next skill only the remaining positions will be queried.
               positions = possiblePositions;
            }
         }

         if (!possiblePositions.Any())
         {
            throw new ApplicationException("No position was determined, this shouldn't be possible?");
         }

         Position determinedPosition = possiblePositions.Count == 1
            ? possiblePositions.First()
            : _listRandomizer.GetItem(possiblePositions);

         // Dirty Hack: Do not use the sweeper position. If a player has been identified as a sweeper he will become a centre back.
         bool playerIsSweeper = determinedPosition.Equals(_positionRepository.GetSweeper());
         if (playerIsSweeper)
         {
            determinedPosition = _positionRepository.GetCentreBack();
         }

         return determinedPosition;
      }

      public void Dispose()
      {
         _positionRepository?.Dispose();
      }
   }
}
