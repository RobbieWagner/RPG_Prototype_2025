using System;
using System.Collections;
using RobbieWagnerGames.Managers;
using RobbieWagnerGames.Utilities;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using NavMeshPlus.Components;
using NavMeshPlus.Extensions;
using RobbieWagnerGames.AI;

namespace RobbieWagnerGames.RPG
{
    public class ExplorationManager : MonoBehaviourSingleton<ExplorationManager>
    {
        [SerializeField] private ExplorationDetails testExplorationDetails = null;
        [HideInInspector] public ExplorationDetails currentExplorationDetails = null;
        [SerializeField] private CharacterMovement2D defaultPlayerPrefab;

        private NavMeshSurface navMesh = null;
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
            navMesh = FindFirstObjectByType<NavMeshSurface>();

            if (navMesh != null)
                navMesh.BuildNavMesh();

            Vector2 spawnPos = (Vector2) NavMeshExtensions.GetRandomNavMeshPositionOnCircle(Vector3.zero, 5f, 2f);
        }
    }
}