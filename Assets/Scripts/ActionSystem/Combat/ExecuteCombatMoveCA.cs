using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class ExecuteCombatMoveCA : GameAction
    {
        public CombatMove combatMove;
        public Unit user;
        public List<Unit> targets;
        public override ActionScope Scope => ActionScope.COMBAT_PHASE;

        public ExecuteCombatMoveCA(CombatMove combatMove = null, Unit user = null, List<Unit> targets = null)
        {
            this.combatMove = combatMove;
            this.user = user;
            this.targets = targets;
        }
    }
}