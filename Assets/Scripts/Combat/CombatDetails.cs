using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    [CreateAssetMenu(fileName = "Combat Details", menuName = "RobbieWagnerGames/Combat/Combat Details")]
    public class CombatDetails : ScriptableObject
    {
        public List<Unit> enemies;
        public string combatSceneName;
        // TODO: Figure out randomization later for enemy stats and such
    }
}