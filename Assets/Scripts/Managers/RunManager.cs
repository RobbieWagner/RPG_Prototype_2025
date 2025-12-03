using System;
using RobbieWagnerGames.Utilities;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class RunManager : MonoBehaviourSingleton<RunManager>
    {
        private RunDetails runDetails;
        public RunDetails RunDetails => runDetails;
        [SerializeField] private string runSceneName = "RunScene";

        public RandomNumberGenerator runRNG {get; private set;} 

        protected override void Awake()
        {
            base.Awake();
        }

        public void PrepRunScene(RunDetails details = null)
        {
            if(details == null)
                InitializeNewRun();
            else
            {
                runDetails = details;
                runDetails.newRun = false;
            }

            StartCoroutine(SceneLoadManager.Instance.LoadSceneAdditive(runSceneName, () => StartRun()));

            Debug.Log(runDetails);
        }

        private void InitializeNewRun()
        {
            runDetails = new RunDetails();
            
            runDetails.runSeed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            runDetails.rngPulls = 0;
            runDetails.kerfufflesWon = 0;
            runDetails.stagesCompleted = 0;
            runDetails.unitOptions = GameManager.Instance.defaultPlayerUnitOptions; 
            runDetails.newRun = true;

            runDetails.PlayerParty.Clear();
            if (GameManager.Instance.SaveData != null && GameManager.Instance.SaveData.mainPlayerUnit != null)
                runDetails.playerCustomUnit = GameManager.Instance.SaveData.mainPlayerUnit;
            else runDetails.playerCustomUnit = GameManager.Instance.defaultMainPlayerUnit;

            runDetails.PlayerParty.Add(runDetails.playerCustomUnit);
        }

        public void StartRun()
        {
            runRNG = new RandomNumberGenerator(runDetails.runSeed, runDetails.rngPulls);

            if(runDetails.newRun)
                NewRunController.Instance.HandleNewRun(() => RunCombatController.Instance.StartNextCombat(runDetails));
            else
                RunCombatController.Instance.StartNextCombat(runDetails);
        }

        
        public void StartRunAfterUnitSelection()
        {
            if (runDetails?.PlayerParty == null || runDetails.PlayerParty.Count == 0)
                throw new NullReferenceException("Cannot start run with empty party!");

            PrepRunScene(runDetails);
        }

        public void EndRun(bool won = false)
        {
            if (won)
                Debug.Log("Run completed successfully!");
            else
                Debug.Log("Run failed!");

            runDetails = null;

            Debug.Log("RUN COMPLETE");
        }
    }
}