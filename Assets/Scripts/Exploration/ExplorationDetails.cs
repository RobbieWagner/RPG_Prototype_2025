using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    [CreateAssetMenu(fileName = "Exploration Details", menuName = "RobbieWagnerGames/Exploration/Exploration Details")]
    public class ExplorationDetails : ScriptableObject
    {
        public string explorationSceneName = "ExplorationScene";
        public List<OverworldEnemy> randomEnemySpawns;
    }
}