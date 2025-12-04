using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class EndCombatCA : GameAction
    {
        public override ActionScope Scope => ActionScope.COMBAT_PHASE;

        public string combatSceneName;
        public bool win;

        public EndCombatCA(bool win, string combatSceneName)
        {
            this.win = win;
            this.combatSceneName = combatSceneName;
        }
    }
}