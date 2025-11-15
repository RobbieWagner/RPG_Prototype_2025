using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class RunActionSelectionPhaseCA : GameAction
    {
        public bool useCombatManagerUnit = false;
        private Unit unit = null;
        public Unit Unit => useCombatManagerUnit ? CombatManager.Instance.CurrentActingUnit : unit;
        public List<Unit> targetOptions = new List<Unit>();
        public override ActionScope Scope => ActionScope.COMBAT_PHASE;

        public RunActionSelectionPhaseCA(bool usingCombatManagerUnit, Unit currentActingUnit = null, List<Unit> targetOptions = null)
        {
            unit = currentActingUnit;
            useCombatManagerUnit = usingCombatManagerUnit;
            this.targetOptions = targetOptions;
        }
    }
}