using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    [Serializable]
    public class CombatDetails
    {
        public List<UnitData> enemies = new List<UnitData>();
        public List<UnitData> allies = new List<UnitData>();
        public string combatSceneName = "CombatScene";
        // TODO: Figure out randomization later for enemy stats and such
    }
}