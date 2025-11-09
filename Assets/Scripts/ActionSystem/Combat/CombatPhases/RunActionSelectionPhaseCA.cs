using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class RunActionSelectionPhaseCA : GameAction
    {
        public bool useCombatManagerUnit = false;
        private Unit unit = null;
        public Unit Unit => useCombatManagerUnit ? CombatManager.Instance.CurrentActingUnit : unit;

        public RunActionSelectionPhaseCA(bool usingCombatManagerUnit, Unit currentActingUnit = null)
        {
            unit = currentActingUnit;
            useCombatManagerUnit = usingCombatManagerUnit;
        }
    }
}