using System;
using System.Collections;
using RobbieWagnerGames.UI;
using RobbieWagnerGames.Utilities;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public enum GameState
    {
        NONE = -1,
        EXPLORATION,
        COMBAT,
        DIALOGUE,
        PAUSE
    }

    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        private GameState currentState = GameState.NONE;
        public GameState CurrentState => currentState;

        public ExplorationDetails newGameExplorationDetails;

        protected override void Awake()
        {
            base.Awake();

            // For now, just start game automatically
            // TODO: handle this through a main menu
            StartCoroutine(StartGame());
        }

        public IEnumerator StartGame()
        {
            // Load the players save file
            // Load the exploration details from the players save file
            yield return null;

            ExplorationDetails gameSaveDetails = LoadPlayerSaveFile();

            ExplorationManager.Instance.StartExploration(gameSaveDetails ?? newGameExplorationDetails);
        }

        private ExplorationDetails LoadPlayerSaveFile()
        {
            // TODO: IMPLEMENT!
            return null;
        }
    }
}