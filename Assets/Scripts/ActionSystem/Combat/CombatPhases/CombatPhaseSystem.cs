using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            CombatActionSystem.AttachPerformer<StartTurnCA>(StartTurnPerformer);
            CombatActionSystem.AttachPerformer<EndTurnCA>(EndTurnPerformer);
            }

        private void OnDisable()
        {
            CombatActionSystem.DetachPerformer<StartCombatCA>();
            
            CombatActionSystem.DetachPerformer<InitializeRuntimeStatsCA>();
            CombatActionSystem.DetachPerformer<EndCombatCA>();

            CombatActionSystem.DetachPerformer<RunActionSelectionPhaseCA>();

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
        }

        private IEnumerator RunActionSelectionPhasePerformer(RunActionSelectionPhaseCA action)
        {
            yield return null;

            Unit unit = action.Unit;

            if(unit.isPlayerUnit)
                yield return RunUserActionSelection(unit, action);
            else
            {
                unit.selectedCombatMove = AutoSelectCombatAction(unit, action);
                if (unit.selectedCombatMove == null)
                    yield break;
                yield return null;
                unit.selectedTargets = AutoSelectMoveTargets(unit, unit.selectedCombatMove, action);
            }

            Debug.Log($"{unit.UnitData.unitName} selected move: {unit.selectedCombatMove.moveName}");
        }

        private IEnumerator RunUserActionSelection(Unit selectingUnit, RunActionSelectionPhaseCA action)
        {
            yield return null;

            CombatSelectionUI.Instance.StartActionSelection();

            while(selectingUnit.selectedCombatMove == null || selectingUnit.selectedTargets == null || !selectingUnit.selectedTargets.Any())
                yield return null;
        } 

        private CombatMove AutoSelectCombatAction(Unit selectingUnit, RunActionSelectionPhaseCA action)
        {
            List<CombatMove> moves = selectingUnit.GetAvailableCombatMoves();

            if (moves.Count == 0)
            {
                selectingUnit.SetRuntimeStatValue(ComputedStatType.STAMINA, 0);
                return null;
            }

            return moves[Random.Range(0, moves.Count)];
        }

        private List<Unit> AutoSelectMoveTargets(Unit selectingUnit, CombatMove selectedCombatMove, RunActionSelectionPhaseCA action)
        {
            return selectedCombatMove?.GetRandomTargets(selectingUnit, action.targetOptions) ?? new List<Unit>();
        }

        private IEnumerator EndTurnPerformer(EndTurnCA action)
        {
            yield return null;
            Debug.Log($"{ action.GetType().Name } performed.");
        }

        private IEnumerator StartTurnPerformer(StartTurnCA action)
        {
            yield return null;
            CombatManager.Instance.currentTurn++;
            CombatManager.Instance.BuildTurnInitiativeOrder();
            Debug.Log($"{action.GetType().Name} performed.");
            foreach (Unit unit in CombatManager.Instance.allCurrentUnits)
                unit.ResetRuntimeStat(ComputedStatType.STAMINA);
        }
    }
}