using System.Collections.Generic;
using RobbieWagnerGames.Utilities;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class BattlefieldController : MonoBehaviourSingleton<BattlefieldController>
    {
        [SerializeField] private List<Transform> allySpawnPoints = new List<Transform>();
        [SerializeField] private List<Transform> enemySpawnPoints = new List<Transform>();

        protected override void Awake()
        {
            base.Awake();
        }

        public void PlaceAllies(List<Transform> allyInstances)
        {
            for (int i = 0; i < allyInstances.Count && i < allySpawnPoints.Count; i++)
            {
                Transform ally = allyInstances[i];
                Transform spawnPoint = allySpawnPoints[i];

                ally.position = spawnPoint.position;
            }
        }

        public void PlaceEnemies(List<Transform> enemyInstances)
        {
            for (int i = 0; i < enemyInstances.Count && i < enemySpawnPoints.Count; i++)
            {
                Transform enemy = enemyInstances[i];
                Transform spawnPoint = enemySpawnPoints[i];

                enemy.position = spawnPoint.position;
            }
        }
    }
}