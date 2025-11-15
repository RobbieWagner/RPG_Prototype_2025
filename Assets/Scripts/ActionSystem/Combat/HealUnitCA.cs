using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class HealUnitCA : GameAction
    {
        public int amount;
        public Unit unit;
        public override ActionScope Scope => ActionScope.SUB_EXECUTION_PHASE;
        public HealUnitCA(int amount, Unit unit)
        {
            this.amount = amount;
            this.unit = unit;
        }
    }
}