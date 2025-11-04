using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class DebuffUnitCA : GameAction
    {
        public Debuff debuffEffect;

        public DebuffUnitCA(Debuff effect)
        {
            debuffEffect = effect;
        }
    }
}