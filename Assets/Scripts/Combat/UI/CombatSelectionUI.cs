using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RobbieWagnerGames.RPG
{
    public class CombatSelectionUI : MonoBehaviour
    {
        public static CombatSelectionUI Instance { get; private set; }

        [Header("Main Action Menu")]
        [SerializeField] private GameObject mainActionMenuPanel;
        [SerializeField] private Button movesButton;
        [SerializeField] private Button itemsButton;
        [SerializeField] private Button fleeButton;

        [Header("Move Selection Menu")]
        [SerializeField] private GameObject moveSelectionPanel;
        [SerializeField] private Transform moveButtonContainer;
        [SerializeField] private GameObject moveButtonPrefab;
        [SerializeField] private Button moveSelectionBackButton;
        
        [Header("Move Information Display")]
        [SerializeField] private GameObject moveInfoDisplay;
        [SerializeField] private TextMeshProUGUI moveNameText;
        [SerializeField] private TextMeshProUGUI moveCostText;
        [SerializeField] private TextMeshProUGUI moveDescriptionText;

        [Header("Target Selection Menu")]
        [SerializeField] private GameObject targetSelectionPanel;
        [SerializeField] private Transform targetButtonContainer;
        [SerializeField] private GameObject targetButtonPrefab;
        [SerializeField] private Button targetSelectionBackButton;
        [SerializeField] private TextMeshProUGUI targetSelectionTitle;
        
        [Header("Target Information Display")]
        [SerializeField] private GameObject targetInfoDisplay;
        [SerializeField] private TextMeshProUGUI targetNameText;
        [SerializeField] private TextMeshProUGUI targetHPText;
        [SerializeField] private TextMeshProUGUI targetStatusText;

        private Unit currentSelectingUnit;
        private CombatMove selectedMove;
        private List<CombatMove> availableMoves;
        private List<Unit> availableTargets;
        private CombatMove currentlyHoveredMove;
        private Unit currentlyHoveredTarget;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                InitializeUI();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void InitializeUI()
        {
            // Set up button listeners
            movesButton.onClick.AddListener(OnMovesButtonClicked);
            itemsButton.onClick.AddListener(OnItemsButtonClicked);
            fleeButton.onClick.AddListener(OnFleeButtonClicked);

            moveSelectionBackButton.onClick.AddListener(OnMoveSelectionBackClicked);
            targetSelectionBackButton.onClick.AddListener(OnTargetSelectionBackClicked);

            // Hide all panels initially
            mainActionMenuPanel.SetActive(false);
            moveSelectionPanel.SetActive(false);
            moveInfoDisplay.SetActive(false);
            targetSelectionPanel.SetActive(false);
            targetInfoDisplay.SetActive(false);
        }

        public void StartActionSelection()
        {
            currentSelectingUnit = CombatManager.Instance.CurrentActingUnit;
            if (currentSelectingUnit == null || !currentSelectingUnit.isPlayerUnit)
            {
                Debug.LogError("Cannot start action selection - no valid player unit selected");
                return;
            }

            ShowMainActionMenu();
        }

        private void ShowMainActionMenu()
        {
            // Hide other panels
            moveSelectionPanel.SetActive(false);
            moveInfoDisplay.SetActive(false);
            targetSelectionPanel.SetActive(false);
            targetInfoDisplay.SetActive(false);

            // Show main menu
            mainActionMenuPanel.SetActive(true);

            // Update button interactability based on context
            UpdateMainMenuButtons();
        }

        private void UpdateMainMenuButtons()
        {
            // Check if unit has any available moves
            availableMoves = currentSelectingUnit?.GetAvailableCombatMoves();
            movesButton.interactable = availableMoves != null && availableMoves.Count > 0;

            // For now, items and flee are always available (implementation pending)
            itemsButton.interactable = true;
            fleeButton.interactable = true;
        }

        private void OnMovesButtonClicked()
        {
            ShowMoveSelectionMenu();
        }

        private void OnItemsButtonClicked()
        {
            // Placeholder for item selection
            Debug.Log("Items button clicked - item system not implemented");
        }

        private void OnFleeButtonClicked()
        {
            // Placeholder for flee action
            Debug.Log("Flee button clicked - flee system not implemented");
        }

        private void OnBackButtonClicked()
        {
            // Currently no back option from main menu in standard flow
            Debug.Log("Back button clicked from main menu");
        }

        private void ShowMoveSelectionMenu()
        {
            mainActionMenuPanel.SetActive(false);
            moveSelectionPanel.SetActive(true);
            moveInfoDisplay.SetActive(true);

            // Clear existing move buttons
            foreach (Transform child in moveButtonContainer)
                Destroy(child.gameObject);

            // Reset hover state
            currentlyHoveredMove = null;
            ClearMoveInfoDisplay();

            // Create buttons for each available move
            foreach (var move in availableMoves)
            {
                GameObject moveButtonObj = Instantiate(moveButtonPrefab, moveButtonContainer);
                Button moveButton = moveButtonObj.GetComponent<Button>();
                TextMeshProUGUI moveText = moveButtonObj.GetComponentInChildren<TextMeshProUGUI>();

                // Set only the move name on the button
                moveText.text = move.moveName;

                // Add click listener
                moveButton.onClick.AddListener(() => OnMoveSelected(move));

                // Add hover listeners
                MoveButtonHoverHandler hoverHandler = moveButtonObj.GetComponent<MoveButtonHoverHandler>();
                if (hoverHandler == null)
                    hoverHandler = moveButtonObj.AddComponent<MoveButtonHoverHandler>();
                hoverHandler.Initialize(move, this);

                // Disable button if unit doesn't have enough stamina
                moveButton.interactable = currentSelectingUnit.RuntimeStats[ComputedStatType.STAMINA] >= move.moveCost;

                // Visual feedback for disabled moves
                if (!moveButton.interactable)
                    moveText.color = Color.gray;
            }
        }

        public void OnMoveButtonHover(CombatMove move)
        {
            currentlyHoveredMove = move;
            UpdateMoveInfoDisplay(move);
        }

        public void OnMoveButtonHoverExit()
        {
            // Only clear if we're not hovering over a different move
            if (currentlyHoveredMove != null)
            {
                currentlyHoveredMove = null;
                ClearMoveInfoDisplay();
            }
        }

        private void UpdateMoveInfoDisplay(CombatMove move)
        {
            if (move == null) return;

            moveNameText.text = move.moveName;
            moveCostText.text = $"Cost: {move.moveCost} Stamina";
            moveDescriptionText.text = string.IsNullOrEmpty(move.description) ? "No description available." : move.description;
            
            moveInfoDisplay.SetActive(true);
        }

        private void ClearMoveInfoDisplay()
        {
            moveNameText.text = "Select a Move";
            moveCostText.text = "Cost: -";
            moveDescriptionText.text = "Hover over a move to see details.";
        }

        private void OnMoveSelected(CombatMove move)
        {
            selectedMove = move;
            currentSelectingUnit.selectedCombatMove = move;
            ShowTargetSelectionMenu(move);
        }

        private void OnMoveSelectionBackClicked()
        {
            moveSelectionPanel.SetActive(false);
            moveInfoDisplay.SetActive(false);
            ShowMainActionMenu();
        }

        private void ShowTargetSelectionMenu(CombatMove move)
        {
            moveSelectionPanel.SetActive(false);
            moveInfoDisplay.SetActive(false);
            targetSelectionPanel.SetActive(true);
            targetInfoDisplay.SetActive(true);

            // Set selection title
            targetSelectionTitle.text = $"Select target for {move.moveName}";

            // Clear existing target buttons
            foreach (Transform child in targetButtonContainer)
            {
                Destroy(child.gameObject);
            }

            // Reset hover state
            currentlyHoveredTarget = null;
            ClearTargetInfoDisplay();

            // Get valid targets based on move properties
            availableTargets = TargetSelectionUtility.GetValidTargetsForMove(currentSelectingUnit, move);

            // Create buttons for each valid target
            foreach (var target in availableTargets)
            {
                GameObject targetButtonObj = Instantiate(targetButtonPrefab, targetButtonContainer);
                Button targetButton = targetButtonObj.GetComponent<Button>();
                TextMeshProUGUI targetText = targetButtonObj.GetComponentInChildren<TextMeshProUGUI>();

                // Set only the target name on the button
                targetText.text = target.UnitData.unitName;

                // Add click listener
                targetButton.onClick.AddListener(() => OnTargetSelected(target));

                // Add hover listeners
                TargetButtonHoverHandler hoverHandler = targetButtonObj.GetComponent<TargetButtonHoverHandler>();
                if (hoverHandler == null)
                {
                    hoverHandler = targetButtonObj.AddComponent<TargetButtonHoverHandler>();
                }
                hoverHandler.Initialize(target, this);

                // Visual indication for dead units
                if (target.RuntimeStats.ContainsKey(ComputedStatType.HP) && target.RuntimeStats[ComputedStatType.HP] <= 0)
                {
                    targetButton.interactable = false;
                    targetText.color = Color.gray;
                }
            }

            // Handle moves that target all units of a type
            if (move.targetsAllUnits || move.targetsAllAllies || move.targetsAllOpposition)
            {
                CreateSelectAllButton(move);
            }
        }

        public void OnTargetButtonHover(Unit target)
        {
            currentlyHoveredTarget = target;
            UpdateTargetInfoDisplay(target);
        }

        public void OnTargetButtonHoverExit()
        {
            // Only clear if we're not hovering over a different target
            if (currentlyHoveredTarget != null)
            {
                currentlyHoveredTarget = null;
                ClearTargetInfoDisplay();
            }
        }

        private void UpdateTargetInfoDisplay(Unit target)
        {
            if (target == null) return;

            targetNameText.text = target.UnitData.unitName;
            
            // Display HP
            if (target.RuntimeStats.ContainsKey(ComputedStatType.HP))
            {
                int currentHP = target.RuntimeStats[ComputedStatType.HP];
                int maxHP = target.GetComputedStatDefaultValue(ComputedStatType.HP);
                targetHPText.text = $"HP: {currentHP}/{maxHP}";
                
                // Color code based on HP percentage
                float hpPercent = (float)currentHP / maxHP;
                targetHPText.color = hpPercent > 0.5f ? Color.green : 
                                   hpPercent > 0.25f ? Color.yellow : Color.red;
            }
            else
            {
                targetHPText.text = "HP: N/A";
            }

            // Display status/stats (you can expand this later)
            targetStatusText.text = GetTargetStatusDescription(target);
            
            targetInfoDisplay.SetActive(true);
        }

        private string GetTargetStatusDescription(Unit target)
        {
            List<string> statuses = new List<string>();
            
            // Check for low stamina
            if (target.RuntimeStats.ContainsKey(ComputedStatType.STAMINA) && 
                target.RuntimeStats[ComputedStatType.STAMINA] <= 0)
            {
                statuses.Add("Exhausted");
            }
            
            // Check for low HP
            if (target.RuntimeStats.ContainsKey(ComputedStatType.HP))
            {
                int currentHP = target.RuntimeStats[ComputedStatType.HP];
                int maxHP = target.GetComputedStatDefaultValue(ComputedStatType.HP);
                if (currentHP <= 0)
                {
                    statuses.Add("Defeated");
                }
                else if ((float)currentHP / maxHP < 0.25f)
                {
                    statuses.Add("Critical");
                }
            }
            
            return statuses.Count > 0 ? string.Join(", ", statuses) : "Normal";
        }

        private void ClearTargetInfoDisplay()
        {
            targetNameText.text = "Select a Target";
            targetHPText.text = "HP: -";
            targetHPText.color = Color.white;
            targetStatusText.text = "Hover over a target to see details.";
        }

        private void CreateSelectAllButton(CombatMove move)
        {
            GameObject allButtonObj = Instantiate(targetButtonPrefab, targetButtonContainer);
            Button allButton = allButtonObj.GetComponent<Button>();
            TextMeshProUGUI allButtonText = allButtonObj.GetComponentInChildren<TextMeshProUGUI>();

            allButtonText.text = "All Valid Targets";
            allButton.onClick.AddListener(() => OnAllTargetsSelected());
        }

        private void OnTargetSelected(Unit target)
        {
            currentSelectingUnit.selectedTargets = new List<Unit> { target };
            CompleteActionSelection();
        }

        private void OnAllTargetsSelected()
        {
            // For moves that target multiple units, select all valid targets
            currentSelectingUnit.selectedTargets = availableTargets
                .Where(target => target.RuntimeStats[ComputedStatType.HP] > 0) // Only alive units
                .ToList();
            
            CompleteActionSelection();
        }

        private void OnTargetSelectionBackClicked()
        {
            targetSelectionPanel.SetActive(false);
            targetInfoDisplay.SetActive(false);
            selectedMove = null;
            currentSelectingUnit.selectedCombatMove = null;
            ShowMoveSelectionMenu();
        }

        private void CompleteActionSelection()
        {
            // Clear UI state
            mainActionMenuPanel.SetActive(false);
            moveSelectionPanel.SetActive(false);
            moveInfoDisplay.SetActive(false);
            targetSelectionPanel.SetActive(false);
            targetInfoDisplay.SetActive(false);

            // Clear current selection state
            currentSelectingUnit = null;
            selectedMove = null;
            availableMoves?.Clear();
            availableTargets?.Clear();
            currentlyHoveredMove = null;
            currentlyHoveredTarget = null;

            Debug.Log("Action selection completed");
        }

        public void CancelSelection()
        {
            if (currentSelectingUnit != null)
            {
                currentSelectingUnit.selectedCombatMove = null;
                currentSelectingUnit.selectedTargets = null;
            }
            CompleteActionSelection();
        }

        private void OnDestroy()
        {
            // Clean up button listeners to prevent memory leaks
            movesButton.onClick.RemoveAllListeners();
            itemsButton.onClick.RemoveAllListeners();
            fleeButton.onClick.RemoveAllListeners();
            moveSelectionBackButton.onClick.RemoveAllListeners();
            targetSelectionBackButton.onClick.RemoveAllListeners();
        }
    }
}