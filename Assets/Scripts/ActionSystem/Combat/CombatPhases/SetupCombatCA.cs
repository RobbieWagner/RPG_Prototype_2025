using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class SetupCombatCA : GameAction
    {
        public override ActionScope Scope => ActionScope.COMBAT_PHASE;
    }
}