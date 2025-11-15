using System;
using System.Collections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{ 
    public class StatusEffectSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            CombatActionSystem.AttachPerformer<BuffUnitCA>(BuffUnitPerformer);
            CombatActionSystem.AttachPerformer<DebuffUnitCA>(DebuffUnitPerformer);
        }

        private void OnDisable()
        {
            CombatActionSystem.DetachPerformer<BuffUnitCA>();
            CombatActionSystem.DetachPerformer<DebuffUnitCA>();
        }

        private IEnumerator BuffUnitPerformer(BuffUnitCA action)
        {
            yield return null;

            Unit unit = action.unit;
            ComputedStatType stat = action.stat;
            int amount = action.amount;
            
            unit.SetRuntimeStatValue(stat, Math.Clamp(
                unit.RuntimeStats[stat] + amount,
                0,
                unit.GetComputedStatDefaultValue(stat)));
        }

        private IEnumerator DebuffUnitPerformer(DebuffUnitCA action)
        {
            yield return null;

            Unit unit = action.unit;
            ComputedStatType stat = action.stat;
            int amount = action.amount;

            unit.SetRuntimeStatValue(stat, Math.Clamp(
                unit.RuntimeStats[stat] - amount,
                0,
                unit.GetComputedStatDefaultValue(stat)));
        }
    }
}