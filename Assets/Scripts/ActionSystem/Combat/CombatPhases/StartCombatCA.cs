using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class StartCombatCA : GameAction
    {
        public override ActionScope Scope => ActionScope.COMBAT_PHASE; 
    }
}