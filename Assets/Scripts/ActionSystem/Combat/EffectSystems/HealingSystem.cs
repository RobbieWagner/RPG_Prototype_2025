using System;
using System.Collections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class HealingSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            CombatActionSystem.AttachPerformer<HealUnitCA>(HealUnitPerformer);
        }

        private void OnDisable()
        {
            CombatActionSystem.DetachPerformer<HealUnitCA>();
        }

        private IEnumerator HealUnitPerformer(HealUnitCA action)
        {
            yield return null;

            Unit unit = action.unit;
            int amount = action.amount;

            unit.SetRuntimeStatValue(ComputedStatType.HP, Math.Clamp(
                unit.RuntimeStats[ComputedStatType.HP] + amount,
                0,
                unit.GetComputedStatDefaultValue(ComputedStatType.HP)));       
        }
    }
}