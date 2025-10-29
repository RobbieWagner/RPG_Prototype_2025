using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public enum MoveEffectType
    {
        NONE = -1,
        DAMAGE = 0,
        HEAL = 1,
        BUFF = 2,
        DEBUFF = 3
    }

    [Serializable]
    public class MoveEffect
    {
        [Header("General")]
        [SerializeField][Range(1, 101)] public float accuracy = 101f;
        [SerializeField] private bool failureStopsActionExecution;
        public virtual bool FailureStopsActionExecution => failureStopsActionExecution;

        public virtual bool TryEffectApply(Unit user, List<Unit> targets)
        {
            return true;
        }
    }

    [Serializable]
    public class Attack : MoveEffect
    {
        [Header("Attack")]
        [SerializeField] private int power = 10;
        public int Power => power;

        public override bool TryEffectApply(Unit user, List<Unit> targets)
        {
            // Implement attack logic here
            Debug.Log($"{user.unitName} attacks with power {power}!");
            return true;
        }
    }

    [Serializable]
    public class Heal : MoveEffect
    {
        [Header("Heal")]
        [SerializeField] private int healAmount = 10;
        public int HealAmount => healAmount;

        public override bool TryEffectApply(Unit user, List<Unit> targets)
        {
            // Implement heal logic here
            Debug.Log($"{user.unitName} heals for {healAmount}!");
            return true;
        }
    }

    [Serializable]
    public class Buff : MoveEffect
    {
        [Header("Buff")]
        [SerializeField] private StatType statToBuff = StatType.NONE;
        [SerializeField] private int buffAmount = 5;
        public StatType StatToBuff => statToBuff;
        public int BuffAmount => buffAmount;

        public override bool TryEffectApply(Unit user, List<Unit> targets)
        {
            // Implement buff logic here
            Debug.Log($"{user.unitName} buffs {statToBuff} by {buffAmount}!");
            return true;
        }
    }

    [Serializable]
    public class Debuff : MoveEffect
    {
        [Header("Debuff")]
        [SerializeField] private StatType statToDebuff = StatType.NONE;
        [SerializeField] private int debuffAmount = 5;
        public StatType StatToDebuff => statToDebuff;
        public int DebuffAmount => debuffAmount;

        public override bool TryEffectApply(Unit user, List<Unit> targets)
        {
            // Implement debuff logic here
            Debug.Log($"{user.unitName} debuffs {statToDebuff} by {debuffAmount}!");
            return true;
        }
    }
}