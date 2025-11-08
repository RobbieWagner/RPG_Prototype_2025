using System.Collections.Generic;
using RobbieWagnerGames.Utilities;
using UnityEngine;


namespace RobbieWagnerGames.RPG
{
    public class CombatHUD : MonoBehaviourSingleton<CombatHUD>
    {
        [SerializeField] private TurnTrackerUI turnTrackerPrefab;
        [SerializeField] private UnitBattleInfoCard unitBattleInfoCardPrefab;
        [SerializeField] private Transform topBar;

        private List<GameObject> topBarObjects = new List<GameObject>();
        private List<UnitBattleInfoCard> allyUIInfo = new List<UnitBattleInfoCard>();
        private List<UnitBattleInfoCard> enemyUIInfo = new List<UnitBattleInfoCard>();
        [HideInInspector] public TurnTrackerUI turnTrackerUIInstance;

        public void InitializeHUD(List<Unit> allies, List<Unit> enemies)
        {
            foreach (GameObject o in topBarObjects)
                Destroy(o);
            topBarObjects.Clear();

            allyUIInfo.Clear();
            enemyUIInfo.Clear();

            foreach (Unit ally in allies)
            {
                UnitBattleInfoCard newCard = Instantiate(unitBattleInfoCardPrefab, topBar);
                newCard.Unit = ally;
                allyUIInfo.Add(newCard);
                topBarObjects.Add(newCard.gameObject);
            }

            turnTrackerUIInstance = Instantiate(turnTrackerPrefab, topBar);
            turnTrackerUIInstance.UpdateTurnTrackerUI(1, true);

            foreach (Unit enemy in enemies)
            {
                UnitBattleInfoCard newCard = Instantiate(unitBattleInfoCardPrefab, topBar);
                newCard.Unit = enemy;
                enemyUIInfo.Add(newCard);
                topBarObjects.Add(newCard.gameObject);
            }
        }
    }
}