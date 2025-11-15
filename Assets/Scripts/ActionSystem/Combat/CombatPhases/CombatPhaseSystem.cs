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

            //TODO: IMPLEMENT!!!!!

            selectingUnit.selectedCombatMove = AutoSelectCombatAction(selectingUnit, action);
            if (selectingUnit.selectedCombatMove == null)
                yield break;
            
            yield return null;
            selectingUnit.selectedTargets = AutoSelectMoveTargets(selectingUnit, selectingUnit.selectedCombatMove, action);
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

            List<Unit> selectedTargets = new List<Unit>();
            
            if(selectedCombatMove.targetsAllUnits) //BUG
            {
                return selectedCombatMove.canTargetSelf 
                    ? action.targetOptions 
                    : action.targetOptions.Where(x => !x.Equals(selectingUnit)).ToList();
            } 

            if(selectedCombatMove.targetsAllAllies)
            {
                return selectingUnit.isPlayerUnit ? 
                action.targetOptions.Where(x => x.isPlayerUnit).ToList() :
                action.targetOptions.Where(x => !x.isPlayerUnit).ToList();
            }

            if(selectedCombatMove.targetsAllOpposition)
            {
                return selectingUnit.isPlayerUnit ? 
                action.targetOptions.Where(x => !x.isPlayerUnit).ToList() :
                action.targetOptions.Where(x => x.isPlayerUnit).ToList();
            }

            List<Unit> validOptions = new List<Unit>();

            if(selectedCombatMove.canTargetAllies)
                validOptions.AddRange(
                    selectingUnit.isPlayerUnit 
                    ? action.targetOptions.Where(x => x.isPlayerUnit && x != selectingUnit).ToList()
                    : action.targetOptions.Where(x => !x.isPlayerUnit && x != selectingUnit).ToList()
                );
            
            if(selectedCombatMove.canTargetOpposition)
                validOptions.AddRange(
                    selectingUnit.isPlayerUnit 
                    ? action.targetOptions.Where(x => !x.isPlayerUnit).ToList()
                    : action.targetOptions.Where(x => x.isPlayerUnit).ToList()
                );
            
            if(selectedCombatMove.canTargetSelf)
                validOptions.Add(selectingUnit);

            return new List<Unit> {validOptions[UnityEngine.Random.Range(0, validOptions.Count)]}; 
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