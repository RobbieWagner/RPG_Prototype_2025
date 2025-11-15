using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class AttemptBuffCA : GameAction
    {
        public Buff buffEffect;
        public Unit user;
        public List<Unit> targets;
        public override ActionScope Scope => ActionScope.EXECUTION_PHASE; 

        public AttemptBuffCA(Buff effect = null, Unit user = null, List<Unit> targets = null)
        {
            buffEffect = effect;
            this.user = user;
            this.targets = targets;
        }
    }
}