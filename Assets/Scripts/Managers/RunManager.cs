using System;
using RobbieWagnerGames.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RobbieWagnerGames.RPG
{
    public class RunManager : MonoBehaviourSingleton<RunManager>
    {
        private RunDetails runDetails;
        public RunDetails RunDetails => runDetails;
        [SerializeField] private string runStartSceneName = "RunStartScene";

        protected override void Awake()
        {
            base.Awake();
        }

        public void StartRun(RunDetails details)
        {
            if(details == null)
                throw new NullReferenceException("Run Details cannot be null. Please provide valid run details or use StartNewRun to begin a new run.");

            runDetails = details;

            Debug.Log(runDetails);
        }

        public void StartNewRun()
        {
            runDetails = new RunDetails();
            
            runDetails.runSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);

            runDetails.unitOptions = GameManager.Instance.defaultPlayerUnitOptions; 

            runDetails.PlayerParty.Clear();
            if (GameManager.Instance.SaveData != null && GameManager.Instance.SaveData.mainPlayerUnit != null)
                runDetails.playerCustomUnit = GameManager.Instance.SaveData.mainPlayerUnit;
            else runDetails.playerCustomUnit = GameManager.Instance.defaultMainPlayerUnit;

            runDetails.PlayerParty.Add(runDetails.playerCustomUnit);

            StartCoroutine(SceneLoadManager.Instance.LoadSceneAdditive(runStartSceneName));
        }

        // New method to handle run start after unit selection
        public void StartRunAfterUnitSelection()
        {
            // Validate that we have at least one unit in the party
            if (runDetails?.PlayerParty == null || runDetails.PlayerParty.Count == 0)
            {
                Debug.LogError("Cannot start run with empty party!");
                return;
            }

            SceneManager.UnloadSceneAsync(runStartSceneName);

            StartRun(runDetails);
        }

        private void InitializeRun()
        {
            // Set the random seed for consistent run generation
            UnityEngine.Random.InitState(runDetails.runSeed);

            // Reset run progress
            runDetails.kerfufflesWon = 0;

            Debug.Log($"Run started with seed: {runDetails.runSeed} and {runDetails.PlayerParty.Count} units");
        }

        public void EndRun(bool won = false)
        {
            // Handle run completion logic
            if (won)
                Debug.Log("Run completed successfully!");
            else
                Debug.Log("Run failed!");

            // Clear run details
            runDetails = null;

            Debug.Log("RUN COMPLETE");
        }
    }
}