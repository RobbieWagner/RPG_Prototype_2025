using System;
using System.Collections;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class CombatPhaseSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            CombatActionSystem.AttachPerformer<StartCombatCA>(StartCombatPerformer);
            CombatActionSystem.AttachPerformer<EndCombatCA>(EndCombatPerformer);

            CombatActionSystem.AttachPerformer<RunPlayerPhaseCA>(RunPlayerPhasePerformer);
            CombatActionSystem.AttachPerformer<RunEnemyPhaseCA>(RunEnemyPhasePerformer);

            CombatActionSystem.AttachPerformer<StartTurnCA>(StartTurnPerformer);
            CombatActionSystem.AttachPerformer<EndTurnCA>(EndTurnPerformer);
        }

        private void OnDisable()
        {
            CombatActionSystem.DetachPerformer<StartCombatCA>();
            CombatActionSystem.DetachPerformer<EndCombatCA>();

            CombatActionSystem.DetachPerformer<RunPlayerPhaseCA>();
            CombatActionSystem.DetachPerformer<RunEnemyPhaseCA>();

            CombatActionSystem.DetachPerformer<StartTurnCA>();
            CombatActionSystem.DetachPerformer<EndTurnCA>();
        }

        private IEnumerator StartCombatPerformer(StartCombatCA action)
        {
            yield return null;
            Debug.Log($"{ action.GetType().Name } performed.");
        }

        private IEnumerator EndCombatPerformer(EndCombatCA action)
        {
            yield return null;
            Debug.Log($"{ action.GetType().Name } performed.");
        }

        private IEnumerator RunPlayerPhasePerformer(RunPlayerPhaseCA action)
        {
            yield return null;
            Debug.Log($"{ action.GetType().Name } performed.");
        }

        private IEnumerator RunEnemyPhasePerformer(RunEnemyPhaseCA action)
        {
            yield return null;
            Debug.Log($"{ action.GetType().Name } performed.");
        }

        private IEnumerator EndTurnPerformer(EndTurnCA action)
        {
            yield return null;
            Debug.Log($"{ action.GetType().Name } performed.");
        }

        private IEnumerator StartTurnPerformer(StartTurnCA action)
        {
            yield return null;
            Debug.Log($"{ action.GetType().Name } performed.");
        }
    }
}