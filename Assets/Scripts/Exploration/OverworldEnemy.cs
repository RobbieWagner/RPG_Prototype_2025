using RobbieWagnerGames.RPG;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    [CreateAssetMenu(fileName = "OverworldEnemy", menuName = "RobbieWagnerGames/Exploration/Overworld Enemy")]
    public class OverworldEnemy : ScriptableObject
    {
        public CombatDetails enemyCombatEncounterDetails;
    }
}