using System;
using System.Collections;
using RobbieWagnerGames.Managers;
using RobbieWagnerGames.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RobbieWagnerGames.RPG
{
    public class ExplorationManager : MonoBehaviourSingleton<ExplorationManager>
    {
        [SerializeField] private ExplorationDetails testExplorationDetails = null;
        [HideInInspector] public ExplorationDetails currentExplorationDetails = null;
        [SerializeField] private CharacterMovement2D defaultPlayerPrefab;
        // Sets up the exploration scene, spawns the player, and relevant random encounter enemies
    
        public virtual void StartExploration(ExplorationDetails explorationDetails)
        {
            if (currentExplorationDetails != null)
            {
                Debug.LogWarning("Exploration is already in progress!");
                return;
            }

            StartCoroutine(StartExplorationCo(explorationDetails));
        }

        public virtual IEnumerator StartExplorationCo(ExplorationDetails explorationDetails)
        {
            currentExplorationDetails = explorationDetails;
            yield return null;
            Debug.Log(SceneLoadManager.Instance == null);
            yield return SceneLoadManager.Instance.LoadSceneAdditive(explorationDetails.explorationSceneName, () => OnExplorationSceneLoaded()); 
        }

        protected void OnExplorationSceneLoaded()
        {
            SpawnPlayer();
            SpawnOverworldEnemies();

            InputManager.Instance.EnableActionMap(ActionMapName.EXPLORATION);
        }

        private void SpawnPlayer()
        {
            Instantiate(defaultPlayerPrefab);
        }

        private void SpawnOverworldEnemies()
        {
            // Spawn enemies in the world based on allowed placements (AI NavMesh)
            // Make sure not to place them too close to the player
        }
    }
}