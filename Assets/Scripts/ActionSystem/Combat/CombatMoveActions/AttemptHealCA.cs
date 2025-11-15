using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class AttemptHealCA : GameAction
    {
        public Heal healEffect;

        public Unit user;
        public List<Unit> targets;
        public override ActionScope Scope => ActionScope.EXECUTION_PHASE;

        public AttemptHealCA(Heal healingEffect = null, Unit user = null, List<Unit> targets = null)
        {
            this.healEffect = healingEffect;
            this.user = user;
            this.targets = targets;
        }
    }
}