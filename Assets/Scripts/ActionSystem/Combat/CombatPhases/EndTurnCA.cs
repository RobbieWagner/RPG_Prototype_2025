using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class EndTurnCA : GameAction
    {
        public override ActionScope Scope => ActionScope.COMBAT_PHASE;
    }
}