using UnityEngine;
using UnityEngine.EventSystems;

namespace RobbieWagnerGames.RPG
{
    public class MoveButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private CombatMove move;
        private CombatSelectionUI ui;

        public void Initialize(CombatMove move, CombatSelectionUI ui)
        {
            this.move = move;
            this.ui = ui;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ui?.OnMoveButtonHover(move);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui?.OnMoveButtonHoverExit();
        }
    }
}