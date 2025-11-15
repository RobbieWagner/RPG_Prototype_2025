using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class RunActionExecutionPhaseCA : GameAction
    {
        public bool useCombatManagerUnit = false;
        private Unit unit = null;
        public Unit Unit => useCombatManagerUnit ? CombatManager.Instance.CurrentActingUnit : unit;
        public override ActionScope Scope => ActionScope.COMBAT_PHASE;

        public RunActionExecutionPhaseCA(bool usingCombatManagerUnit, Unit currentActingUnit = null)
        {
            unit = currentActingUnit;
            useCombatManagerUnit = usingCombatManagerUnit;
        }
    }
}