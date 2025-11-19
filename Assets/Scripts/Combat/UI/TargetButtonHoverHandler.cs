using UnityEngine;
using UnityEngine.EventSystems;

namespace RobbieWagnerGames.RPG
{
    public class TargetButtonHoverHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private Unit target;
        private CombatSelectionUI ui;

        public void Initialize(Unit target, CombatSelectionUI ui)
        {
            this.target = target;
            this.ui = ui;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            ui?.OnTargetButtonHover(target);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ui?.OnTargetButtonHoverExit();
        }
    }
}