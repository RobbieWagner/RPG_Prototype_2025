using System;
using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class Unit : MonoBehaviour
    {
        private UnitData unitData = null;
        public UnitData UnitData
        {
            get
            {
                return unitData;
            }
            set
            {
                unitData = value;
                UpdateUnitData();
            }
        }

        public Dictionary<ComputedStatType, int> runtimeStats = new Dictionary<ComputedStatType, int>();

        public SpriteRenderer unitSpriteRenderer;

        private void UpdateUnitData()
        {
            Sprite sprite = Resources.Load<Sprite>(unitData.unitSpriteFilePath);
            unitSpriteRenderer.sprite = sprite;
        }

        public void ResetRuntimeStats()
        {
            runtimeStats.Clear();

            runtimeStats.Add(ComputedStatType.STAMINA, unitData.baseStats[BaseStatType.CAT]);
            runtimeStats.Add(ComputedStatType.ACCURACY, unitData.baseStats[BaseStatType.CAT]);
            runtimeStats.Add(ComputedStatType.MAGIC_DEFENSE, unitData.baseStats[BaseStatType.CAT]);

            runtimeStats.Add(ComputedStatType.POWER, unitData.baseStats[BaseStatType.BOY]);
            runtimeStats.Add(ComputedStatType.DEFENSE, unitData.baseStats[BaseStatType.BOY]);
            runtimeStats.Add(ComputedStatType.VITALITY, unitData.baseStats[BaseStatType.BOY]);

            runtimeStats.Add(ComputedStatType.MAGIC_POWER, unitData.baseStats[BaseStatType.ISEKAI]);
            runtimeStats.Add(ComputedStatType.CRIT_CHANCE, unitData.baseStats[BaseStatType.ISEKAI]);
            runtimeStats.Add(ComputedStatType.INITIATIVE, unitData.baseStats[BaseStatType.ISEKAI]);
        }

        public override string ToString()
        {
            string statsString = "";
            foreach (var stat in unitData.baseStats)
            {
                statsString += $"{stat.Key}: {stat.Value}, ";
            }
            foreach (var stat in runtimeStats)
            {
                statsString += $"{stat.Key}: {stat.Value}, ";
            }

            return $"{unitData.unitName}: {statsString} ";
        }
    }
}