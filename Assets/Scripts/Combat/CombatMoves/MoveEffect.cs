using System;
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
    }

    [Serializable]
    public class Attack : MoveEffect
    {
        [Header("Attack")]
        [SerializeField] private int power = 10;
        public int Power => power;
    }

    [Serializable]
    public class Heal : MoveEffect
    {
        [Header("Heal")]
        [SerializeField] private int healAmount = 10;
        public int HealAmount => healAmount;
    }

    [Serializable]
    public class Buff : MoveEffect
    {
        [Header("Buff")]
        [SerializeField] private ComputedStatType statToBuff = ComputedStatType.NONE;
        [SerializeField] private int buffAmount = 5;
        public ComputedStatType StatToBuff => statToBuff;
        public int BuffAmount => buffAmount;
    }

    [Serializable]
    public class Debuff : MoveEffect
    {
        [Header("Debuff")]
        [SerializeField] private ComputedStatType statToDebuff = ComputedStatType.NONE;
        [SerializeField] private int debuffAmount = 5;
        public ComputedStatType StatToDebuff => statToDebuff;
        public int DebuffAmount => debuffAmount;
    }
}