using System.Collections.Generic;

namespace RobbieWagnerGames.RPG
{
    public static class CombatMoveExtensions
    {
        public static List<Unit> GetValidTargets(this CombatMove move, Unit selectingUnit, List<Unit> allPossibleTargets)
        {
            return TargetSelectionUtility.GetValidTargetsForMove(selectingUnit, move, allPossibleTargets);
        }

        public static List<Unit> GetValidTargets(this CombatMove move, Unit selectingUnit)
        {
            return TargetSelectionUtility.GetValidTargetsForMove(selectingUnit, move);
        }

        public static List<Unit> GetRandomTargets(this CombatMove move, Unit selectingUnit, List<Unit> allPossibleTargets, int maxTargets = 1)
        {
            return TargetSelectionUtility.GetRandomValidTargets(selectingUnit, move, allPossibleTargets, maxTargets);
        }
    }
}