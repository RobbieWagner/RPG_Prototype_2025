using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public interface ICombatMoveHandler
    {
        bool CanHandle(MoveEffect effect);
        IEnumerator HandleEffect(MoveEffect effect, Unit user, List<Unit> targets);
    }

    public class CombatMoveOrchestrator : MonoBehaviour
    {
        private List<ICombatMoveHandler> _handlers;
        
        private void Awake()
        {
            _handlers = new List<ICombatMoveHandler>
            {
                new AttackEffectHandler(),
                new HealEffectHandler(),
                new BuffEffectHandler(),
                new DebuffEffectHandler()
            };
        }
        
        public IEnumerator ExecuteCombatMove(CombatMove move, Unit user, List<Unit> targets)
        {
            if (move == null) yield break;
            
            yield return CombatHUD.Instance.DisplayCombatMoveExecutionDetails(move);

            foreach (var effect in move.effects)
            {
                var handler = _handlers.FirstOrDefault(h => h.CanHandle(effect));
                if (handler != null)
                {
                    yield return handler.HandleEffect(effect, user, targets);
                }
                else
                {
                    Debug.LogWarning($"Encountered unknown move effect type: {effect.GetType().Name}");
                }
            }
        }
    }

    public abstract class MoveEffectHandler<T> : ICombatMoveHandler where T : MoveEffect
    {
        public bool CanHandle(MoveEffect effect) => effect is T;
        
        public IEnumerator HandleEffect(MoveEffect effect, Unit user, List<Unit> targets)
        {
            return HandleEffectTyped((T)effect, user, targets);
        }
        
        protected abstract IEnumerator HandleEffectTyped(T effect, Unit user, List<Unit> targets);
    }
}