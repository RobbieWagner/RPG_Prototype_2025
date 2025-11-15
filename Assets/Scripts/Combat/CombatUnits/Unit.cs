using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private readonly Dictionary<ComputedStatType, int> runtimeStats = new Dictionary<ComputedStatType, int>();
        public IReadOnlyDictionary<ComputedStatType, int> RuntimeStats => runtimeStats;
        public SpriteRenderer unitSpriteRenderer;
        public CombatMove selectedCombatMove = null;
        public List<Unit> selectedTargets = new List<Unit>();
        public bool isPlayerUnit = true;

        public void SetRuntimeStatValue(ComputedStatType statType, int value)
        {
            runtimeStats[statType] = value;
            OnUpdateRuntimeStat?.Invoke(statType, value);
        }
        public Action<ComputedStatType, int> OnUpdateRuntimeStat;

        private void UpdateUnitData()
        {
            Sprite sprite = Resources.Load<Sprite>(unitData.unitSpriteFilePath);
            unitSpriteRenderer.sprite = sprite;
        }

        public void ResetRuntimeStats()
        {
            runtimeStats.Clear();

            runtimeStats.Add(ComputedStatType.STAMINA, GetComputedStatDefaultValue(ComputedStatType.STAMINA));
            runtimeStats.Add(ComputedStatType.ACCURACY, GetComputedStatDefaultValue(ComputedStatType.ACCURACY));
            runtimeStats.Add(ComputedStatType.MAGIC_DEFENSE, GetComputedStatDefaultValue(ComputedStatType.MAGIC_DEFENSE));

            runtimeStats.Add(ComputedStatType.POWER, GetComputedStatDefaultValue(ComputedStatType.POWER));
            runtimeStats.Add(ComputedStatType.DEFENSE, GetComputedStatDefaultValue(ComputedStatType.DEFENSE));
            runtimeStats.Add(ComputedStatType.HP, GetComputedStatDefaultValue(ComputedStatType.HP));

            runtimeStats.Add(ComputedStatType.MAGIC_POWER, GetComputedStatDefaultValue(ComputedStatType.MAGIC_POWER));
            runtimeStats.Add(ComputedStatType.CRIT_CHANCE, GetComputedStatDefaultValue(ComputedStatType.CRIT_CHANCE));
            runtimeStats.Add(ComputedStatType.INITIATIVE, GetComputedStatDefaultValue(ComputedStatType.INITIATIVE));

            OnUpdateRuntimeStat?.Invoke(ComputedStatType.STAMINA, runtimeStats[ComputedStatType.STAMINA]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.ACCURACY, runtimeStats[ComputedStatType.ACCURACY]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.MAGIC_DEFENSE, runtimeStats[ComputedStatType.MAGIC_DEFENSE]);
            
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.POWER, runtimeStats[ComputedStatType.POWER]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.DEFENSE, runtimeStats[ComputedStatType.DEFENSE]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.HP, runtimeStats[ComputedStatType.HP]);
            
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.MAGIC_POWER, runtimeStats[ComputedStatType.MAGIC_POWER]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.CRIT_CHANCE, runtimeStats[ComputedStatType.CRIT_CHANCE]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.INITIATIVE, runtimeStats[ComputedStatType.INITIATIVE]);
        }

        public void ResetRuntimeStat(ComputedStatType stat)
        {
            switch (stat)
            {
                case ComputedStatType.STAMINA:
                    runtimeStats.Remove(ComputedStatType.STAMINA);
                    runtimeStats.Add(ComputedStatType.STAMINA, GetComputedStatDefaultValue(ComputedStatType.STAMINA));
                    break;
                case ComputedStatType.ACCURACY:
                    runtimeStats.Remove(ComputedStatType.ACCURACY);
                    runtimeStats.Add(ComputedStatType.ACCURACY, GetComputedStatDefaultValue(ComputedStatType.ACCURACY));
                    break;
                case ComputedStatType.MAGIC_DEFENSE:
                    runtimeStats.Remove(ComputedStatType.MAGIC_DEFENSE);
                    runtimeStats.Add(ComputedStatType.MAGIC_DEFENSE, GetComputedStatDefaultValue(ComputedStatType.MAGIC_DEFENSE));
                    break;
                case ComputedStatType.POWER:
                    runtimeStats.Remove(ComputedStatType.POWER);
                    runtimeStats.Add(ComputedStatType.POWER, GetComputedStatDefaultValue(ComputedStatType.POWER));
                    break;
                case ComputedStatType.DEFENSE:
                    runtimeStats.Remove(ComputedStatType.DEFENSE);
                    runtimeStats.Add(ComputedStatType.DEFENSE, GetComputedStatDefaultValue(ComputedStatType.DEFENSE));
                    break;
                case ComputedStatType.HP:
                    runtimeStats.Remove(ComputedStatType.HP);
                    runtimeStats.Add(ComputedStatType.HP, GetComputedStatDefaultValue(ComputedStatType.HP));
                    break;
                case ComputedStatType.MAGIC_POWER:
                    runtimeStats.Remove(ComputedStatType.MAGIC_POWER);
                    runtimeStats.Add(ComputedStatType.MAGIC_POWER, GetComputedStatDefaultValue(ComputedStatType.MAGIC_POWER));
                    break;
                case ComputedStatType.CRIT_CHANCE:
                    runtimeStats.Remove(ComputedStatType.CRIT_CHANCE);
                    runtimeStats.Add(ComputedStatType.CRIT_CHANCE, GetComputedStatDefaultValue(ComputedStatType.CRIT_CHANCE));
                    break;
                case ComputedStatType.INITIATIVE:
                    runtimeStats.Remove(ComputedStatType.INITIATIVE);
                    runtimeStats.Add(ComputedStatType.INITIATIVE, GetComputedStatDefaultValue(ComputedStatType.INITIATIVE));
                    break;
                default:
                    break;
            }

            OnUpdateRuntimeStat?.Invoke(stat, runtimeStats[stat]);
        }

        public int GetComputedStatDefaultValue(ComputedStatType stat)
        {
            switch(stat)
            {
                case ComputedStatType.STAMINA:
                    return unitData.baseStats[BaseStatType.CAT];
                case ComputedStatType.ACCURACY:
                    return unitData.baseStats[BaseStatType.CAT];
                case ComputedStatType.MAGIC_DEFENSE:
                    return unitData.baseStats[BaseStatType.CAT];
                case ComputedStatType.POWER:
                    return unitData.baseStats[BaseStatType.BOY];
                case ComputedStatType.DEFENSE:
                    return unitData.baseStats[BaseStatType.BOY];
                case ComputedStatType.HP:
                    return unitData.baseStats[BaseStatType.BOY] * 10;
                case ComputedStatType.MAGIC_POWER:
                    return unitData.baseStats[BaseStatType.ISEKAI];
                case ComputedStatType.CRIT_CHANCE:
                    return unitData.baseStats[BaseStatType.ISEKAI];
                case ComputedStatType.INITIATIVE:
                    return unitData.baseStats[BaseStatType.ISEKAI];
                default:
                    return -1;
            }
        }

        public List<CombatMove> GetAvailableCombatMoves()
        {
            //TODO: Enhance by checking if the unit also has at least one action with a valid target
            return unitData.combatMoves.Where(x => x.isAvailable(this)).ToList();
        }

        public override string ToString()
        {
            string statsString = "";
            foreach (var stat in unitData.baseStats)
                statsString += $"{stat.Key}: {stat.Value}, ";
            foreach (var stat in runtimeStats)
                statsString += $"{stat.Key}: {stat.Value}, ";

            return $"{unitData.unitName}: {statsString} ";
        }

        public override bool Equals(object other)
        {
            if (other == null || !(other is Unit otherUnit))
                return false;

            return this.gameObject == otherUnit.gameObject;
        }

        public override int GetHashCode()
        {
            return this.gameObject.GetInstanceID();
        }
    }
}