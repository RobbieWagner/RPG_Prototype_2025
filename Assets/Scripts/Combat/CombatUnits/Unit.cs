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

        public IReadOnlyDictionary<ComputedStatType, int> RuntimeStats => unitData.computedStats;
        public SpriteRenderer unitSpriteRenderer;
        public CombatMove selectedCombatMove = null;
        public List<Unit> selectedTargets = new List<Unit>();
        public bool isPlayerUnit = true;
        public int unitListPriority = 0;

        public void ModifyRuntimeStat(ComputedStatType stat, int delta)
        {
            var newValue = Math.Clamp(
                RuntimeStats[stat] + delta,
                0,
                GetComputedStatDefaultValue(stat));
            
            SetRuntimeStatValue(stat, newValue);
        }

        public void SetRuntimeStatValue(ComputedStatType statType, int value)
        {
            unitData.computedStats[statType] = value;
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
            unitData.computedStats.Clear();

            unitData.computedStats.Add(ComputedStatType.STAMINA, GetComputedStatDefaultValue(ComputedStatType.STAMINA));
            unitData.computedStats.Add(ComputedStatType.ACCURACY, GetComputedStatDefaultValue(ComputedStatType.ACCURACY));
            unitData.computedStats.Add(ComputedStatType.MAGIC_DEFENSE, GetComputedStatDefaultValue(ComputedStatType.MAGIC_DEFENSE));

            unitData.computedStats.Add(ComputedStatType.POWER, GetComputedStatDefaultValue(ComputedStatType.POWER));
            unitData.computedStats.Add(ComputedStatType.DEFENSE, GetComputedStatDefaultValue(ComputedStatType.DEFENSE));
            unitData.computedStats.Add(ComputedStatType.HP, GetComputedStatDefaultValue(ComputedStatType.HP));

            unitData.computedStats.Add(ComputedStatType.MAGIC_POWER, GetComputedStatDefaultValue(ComputedStatType.MAGIC_POWER));
            unitData.computedStats.Add(ComputedStatType.CRIT_CHANCE, GetComputedStatDefaultValue(ComputedStatType.CRIT_CHANCE));
            unitData.computedStats.Add(ComputedStatType.INITIATIVE, GetComputedStatDefaultValue(ComputedStatType.INITIATIVE));

            OnUpdateRuntimeStat?.Invoke(ComputedStatType.STAMINA, unitData.computedStats[ComputedStatType.STAMINA]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.ACCURACY, unitData.computedStats[ComputedStatType.ACCURACY]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.MAGIC_DEFENSE, unitData.computedStats[ComputedStatType.MAGIC_DEFENSE]);
            
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.POWER, unitData.computedStats[ComputedStatType.POWER]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.DEFENSE, unitData.computedStats[ComputedStatType.DEFENSE]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.HP, unitData.computedStats[ComputedStatType.HP]);
            
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.MAGIC_POWER, unitData.computedStats[ComputedStatType.MAGIC_POWER]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.CRIT_CHANCE, unitData.computedStats[ComputedStatType.CRIT_CHANCE]);
            OnUpdateRuntimeStat?.Invoke(ComputedStatType.INITIATIVE, unitData.computedStats[ComputedStatType.INITIATIVE]);
        }

        public void ResetRuntimeStat(ComputedStatType stat)
        {
            switch (stat)
            {
                case ComputedStatType.STAMINA:
                    unitData.computedStats.Remove(ComputedStatType.STAMINA);
                    unitData.computedStats.Add(ComputedStatType.STAMINA, GetComputedStatDefaultValue(ComputedStatType.STAMINA));
                    break;
                case ComputedStatType.ACCURACY:
                    unitData.computedStats.Remove(ComputedStatType.ACCURACY);
                    unitData.computedStats.Add(ComputedStatType.ACCURACY, GetComputedStatDefaultValue(ComputedStatType.ACCURACY));
                    break;
                case ComputedStatType.MAGIC_DEFENSE:
                    unitData.computedStats.Remove(ComputedStatType.MAGIC_DEFENSE);
                    unitData.computedStats.Add(ComputedStatType.MAGIC_DEFENSE, GetComputedStatDefaultValue(ComputedStatType.MAGIC_DEFENSE));
                    break;
                case ComputedStatType.POWER:
                    unitData.computedStats.Remove(ComputedStatType.POWER);
                    unitData.computedStats.Add(ComputedStatType.POWER, GetComputedStatDefaultValue(ComputedStatType.POWER));
                    break;
                case ComputedStatType.DEFENSE:
                    unitData.computedStats.Remove(ComputedStatType.DEFENSE);
                    unitData.computedStats.Add(ComputedStatType.DEFENSE, GetComputedStatDefaultValue(ComputedStatType.DEFENSE));
                    break;
                case ComputedStatType.HP:
                    unitData.computedStats.Remove(ComputedStatType.HP);
                    unitData.computedStats.Add(ComputedStatType.HP, GetComputedStatDefaultValue(ComputedStatType.HP));
                    break;
                case ComputedStatType.MAGIC_POWER:
                    unitData.computedStats.Remove(ComputedStatType.MAGIC_POWER);
                    unitData.computedStats.Add(ComputedStatType.MAGIC_POWER, GetComputedStatDefaultValue(ComputedStatType.MAGIC_POWER));
                    break;
                case ComputedStatType.CRIT_CHANCE:
                    unitData.computedStats.Remove(ComputedStatType.CRIT_CHANCE);
                    unitData.computedStats.Add(ComputedStatType.CRIT_CHANCE, GetComputedStatDefaultValue(ComputedStatType.CRIT_CHANCE));
                    break;
                case ComputedStatType.INITIATIVE:
                    unitData.computedStats.Remove(ComputedStatType.INITIATIVE);
                    unitData.computedStats.Add(ComputedStatType.INITIATIVE, GetComputedStatDefaultValue(ComputedStatType.INITIATIVE));
                    break;
                default:
                    break;
            }

            OnUpdateRuntimeStat?.Invoke(stat, unitData.computedStats[stat]);
        }

        public int GetComputedStatDefaultValue(ComputedStatType stat)
        {
            return unitData.GetComputedStatDefaultValue(stat);
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
            foreach (var stat in unitData.computedStats)
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

        public bool CanAct()
        {
            return unitData.computedStats[ComputedStatType.HP] > 0 && GetAvailableCombatMoves().Count > 0 && unitData.computedStats[ComputedStatType.STAMINA] > 0;
        }
    }
}