using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public static class TargetSelectionUtility
    {
        public static List<Unit> GetValidTargetsForMove(Unit selectingUnit, CombatMove move, List<Unit> allPossibleTargets)
        {
            if (move == null || allPossibleTargets == null || allPossibleTargets.Count == 0)
                return new List<Unit>();

            if (move.targetsAllUnits)
            {
                return move.canTargetSelf 
                    ? new List<Unit>(allPossibleTargets) 
                    : allPossibleTargets.Where(x => !x.Equals(selectingUnit)).ToList();
            }

            if (move.targetsAllAllies)
            {
                return selectingUnit.isPlayerUnit 
                    ? allPossibleTargets.Where(x => x.isPlayerUnit).ToList()
                    : allPossibleTargets.Where(x => !x.isPlayerUnit).ToList();
            }

            if (move.targetsAllOpposition)
            {
                return selectingUnit.isPlayerUnit 
                    ? allPossibleTargets.Where(x => !x.isPlayerUnit).ToList()
                    : allPossibleTargets.Where(x => x.isPlayerUnit).ToList();
            }

            List<Unit> validTargets = new List<Unit>();

            if (move.canTargetAllies)
            {
                validTargets.AddRange(
                    selectingUnit.isPlayerUnit 
                    ? allPossibleTargets.Where(x => x.isPlayerUnit && x != selectingUnit).ToList()
                    : allPossibleTargets.Where(x => !x.isPlayerUnit && x != selectingUnit).ToList()
                );
            }

            if (move.canTargetOpposition)
            {
                validTargets.AddRange(
                    selectingUnit.isPlayerUnit 
                    ? allPossibleTargets.Where(x => !x.isPlayerUnit).ToList()
                    : allPossibleTargets.Where(x => x.isPlayerUnit).ToList()
                );
            }

            if (move.canTargetSelf)
            {
                validTargets.Add(selectingUnit);
            }

            return validTargets.Distinct().ToList();
        }

        public static List<Unit> GetValidTargetsForMove(Unit selectingUnit, CombatMove move)
        {
            return GetValidTargetsForMove(selectingUnit, move, CombatManager.Instance.allCurrentUnits);
        }

        // Helper method for AI to randomly select targets
        public static List<Unit> GetRandomValidTargets(Unit selectingUnit, CombatMove move, List<Unit> allPossibleTargets, int maxTargets = 1)
        {
            List<Unit> validTargets = GetValidTargetsForMove(selectingUnit, move, allPossibleTargets);
            
            if (validTargets.Count == 0)
                return new List<Unit>();

            // For moves that target multiple units automatically, return all valid targets
            if (move.targetsAllUnits || move.targetsAllAllies || move.targetsAllOpposition)
            {
                return validTargets;
            }

            // For single-target moves, pick random targets up to maxTargets
            List<Unit> selectedTargets = new List<Unit>();
            List<Unit> availableTargets = new List<Unit>(validTargets);

            for (int i = 0; i < Mathf.Min(maxTargets, availableTargets.Count); i++)
            {
                int randomIndex = Random.Range(0, availableTargets.Count);
                selectedTargets.Add(availableTargets[randomIndex]);
                availableTargets.RemoveAt(randomIndex);
            }

            return selectedTargets;
        }
    }
}