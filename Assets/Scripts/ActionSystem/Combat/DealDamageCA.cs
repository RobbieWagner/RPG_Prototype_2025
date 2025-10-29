using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class DealDamageCA : GameAction
    {
        public int Amount;
        public DealDamageCA(int amount)
        {
            Amount = amount;
        }
    }
}