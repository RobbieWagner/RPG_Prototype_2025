using System;
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

        private RunDetails _runDetails => RunManager.Instance.RunDetails;

        private System.Action setupCompletionCallback = null;

        protected override void Awake()
        {
            base.Awake();
        }

        public void HandleNewRun(System.Action callback)
        {
            setupCompletionCallback = callback;
            SetupPartyMenu();
        }

        private void SetupPartyMenu()
        {
            unitOptions = _runDetails.unitOptions;
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

            _runDetails.AddUnitToParty(selectedSprite.unitData);
            unitsSelected++;

            unitOptions.Remove(selectedSprite.unitData);
            unitSelections.Remove(selectedSprite);
            Destroy(selectedSprite.gameObject);

            CheckSelectionComplete();
        }

        private void CheckSelectionComplete()
        {
            if (_runDetails.PlayerParty.Count >= 3)
                OnUnitSelectionComplete();
            else Debug.Log(_runDetails.PlayerParty.Count);
        }

        private void OnUnitSelectionComplete()
        {
            canvas.enabled = false;
            CheckForRunStartCompletion();
        }

        private void CheckForRunStartCompletion()
        {
            if(IsPartySetupComplete()) // Add Other conditions as necessary
            {
                if(setupCompletionCallback != null)
                    setupCompletionCallback.Invoke();
                else
                    throw new NullReferenceException("New Run Setup Callback not set. Please ensure the callback is set before trying to setup a new run");
            }
        }

        private bool IsPartySetupComplete()
        {
            return _runDetails.PlayerParty.Count > 0;
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