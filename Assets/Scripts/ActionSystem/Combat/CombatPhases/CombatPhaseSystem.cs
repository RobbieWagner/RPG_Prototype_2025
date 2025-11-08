using System;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class CombatPhaseSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            CombatActionSystem.AttachPerformer<StartCombatCA>(StartCombatPerformer);
            CombatActionSystem.AttachPerformer<InitializeRuntimeStatsCA>(InitializeRuntimeStatsPerformer);
            CombatActionSystem.AttachPerformer<EndCombatCA>(EndCombatPerformer);

            CombatActionSystem.AttachPerformer<RunActionSelectionPhaseCA>(RunActionSelectionPhasePerformer);
            CombatActionSystem.AttachPerformer<RunActionExecutionPhaseCA>(RunActionExecutionPhasePerformer);

            CombatActionSystem.AttachPerformer<StartTurnCA>(StartTurnPerformer);
            CombatActionSystem.AttachPerformer<EndTurnCA>(EndTurnPerformer);
        }

        private void OnDisable()
        {
            CombatActionSystem.DetachPerformer<StartCombatCA>();
            
            CombatActionSystem.DetachPerformer<InitializeRuntimeStatsCA>();
            CombatActionSystem.DetachPerformer<EndCombatCA>();

            CombatActionSystem.DetachPerformer<RunActionSelectionPhaseCA>();
            CombatActionSystem.DetachPerformer<RunActionExecutionPhaseCA>();

            CombatActionSystem.DetachPerformer<StartTurnCA>();
            CombatActionSystem.DetachPerformer<EndTurnCA>();
        }

        private IEnumerator StartCombatPerformer(StartCombatCA action)
        {
            yield return null;
            Debug.Log($"{action.GetType().Name} performed.");

            CombatManager.Instance.SpawnCombatUnitsOnField();
        }
        
        private IEnumerator InitializeRuntimeStatsPerformer(InitializeRuntimeStatsCA action)
        {
            yield return null;
            Debug.Log($"{action.GetType().Name} performed.");

            foreach (Unit unit in action.units)
                unit.ResetRuntimeStats();
        }

        private IEnumerator EndCombatPerformer(EndCombatCA action)
        {
            yield return null;
            Debug.Log($"{action.GetType().Name} performed.");
            // destroy field units, clear ui, tell the combat manager to end the combat
        }

        private IEnumerator RunActionSelectionPhasePerformer(RunActionSelectionPhaseCA action)
        {
            yield return null;
            Debug.Log($"{ action.GetType().Name } performed.");
        }

        private IEnumerator RunActionExecutionPhasePerformer(RunActionExecutionPhaseCA action)
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