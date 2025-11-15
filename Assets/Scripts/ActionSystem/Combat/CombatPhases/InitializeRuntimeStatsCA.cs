using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class InitializeRuntimeStatsCA : GameAction
    {
        public List<Unit> units;
        public override ActionScope Scope => ActionScope.COMBAT_PHASE;

        public InitializeRuntimeStatsCA(List<Unit> units)
        {
            this.units = units;
        }
    }
}