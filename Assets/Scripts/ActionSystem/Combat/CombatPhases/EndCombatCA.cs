using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class EndCombatCA : GameAction
    {
        public override ActionScope Scope => ActionScope.COMBAT_PHASE;
        public EndCombatCA(bool win)
        {
            
        }
    }
}