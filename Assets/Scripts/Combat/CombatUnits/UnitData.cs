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
        public SerializedDictionary<ComputedStatType, int> computedStats = new SerializedDictionary<ComputedStatType, int>();
        public List<CombatMove> combatMoves = new List<CombatMove>();

        // sprite path for the unit in the resources folder
        public string unitSpriteFilePath = "";
        public Color unitColor = Color.white;

        public void ResetStats(bool resetHealth)
        {
            int healthValue = computedStats[ComputedStatType.HP];

            foreach(ComputedStatType statType in computedStats.Keys)
                computedStats[statType] = GetComputedStatDefaultValue(statType);

            if(!resetHealth)
                computedStats[ComputedStatType.HP] = healthValue;
        }

        public int GetComputedStatDefaultValue(ComputedStatType stat)
        {
            switch(stat)
            {
                case ComputedStatType.STAMINA:
                    return baseStats[BaseStatType.CAT];
                case ComputedStatType.ACCURACY:
                    return baseStats[BaseStatType.CAT];
                case ComputedStatType.MAGIC_DEFENSE:
                    return baseStats[BaseStatType.CAT];
                case ComputedStatType.POWER:
                    return baseStats[BaseStatType.BOY];
                case ComputedStatType.DEFENSE:
                    return baseStats[BaseStatType.BOY];
                case ComputedStatType.HP:
                    return baseStats[BaseStatType.BOY] * 10;
                case ComputedStatType.MAGIC_POWER:
                    return baseStats[BaseStatType.ISEKAI];
                case ComputedStatType.CRIT_CHANCE:
                    return baseStats[BaseStatType.ISEKAI];
                case ComputedStatType.INITIATIVE:
                    return baseStats[BaseStatType.ISEKAI];
                default:
                    return -1;
            }
        }
    }
}