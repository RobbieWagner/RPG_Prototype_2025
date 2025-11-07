using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    [Serializable]
    public class UnitData
    {
        public string unitName = "New Unit";
        public Sprite unitIcon;
        [SerializedDictionary("Stat", "Value")]
        public SerializedDictionary<StatType, int> baseStats = new SerializedDictionary<StatType, int>();
        public List<CombatMove> combatMoves = new List<CombatMove>();

        // sprite path for the unit in the resources folder
        public string unitSpriteFilePath = "";
    }
}