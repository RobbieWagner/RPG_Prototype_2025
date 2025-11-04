using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class BuffUnitCA : GameAction
    {
        public Buff buffEffect;

        public BuffUnitCA(Buff effect)
        {
            buffEffect = effect;
        }
    }
}