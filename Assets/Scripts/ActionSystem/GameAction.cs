using UnityEngine;
using System.Collections.Generic;

namespace RobbieWagnerGames.RPG
{
    public abstract class GameAction 
    {
        public List<GameAction> PreActions {get; private set;} = new();
        public List<GameAction> PerformActions {get; private set;} = new();
        public List<GameAction> PostActions {get; private set;} = new();
    }
}