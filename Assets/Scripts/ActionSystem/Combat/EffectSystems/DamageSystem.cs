using System;
using System.Collections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class DamageSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            CombatActionSystem.AttachPerformer<DealDamageCA>(DealDamagePerformer);
        }

        private void OnDisable()
        {
            CombatActionSystem.DetachPerformer<DealDamageCA>();
        }

        private IEnumerator DealDamagePerformer(DealDamageCA action)
        {
            yield return null;

            Unit unit = action.unit;
            int amount = action.amount;

            Debug.Log($"{unit.UnitData.unitName} dealt {amount} damage!");

            unit.SetRuntimeStatValue(ComputedStatType.HP, Math.Clamp(
                unit.RuntimeStats[ComputedStatType.HP] - amount,
                0,
                unit.GetComputedStatDefaultValue(ComputedStatType.HP)));
        }
    }
}