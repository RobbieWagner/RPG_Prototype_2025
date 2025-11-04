using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class HealUnitCA : GameAction
    {
        public Heal healingEffect;

        public HealUnitCA(Heal healEffect)
        {
            healingEffect = healEffect;
        }
    }
}