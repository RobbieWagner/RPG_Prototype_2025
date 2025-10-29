using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class Unit
    {
        public string unitName;
        public Sprite unitIcon;
        [SerializedDictionary("Stat", "Value")]
        public SerializedDictionary<StatType, int> baseStats = new SerializedDictionary<StatType, int>();
        public List<CombatMove> combatMoves = new List<CombatMove>();
    }
}