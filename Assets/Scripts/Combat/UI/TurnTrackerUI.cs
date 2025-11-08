using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace RobbieWagnerGames.RPG
{
    public class TurnTrackerUI : MonoBehaviour
    {
        [SerializeField] private Image turnTrackerBG;
        [SerializeField] private TextMeshProUGUI turnText;
        [SerializeField] private Color playerTurnColor = Color.blue;
        [SerializeField] private Color enemyTurnColor = Color.red;

        public void UpdateTurnTrackerUI(int turn, bool playerTurn = true)
        {
            turnText.text = $"{turn}";

            turnTrackerBG.color = playerTurn ? playerTurnColor : enemyTurnColor;
        }
    }
}