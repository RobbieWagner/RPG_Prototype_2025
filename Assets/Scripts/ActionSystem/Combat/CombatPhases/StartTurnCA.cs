using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class StartTurnCA : GameAction
    {
        public override ActionScope Scope => ActionScope.COMBAT_PHASE;
    }
}