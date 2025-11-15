using UnityEngine;
using System.Collections.Generic;

namespace RobbieWagnerGames.RPG
{
    public enum ActionScope
    {
        UNDEFINED,
        // Top-level actions that control the main flow
        COMBAT_PHASE,
        // Actions related to selecting options/targets
        INPUT_PHASE,
        // Actions that perform the core logic (Attack, Heal, etc.)
        EXECUTION_PHASE,
        // Actions nested within execution (DealDamage, BuffUnit)
        SUB_EXECUTION_PHASE
    }

    public abstract class GameAction 
    {
        public List<GameAction> PreActions {get; private set;} = new();
        public List<GameAction> PerformActions {get; private set;} = new();
        public List<GameAction> PostActions { get; private set; } = new();

        public abstract ActionScope Scope {get;}
    }
}