using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using RobbieWagnerGames.Utilities;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class RunCombatController : MonoBehaviourSingleton<RunCombatController>
    {
        [SerializedDictionary("Stage","Unit List"), SerializeField] private SerializedDictionary<int, List<UnitData>> combatEnemyOptions = new SerializedDictionary<int, List<UnitData>>();
        [SerializeField] private List<string> combatSceneOptionNames = new List<string>();

        public void StartNextCombat(RunDetails runDetails)
        {
            if (runDetails == null)
                throw new ArgumentNullException("Run Details value cannot be null");

            CombatDetails newCombat = new CombatDetails();
            InitializeCombatDetails(runDetails, newCombat);

            CombatManager.Instance.StartCombat(newCombat);
        }

        private void InitializeCombatDetails(RunDetails runDetails, CombatDetails combatDetails)
        {
            combatDetails.allies = runDetails.PlayerParty;
            combatDetails.combatSceneName = combatSceneOptionNames[RunManager.Instance.runRNG.Next(0, combatSceneOptionNames.Count)];
            combatDetails.enemies = new List<UnitData>();
            PopulateEnemiesList(runDetails, combatDetails);
        }

        private void PopulateEnemiesList(RunDetails runDetails, CombatDetails combatDetails)
        {
            // TODO: Ensure uniqueness of enemies in some way (needed for dictionary refs)
            // Add "1, 2, 3" / "a, b, c"?
            // Update how these elements are added?
            int stage = runDetails.stagesCompleted;
            List<UnitData> enemyOptions = combatEnemyOptions[stage];

            while (combatDetails.enemies.Count < 3)
                combatDetails.enemies.Add(enemyOptions[RunManager.Instance.runRNG.Next(0, enemyOptions.Count)]);
        }
    }
}