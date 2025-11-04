using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class MakeAttackCA : GameAction
    {
        public Attack attackEffect;
        public MakeAttackCA(Attack attackInfo)
        {
            attackEffect = attackInfo;
        }
    }
}