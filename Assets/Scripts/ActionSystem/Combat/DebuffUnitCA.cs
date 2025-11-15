using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class DebuffUnitCA : GameAction
    {
        public int amount;
        public ComputedStatType stat;
        public Unit unit;
        public override ActionScope Scope => ActionScope.SUB_EXECUTION_PHASE;
        public DebuffUnitCA(int amount, ComputedStatType stat, Unit unit)
        {
            this.amount = amount;
            this.stat = stat;
            this.unit = unit;
        }
    }
}