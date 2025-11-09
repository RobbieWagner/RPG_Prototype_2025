using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace RobbieWagnerGames.RPG
{
    public class CombatMoveExecutionCard : MonoBehaviour
    {

        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private TextMeshProUGUI actionNameText;
        [SerializeField] private Image actionIcon;
        private CombatMove executingMove;
        public CombatMove ExecutingMove
        {
            get
            {
                return executingMove;
            }
            set
            {
                if (value == executingMove) return;

                executingMove = value;

                UpdateCombatMoveDetails();
            }
        }

        private void UpdateCombatMoveDetails()
        {
            if (ExecutingMove != null)
            {
                actionNameText.text = ExecutingMove.moveName;
                actionIcon.sprite = ExecutingMove.moveIcon;
            }
            else
            {
                actionNameText.text = "NA";
                actionIcon.sprite = null;
            }
        }

        public IEnumerator DisplayCombatMoveExecutionDetails()
        {
            yield return rectTransform.DOAnchorPos(Vector2.zero, .5f).SetEase(Ease.Linear).WaitForCompletion();
            yield return new WaitForSeconds(1f);
            yield return rectTransform.DOAnchorPos(new Vector2(0, -rectTransform.sizeDelta.y), .5f).SetEase(Ease.Linear).WaitForCompletion();
            yield return null;
        }
    }
}