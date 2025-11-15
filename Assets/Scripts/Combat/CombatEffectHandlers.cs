using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class AttackEffectHandler : MoveEffectHandler<Attack>
    {
        protected override IEnumerator HandleEffectTyped(Attack effect, Unit user, List<Unit> targets)
        {
            // Create and execute the attack action through the existing action system
            var attackAction = new MakeAttackCA
            {
                attackEffect = effect,
                user = user,
                targets = targets
            };
            
            yield return CombatActionSystem.Instance.PerformCo(attackAction);
        }
    }

    public class HealEffectHandler : MoveEffectHandler<Heal>
    {
        protected override IEnumerator HandleEffectTyped(Heal effect, Unit user, List<Unit> targets)
        {
            var healAction = new AttemptHealCA
            {
                healEffect = effect,
                user = user,
                targets = targets
            };
            
            yield return CombatActionSystem.Instance.PerformCo(healAction);
        }
    }

    public class BuffEffectHandler : MoveEffectHandler<Buff>
    {
        protected override IEnumerator HandleEffectTyped(Buff effect, Unit user, List<Unit> targets)
        {
            var buffAction = new AttemptBuffCA
            {
                buffEffect = effect,
                user = user,
                targets = targets
            };
            
            yield return CombatActionSystem.Instance.PerformCo(buffAction);
        }
    }

    public class DebuffEffectHandler : MoveEffectHandler<Debuff>
    {
        protected override IEnumerator HandleEffectTyped(Debuff effect, Unit user, List<Unit> targets)
        {
            var debuffAction = new AttemptDebuffCA
            {
                debuffEffect = effect,
                user = user,
                targets = targets
            };
            
            yield return CombatActionSystem.Instance.PerformCo(debuffAction);
        }
    }
}