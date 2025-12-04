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
            int stage = runDetails.stagesCompleted;
            List<UnitData> enemyOptions = combatEnemyOptions[stage];

            combatDetails.enemies.Clear();
            
            while (combatDetails.enemies.Count < 3)
            {
                UnitData selectedEnemy = enemyOptions[RunManager.Instance.runRNG.Next(0, enemyOptions.Count)];
                UnitData enemyCopy = CreateDeepCopy(selectedEnemy);
                combatDetails.enemies.Add(enemyCopy);
            }

            EnsureUniqueEnemyNames(combatDetails.enemies);
        }

        private UnitData CreateDeepCopy(UnitData original)
        {
            UnitData copy = new UnitData();
            
            copy.unitName = original.unitName;
            copy.unitIcon = original.unitIcon;
            copy.unitSpriteFilePath = original.unitSpriteFilePath;
            copy.unitColor = original.unitColor;
            
            copy.baseStats = new SerializedDictionary<BaseStatType, int>();
            foreach (KeyValuePair<BaseStatType, int>  baseStat in original.baseStats)
                copy.baseStats.Add(baseStat.Key, baseStat.Value);
            
            copy.computedStats = new SerializedDictionary<ComputedStatType, int>();
            foreach (KeyValuePair<ComputedStatType, int> computedStat in original.computedStats)
                copy.computedStats.Add(computedStat.Key, computedStat.Value);
            
            copy.combatMoves = new List<CombatMove>(original.combatMoves);
            
            return copy;
        }

        private void EnsureUniqueEnemyNames(List<UnitData> enemies)
        {
            Dictionary<string, int> nameCount = new Dictionary<string, int>();
            Dictionary<string, List<int>> nameIndices = new Dictionary<string, List<int>>();
            
            for (int i = 0; i < enemies.Count; i++)
            {
                string originalName = enemies[i].unitName;
                
                if (!nameCount.ContainsKey(originalName))
                {
                    nameCount[originalName] = 0;
                    nameIndices[originalName] = new List<int>();
                }
                
                nameCount[originalName]++;
                nameIndices[originalName].Add(i);
            }
            
            foreach (KeyValuePair<string, int> nameAndIndex in nameCount)
            {
                if (nameAndIndex.Value > 1)
                {
                    char suffix = 'A';
                    foreach (int index in nameIndices[nameAndIndex.Key])
                    {
                        enemies[index].unitName = $"{nameAndIndex.Key} {suffix}";
                        suffix++;
                    }
                }
            }
        }
    }
}