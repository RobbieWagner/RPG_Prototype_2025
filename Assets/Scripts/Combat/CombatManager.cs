using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RobbieWagnerGames.Utilities;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public enum CombatState
    {
        NONE = -1,
        SETUP,
        TURN_START,
        TURN_END,
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
        public Unit CurrentActingUnit => currentActingUnit;

        public StartCombatCA startCombatAction = new StartCombatCA();
        public EndCombatCA winCombatAction = new EndCombatCA(true);
        public EndCombatCA loseCombatAction = new EndCombatCA(false);
        public StartTurnCA startTurnAction = new StartTurnCA();
        public EndTurnCA endTurnAction = new EndTurnCA();
        public RunActionSelectionPhaseCA selectionPhaseAction = new RunActionSelectionPhaseCA(true);
        ExecuteCombatMoveCA executionAction = new ExecuteCombatMoveCA();
        public int currentTurn = 0;

        protected override void Awake()
        {
            base.Awake();

            StartCombat(testCombatDetails);
        }

        public virtual void StartCombat(CombatDetails combatDetails)
        {
            if (currentCombatDetails != null)
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
            CurrentState = IsCombatOver() 
                ? AnyPlayerUnits() ? CombatState.WIN : CombatState.LOSE 
                : newState;

            switch (CurrentState)
            {
                case CombatState.SETUP:
                    StartCoroutine(CombatActionSystem.Instance.PerformCo(new SetupCombatCA(), () =>
                    {
                        InitializeRuntimeStatsCA statSetupCombatAction = new InitializeRuntimeStatsCA(allCurrentUnits);

                        CombatActionSystem.Instance.Perform(statSetupCombatAction, () =>
                        {
                            CombatHUD.Instance.InitializeHUD(currentPlayerUnits.Values.ToList(), currentEnemyUnits.Values.ToList());
                            ChangeCombatState(CombatState.TURN_START);
                        });
                    }));
                    break;
                case CombatState.TURN_START:
                    StartCoroutine(CombatActionSystem.Instance.PerformCo(startTurnAction, () =>
                    {
                        ChangeCombatState(CombatState.ACTION_SELECTION);
                    }));
                    break;
                case CombatState.ACTION_SELECTION:
                    currentActingUnit = initiativeOrder[currentInitiativeIndex];
                    CombatHUD.Instance.turnTrackerUIInstance.UpdateTurnTrackerUI(currentTurn, currentActingUnit.isPlayerUnit);
                    selectionPhaseAction.targetOptions = allCurrentUnits;
                    StartCoroutine(CombatActionSystem.Instance.PerformCo(selectionPhaseAction, () =>
                    {
                        ChangeCombatState(CombatState.ACTION_EXECUTION);
                    }));
                    break;
                case CombatState.ACTION_EXECUTION:
                    executionAction.combatMove = CurrentActingUnit.selectedCombatMove;
                    executionAction.user = CurrentActingUnit;
                    executionAction.targets = CurrentActingUnit.selectedTargets;
                    StartCoroutine(CombatActionSystem.Instance.PerformCo(executionAction, () =>
                    {
                        CombatMove move = currentActingUnit.selectedCombatMove;

                        if (move != null) 
                            currentActingUnit.SetRuntimeStatValue(ComputedStatType.STAMINA, currentActingUnit.RuntimeStats[ComputedStatType.STAMINA] - move.moveCost);
                        currentActingUnit.selectedCombatMove = null;

                        if (currentActingUnit.RuntimeStats[ComputedStatType.STAMINA] == 0
                            || currentActingUnit.GetAvailableCombatMoves().Count == 0)
                        {
                            currentInitiativeIndex++;

                            if (currentInitiativeIndex < initiativeOrder.Count)
                                ChangeCombatState(CombatState.ACTION_SELECTION);
                            else
                                ChangeCombatState(CombatState.TURN_END);
                        }
                        else
                            ChangeCombatState(CombatState.ACTION_SELECTION);
                    }));
                    break;
                case CombatState.TURN_END:
                    StartCoroutine(CombatActionSystem.Instance.PerformCo(endTurnAction, () =>
                    {
                        ChangeCombatState(CombatState.TURN_START);
                    }));
                    break;
                case CombatState.WIN:
                    StartCoroutine(CombatActionSystem.Instance.PerformCo(new EndCombatCA(true), () =>
                    {
                        // Load exploration scene or do other post-combat actions
                    }));
                    break;
                case CombatState.LOSE:
                    StartCoroutine(CombatActionSystem.Instance.PerformCo(new EndCombatCA(false), () =>
                    {
                        // Load game over scene or do other post-combat actions
                    }));
                    break;
                default:
                    break;
            }
        }

        private bool AnyPlayerUnits()
        {
            return currentPlayerUnits.Values
                .Where(x => !x.RuntimeStats.TryGetValue(ComputedStatType.HP, out int value) || value > 0)
                .Any();
        }

        private bool AnyEnemyUnits()
        {
            return currentEnemyUnits.Values
                .Where(x => !x.RuntimeStats.TryGetValue(ComputedStatType.HP, out int value) || value > 0)
                .Any();
        }

        private bool IsCombatOver()
        {
            return !AnyPlayerUnits() || !AnyEnemyUnits();
        }

        public void SpawnCombatUnitsOnField()
        {
            currentEnemyUnits = new Dictionary<UnitData, Unit>();
            foreach (UnitData enemyData in currentCombatDetails.enemies)
            {
                Unit enemyInstance = Instantiate(unitInstancePrefab);
                enemyInstance.UnitData = enemyData;
                enemyInstance.isPlayerUnit = false;
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

        public void BuildTurnInitiativeOrder()
        {
            initiativeOrder.Clear();

            // Combine all units and sort by initiative stat
            List<Unit> allUnits = new List<Unit>();
            allUnits.AddRange(currentPlayerUnits.Values);
            allUnits.AddRange(currentEnemyUnits.Values);

            foreach (Unit unit in allUnits)
                unit.ResetRuntimeStat(ComputedStatType.STAMINA);

            // Sort by speed/initiative stat (highest first)
            initiativeOrder = allUnits.OrderByDescending(unit => unit.RuntimeStats[ComputedStatType.INITIATIVE]).ToList();
            currentInitiativeIndex = 0;
        }
    }
}