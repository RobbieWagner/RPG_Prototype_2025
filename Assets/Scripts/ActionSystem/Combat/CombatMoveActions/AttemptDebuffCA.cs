using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class AttemptDebuffCA : GameAction
    {
        public Debuff debuffEffect;
        public Unit user;
        public List<Unit> targets;
        public override ActionScope Scope => ActionScope.EXECUTION_PHASE;

        public AttemptDebuffCA(Debuff effect = null, Unit user = null, List<Unit> targets = null)
        {
            debuffEffect = effect;
            this.user = user;
            this.targets = targets;
        }
    }
}