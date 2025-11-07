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

    public void PlaceAllies(Dictionary<UnitData, Unit> allyUnits = null)
    {
        if (allyUnits == null) return;

        int i = 0;
        foreach (var unitPair in allyUnits)
        {
            if (i >= allySpawnPoints.Count) 
                break;

            Unit unit = unitPair.Value;
            Transform spawnPoint = allySpawnPoints[i];

            // Place the unit's transform at the spawn point position
            unit.transform.position = spawnPoint.position;
            unit.transform.parent = spawnPoint;
            i++;
        }
    }

    public void PlaceEnemies(Dictionary<UnitData, Unit> enemyUnits = null)
    {
        if (enemyUnits == null) return;

        int i = 0;
        foreach (var unitPair in enemyUnits)
        {
            if (i >= enemySpawnPoints.Count) 
                break;

            Unit unit = unitPair.Value;
            Transform spawnPoint = enemySpawnPoints[i];

            // Place the unit's transform at the spawn point position
            unit.transform.position = spawnPoint.position;
            unit.transform.parent = spawnPoint;
            i++;
        }
    }
}
}