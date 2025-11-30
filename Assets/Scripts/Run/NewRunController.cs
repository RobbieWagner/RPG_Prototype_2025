using System.Collections.Generic;
using RobbieWagnerGames.Utilities;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RobbieWagnerGames.RPG
{
    public class NewRunController : MonoBehaviourSingleton<NewRunController>
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private GridLayoutGroup unitSelectionGrid;
        [SerializeField] private UnitSelectionSprite unitSelectionSpritePrefab;
        private List<UnitSelectionSprite> unitSelections = new List<UnitSelectionSprite>();

        private List<UnitData> unitOptions = new List<UnitData>();
        private int unitsSelected = 0;

        protected override void Awake()
        {
            base.Awake();
            StartUnitSelection();
        }

        public void StartUnitSelection()
        {
            unitOptions = RunManager.Instance.RunDetails.unitOptions;
            unitsSelected = 0;

            // Clear any existing selection sprites
            foreach (Transform child in unitSelectionGrid.transform)
            {
                Destroy(child.gameObject);
            }
            unitSelections.Clear();

            foreach (UnitData unitOption in unitOptions)
            {
                UnitSelectionSprite selectionSprite = Instantiate(unitSelectionSpritePrefab, unitSelectionGrid.transform);
                selectionSprite.spriteRenderer.sprite = unitOption.unitIcon;
                selectionSprite.unitNameText.text = unitOption.unitName;
                selectionSprite.unitData = unitOption;

                // Add listener for unit selection
                selectionSprite.button.onClick.AddListener(() => SelectUnit(selectionSprite));
                
                unitSelections.Add(selectionSprite);
            }
        }

        private void SelectUnit(UnitSelectionSprite selectedSprite)
        {
            if (selectedSprite == null) return;

            // Add unit to player party
            RunManager.Instance.RunDetails.AddUnitToParty(selectedSprite.unitData);
            unitsSelected++;

            // Remove from available options and destroy the sprite
            unitOptions.Remove(selectedSprite.unitData);
            unitSelections.Remove(selectedSprite);
            Destroy(selectedSprite.gameObject);

            // Check if selection is complete
            CheckSelectionComplete();
        }

        private void CheckSelectionComplete()
        {
            if (unitsSelected >= 2)
                OnUnitSelectionComplete();
        }

        private void OnUnitSelectionComplete()
        {
            // Hide the selection UI
            canvas.enabled = false;

            // Start the actual run
            RunManager.Instance.StartRunAfterUnitSelection();
        }

        public void CancelUnitSelection()
        {
            // Clean up and return to main menu or previous screen
            foreach (UnitSelectionSprite selection in unitSelections)
            {
                if (selection != null)
                    Destroy(selection.gameObject);
            }
            unitSelections.Clear();
            unitOptions.Clear();
        }
    }
}