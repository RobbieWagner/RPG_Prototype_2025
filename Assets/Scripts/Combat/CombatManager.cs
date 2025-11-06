using System;
using System.Collections.Generic;
using RobbieWagnerGames.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public class CombatManager : MonoBehaviourSingleton<CombatManager>
    {
        public CombatState CurrentState { get; protected set; } = CombatState.NONE;

        [SerializeField] private CombatDetails testCombatDetails = null;
        [HideInInspector] public CombatDetails currentCombatDetails = null;
        private Dictionary<UnitData, UnitInstance> currentPlayerUnits;
        private Dictionary<UnitData, UnitInstance> currentEnemyUnits;
        [SerializeField] private UnitInstance unitInstancePrefab = null;

        public int currentTurn = 0;

        protected override void Awake()
        {
            base.Awake();

           StartCombat(testCombatDetails);
        }

        public virtual void StartCombat(CombatDetails combatDetails)
        {
            currentCombatDetails = combatDetails;

            ChangeCombatState(CombatState.SETUP);
        }

        protected virtual void ChangeCombatState(CombatState newState)
        {
            CurrentState = newState;

            switch (CurrentState)
            {
                case CombatState.SETUP:
                    CombatActionSystem.Instance.Perform(new SetupCombatCA(), () =>
                    {
                        ChangeCombatState(CombatState.PLAYER_TURN);
                    });
                    break;
                case CombatState.PLAYER_TURN:
                    CombatActionSystem.Instance.Perform(new StartTurnCA(), () =>
                    {
                        CombatActionSystem.Instance.Perform(new RunPlayerPhaseCA(), () =>
                        {
                            ChangeCombatState(CombatState.ENEMY_TURN);
                        });
                    });
                    break;
                case CombatState.ENEMY_TURN:
                    CombatActionSystem.Instance.Perform(new RunEnemyPhaseCA(), () =>
                    {
                        CombatActionSystem.Instance.Perform(new EndTurnCA(), () =>
                        {
                            currentTurn++;
                            ChangeCombatState(CombatState.PLAYER_TURN);
                        });
                    });
                    break;
                case CombatState.WIN:
                    CombatActionSystem.Instance.Perform(new EndCombatCA(true), () =>
                    {
                        // Load exploration scene or do other post-combat actions
                    });
                    break;
                case CombatState.LOSE:
                    CombatActionSystem.Instance.Perform(new EndCombatCA(false), () =>
                    {
                        // Load game over scene or do other post-combat actions
                    });
                    break;
                default:
                    break;
            }
        }

        public void SpawnCombatUnitsOnField()
        {
            currentEnemyUnits = new Dictionary<UnitData, UnitInstance>();
            foreach (UnitData enemyData in currentCombatDetails.enemies)
            {
                UnitInstance enemyInstance = Instantiate(unitInstancePrefab);
                enemyInstance.UnitData = enemyData;
                currentEnemyUnits.Add(enemyData, enemyInstance);
            }
            currentPlayerUnits = new Dictionary<UnitData, UnitInstance>();
            foreach (UnitData allyData in currentCombatDetails.allies)
            {
                UnitInstance allyInstance = Instantiate(unitInstancePrefab);
                allyInstance.UnitData = allyData;
                currentPlayerUnits.Add(allyData, allyInstance);
            }
        }
    }
}