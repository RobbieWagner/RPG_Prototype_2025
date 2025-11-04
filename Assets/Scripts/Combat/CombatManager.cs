using System;
using System.Collections.Generic;
using RobbieWagnerGames.Utilities;

namespace RobbieWagnerGames.RPG
{
    public enum CombatState
    {
        NONE = -1,
        SETUP,
        PLAYER_TURN,
        ENEMY_TURN,
        WIN,
        LOSE
    }

    public abstract class CombatManager : MonoBehaviourSingleton<CombatManager>
    {
        public CombatState CurrentState { get; protected set; } = CombatState.NONE;

        public CombatDetails currentCombatDetails = null;
        private List<Unit> currentPlayerUnits;
        private List<Unit> currentEnemyUnits;

        public int currentTurn = 0;

        protected override void Awake()
        {
            base.Awake();
        }

        public virtual void StartCombat(CombatDetails combatDetails)
        {
            currentCombatDetails = combatDetails;
            // currentPlayerUnits = playerUnits; <-- Get from GameManager when ready
            currentEnemyUnits = new List<Unit>(combatDetails.enemies);
            CurrentState = CombatState.SETUP;

            // figure out integration of action system and manager
        }

        protected virtual void ChangeCombatState(CombatState newState)
        {
            CurrentState = newState;
        }
    }
}