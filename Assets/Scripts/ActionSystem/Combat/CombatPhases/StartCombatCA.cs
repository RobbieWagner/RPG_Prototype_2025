using System;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    [Serializable]
    public class StartCombatCA : GameAction
    {
        public override ActionScope Scope => ActionScope.COMBAT_PHASE; 
    }
}