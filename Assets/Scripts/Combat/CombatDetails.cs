using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    [CreateAssetMenu(fileName = "Combat Details", menuName = "RobbieWagnerGames/Combat/Combat Details")]
    public class CombatDetails : ScriptableObject
    {
        public List<UnitData> enemies = new List<UnitData>();
        public List<UnitData> allies = new List<UnitData>();
        public bool useSavedAllyData = true;
        public string combatSceneName = "CombatScene";
        // TODO: Figure out randomization later for enemy stats and such
    }
}