using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public CombatMove selectedCombatMove = null;

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
            runtimeStats.Add(ComputedStatType.HP, unitData.baseStats[BaseStatType.BOY] * 10);

            runtimeStats.Add(ComputedStatType.MAGIC_POWER, unitData.baseStats[BaseStatType.ISEKAI]);
            runtimeStats.Add(ComputedStatType.CRIT_CHANCE, unitData.baseStats[BaseStatType.ISEKAI]);
            runtimeStats.Add(ComputedStatType.INITIATIVE, unitData.baseStats[BaseStatType.ISEKAI]);
        }

        public void ResetRuntimeStat(ComputedStatType stat)
        {
            switch (stat)
            {
                case ComputedStatType.STAMINA:
                    runtimeStats.Remove(ComputedStatType.STAMINA);
                    runtimeStats.Add(ComputedStatType.STAMINA, unitData.baseStats[BaseStatType.CAT]);
                    break;
                case ComputedStatType.ACCURACY:
                    runtimeStats.Remove(ComputedStatType.ACCURACY);
                    runtimeStats.Add(ComputedStatType.ACCURACY, unitData.baseStats[BaseStatType.CAT]);
                    break;
                case ComputedStatType.MAGIC_DEFENSE:
                    runtimeStats.Remove(ComputedStatType.MAGIC_DEFENSE);
                    runtimeStats.Add(ComputedStatType.MAGIC_DEFENSE, unitData.baseStats[BaseStatType.CAT]);
                    break;
                case ComputedStatType.POWER:
                    runtimeStats.Remove(ComputedStatType.POWER);
                    runtimeStats.Add(ComputedStatType.POWER, unitData.baseStats[BaseStatType.BOY]);
                    break;
                case ComputedStatType.DEFENSE:
                    runtimeStats.Remove(ComputedStatType.DEFENSE);
                    runtimeStats.Add(ComputedStatType.DEFENSE, unitData.baseStats[BaseStatType.BOY]);
                    break;
                case ComputedStatType.HP:
                    runtimeStats.Remove(ComputedStatType.MAGIC_DEFENSE);
                    runtimeStats.Add(ComputedStatType.MAGIC_DEFENSE, unitData.baseStats[BaseStatType.BOY]);
                    break;
                case ComputedStatType.MAGIC_POWER:
                    runtimeStats.Remove(ComputedStatType.MAGIC_POWER);
                    runtimeStats.Add(ComputedStatType.MAGIC_POWER, unitData.baseStats[BaseStatType.ISEKAI]);
                    break;
                case ComputedStatType.CRIT_CHANCE:
                    runtimeStats.Remove(ComputedStatType.CRIT_CHANCE);
                    runtimeStats.Add(ComputedStatType.CRIT_CHANCE, unitData.baseStats[BaseStatType.ISEKAI]);
                    break;
                case ComputedStatType.INITIATIVE:
                    runtimeStats.Remove(ComputedStatType.INITIATIVE);
                    runtimeStats.Add(ComputedStatType.INITIATIVE, unitData.baseStats[BaseStatType.ISEKAI]);
                    break;
                default:
                    break;
            }
        }

        public List<CombatMove> GetAvailableCombatMoves()
        {
            return unitData.combatMoves.Where(x => x.isAvailable(this)).ToList();
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