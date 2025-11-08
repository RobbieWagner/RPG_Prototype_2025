using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    /// <summary>
    /// Data class for combat units. This class should be treated as readonly at runtime.
    /// Any changes to the units stats or moves in combat should be handled through a separate system.
    /// </summary>
    [Serializable]
    public class UnitData
    {
        public string unitName = "New Unit";
        public Sprite unitIcon;
        [SerializedDictionary("Stat", "Value")]
        public SerializedDictionary<BaseStatType, int> baseStats = new SerializedDictionary<BaseStatType, int>();
        public List<CombatMove> combatMoves = new List<CombatMove>();

        // sprite path for the unit in the resources folder
        public string unitSpriteFilePath = "";
    }
}