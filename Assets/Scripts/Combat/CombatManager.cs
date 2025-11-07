using System;
using System.Collections;
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
        private Dictionary<UnitData, Unit> currentPlayerUnits;
        private Dictionary<UnitData, Unit> currentEnemyUnits;
        [SerializeField] private Unit unitInstancePrefab = null;

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
    }
}