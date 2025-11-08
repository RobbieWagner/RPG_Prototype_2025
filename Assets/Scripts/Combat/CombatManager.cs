using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RobbieWagnerGames.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RobbieWagnerGames.RPG
{
    public enum CombatState
    {
        NONE = -1,
        SETUP,
        ACTION_SELECTION,
        ACTION_EXECUTION,
        WIN,
        LOSE
    }

    public class CombatManager : MonoBehaviourSingleton<CombatManager>
    {
        public CombatState CurrentState { get; protected set; } = CombatState.NONE;

        [SerializeField] private CombatDetails testCombatDetails = null;
        [HideInInspector] public CombatDetails currentCombatDetails = null;
        
        
        [SerializeField] private Unit unitInstancePrefab = null;
        [HideInInspector] public Dictionary<UnitData, Unit> currentPlayerUnits;
        [HideInInspector] public Dictionary<UnitData, Unit> currentEnemyUnits;
        public List<Unit> allCurrentUnits
        {
            get
            {
                List<Unit> allUnits = new Dictionary<UnitData, Unit>(currentPlayerUnits).Values.ToList();
                foreach (var enemyUnit in currentEnemyUnits)
                    allUnits.Add(enemyUnit.Value);
                return allUnits;
            }
        }
        private List<Unit> initiativeOrder = new List<Unit>();
        private int currentInitiativeIndex = 0;
        private Unit currentActingUnit = null;
        

        public int currentTurn = 0;

        protected override void Awake()
        {
            base.Awake();

           StartCombat(testCombatDetails);
        }

        public virtual void StartCombat(CombatDetails combatDetails)
        {
            if(currentCombatDetails != null)
            {
                Debug.LogWarning("Combat is already in progress!");
                return;
            }
            StartCoroutine(StartCombatCo(combatDetails));
        }

        public virtual IEnumerator StartCombatCo(CombatDetails combatDetails)
        {
            currentCombatDetails = combatDetails;
            yield return null;
            yield return SceneLoadManager.Instance.LoadSceneAdditive(combatDetails.combatSceneName, () => OnCombatSceneLoaded());
            
            ChangeCombatState(CombatState.SETUP);
        }

        protected void OnCombatSceneLoaded()
        {
            SpawnCombatUnitsOnField();
            BattlefieldController.Instance.PlaceAllies(currentPlayerUnits);
            BattlefieldController.Instance.PlaceEnemies(currentEnemyUnits);
        }

        protected virtual void ChangeCombatState(CombatState newState)
        {
            CurrentState = newState;

            switch (CurrentState)
            {
                case CombatState.SETUP:
                    CombatActionSystem.Instance.Perform(new SetupCombatCA(), () =>
                    {
                        CombatActionSystem.Instance.Perform(new InitializeRuntimeStatsCA(allCurrentUnits), () =>
                        {
                            BuildInitiativeOrder();
                            ChangeCombatState(CombatState.ACTION_SELECTION);
                        });
                    });
                    break;
                case CombatState.ACTION_SELECTION:
                    if (currentInitiativeIndex == 0)
                        CombatActionSystem.Instance.Perform(new StartTurnCA(), () =>
                        {
                            RunActionSelectionPhase();
                        });
                    else
                        RunActionSelectionPhase();
                    break;
                case CombatState.ACTION_EXECUTION:
                    CombatActionSystem.Instance.Perform(new RunActionExecutionPhaseCA(), () =>
                    {
                        currentInitiativeIndex++;

                        if (currentInitiativeIndex < initiativeOrder.Count)
                        {
                            ChangeCombatState(CombatState.ACTION_SELECTION);
                        }
                        else
                        { 
                            CombatActionSystem.Instance.Perform(new EndTurnCA(), () =>
                            {

                            });
                        }
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
            currentEnemyUnits = new Dictionary<UnitData, Unit>();
            foreach (UnitData enemyData in currentCombatDetails.enemies)
            {
                Unit enemyInstance = Instantiate(unitInstancePrefab);
                enemyInstance.UnitData = enemyData;
                currentEnemyUnits.Add(enemyData, enemyInstance);
            }
            currentPlayerUnits = new Dictionary<UnitData, Unit>();
            foreach (UnitData allyData in currentCombatDetails.allies)
            {
                Unit allyInstance = Instantiate(unitInstancePrefab);
                allyInstance.UnitData = allyData;
                currentPlayerUnits.Add(allyData, allyInstance);
            }
        }

        private void RunActionSelectionPhase()
        {
            currentActingUnit = initiativeOrder[currentInitiativeIndex];
            CombatActionSystem.Instance.Perform(new RunActionSelectionPhaseCA(currentActingUnit), () =>
            {
                ChangeCombatState(CombatState.ACTION_EXECUTION);
            });
        }


        private void BuildInitiativeOrder()
        {
            initiativeOrder.Clear();

            // Combine all units and sort by initiative stat
            List<Unit> allUnits = new List<Unit>();
            allUnits.AddRange(currentPlayerUnits.Values);
            allUnits.AddRange(currentEnemyUnits.Values);

            // Sort by speed/initiative stat (highest first)
            initiativeOrder = allUnits.OrderByDescending(unit => unit.runtimeStats[ComputedStatType.INITIATIVE]).ToList();
            currentInitiativeIndex = 0;
        }
    }
}