using System.Collections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class CombatMoveExecutionSystem : MonoBehaviour
    {
        private CombatMoveOrchestrator moveOrchestrator;

        private void Awake()
        {
            moveOrchestrator = GetComponent<CombatMoveOrchestrator>();
            if (moveOrchestrator == null)
                moveOrchestrator = gameObject.AddComponent<CombatMoveOrchestrator>();
        }

        private void OnEnable()
        {
            CombatActionSystem.AttachPerformer<ExecuteCombatMoveCA>(ExecuteCombatActionPerformer);
            // Keep the intermediate action performers for now
            CombatActionSystem.AttachPerformer<MakeAttackCA>(MakeAttackPerformer);
            CombatActionSystem.AttachPerformer<AttemptHealCA>(AttemptHealPerformer);
            CombatActionSystem.AttachPerformer<AttemptBuffCA>(AttemptBuffUnitPerformer);
            CombatActionSystem.AttachPerformer<AttemptDebuffCA>(AttemptDebuffUnitPerformer);
        }

        private void OnDisable()
        {            
            CombatActionSystem.DetachPerformer<ExecuteCombatMoveCA>();
            CombatActionSystem.DetachPerformer<MakeAttackCA>();
            CombatActionSystem.DetachPerformer<AttemptHealCA>();
            CombatActionSystem.DetachPerformer<AttemptBuffCA>();
            CombatActionSystem.DetachPerformer<AttemptDebuffCA>();
        }

        private IEnumerator ExecuteCombatActionPerformer(ExecuteCombatMoveCA action)
        {
            yield return moveOrchestrator.ExecuteCombatMove(action.combatMove, action.user, action.targets);
        }

        // Keep these intermediate performers for now - they handle accuracy checks
        private IEnumerator MakeAttackPerformer(MakeAttackCA action)
        {
            yield return null;
            foreach(Unit target in action.targets)
            {
                if (UnityEngine.Random.Range(0, 100) < action.attackEffect.accuracy)
                    yield return CombatActionSystem.Instance.PerformCo(new DealDamageCA(action.attackEffect.Power, target));
            }
        }

        private IEnumerator AttemptHealPerformer(AttemptHealCA action)
        {
            yield return null;
            foreach(Unit target in action.targets)
            {
                if (UnityEngine.Random.Range(0, 100) < action.healEffect.accuracy)
                    yield return CombatActionSystem.Instance.PerformCo(new HealUnitCA(action.healEffect.HealAmount, target));
            }
        }
        
        private IEnumerator AttemptBuffUnitPerformer(AttemptBuffCA action)
        {
            yield return null;
            foreach (Unit target in action.targets)
            {
                if (UnityEngine.Random.Range(0, 100) < action.buffEffect.accuracy)
                    yield return CombatActionSystem.Instance.PerformCo(new BuffUnitCA(action.buffEffect.BuffAmount, action.buffEffect.StatToBuff, target));
            }
        }

        private IEnumerator AttemptDebuffUnitPerformer(AttemptDebuffCA action)
        {
            yield return null;
            foreach (Unit target in action.targets)
            {
                if (UnityEngine.Random.Range(0, 100) < action.debuffEffect.accuracy)
                    yield return CombatActionSystem.Instance.PerformCo(new DebuffUnitCA(action.debuffEffect.DebuffAmount, action.debuffEffect.StatToDebuff, target));
            }
        }
    }
}