using System;
using System.Collections;
using System.Collections.Generic;
using RobbieWagnerGames.UI;
using RobbieWagnerGames.Utilities;
using RobbieWagnerGames.Utilities.SaveData;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RobbieWagnerGames.RPG
{
    public enum GameState
    {
        NONE = -1,
        EXPLORATION,
        COMBAT,
        DIALOGUE,
        MENU,
        PAUSE
    }

    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        private GameState currentState = GameState.NONE;
        public GameState CurrentState => currentState;
        private GameSaveData gameSaveData;
        public GameSaveData SaveData => gameSaveData;

        public string newGameSceneName = "NewGameScene";
        public List<UnitData> defaultPlayerUnitOptions;
        public UnitData defaultMainPlayerUnit;

        //public ExplorationDetails newGameExplorationDetails;

        protected override void Awake()
        {
            base.Awake();

            // For now, just start game automatically
            // TODO: handle this through a main menu
            StartCoroutine(StartGame());
        }

        public IEnumerator StartGame(bool newGame = false)
        {
            yield return null; 

            gameSaveData = LoadGame();

            if(gameSaveData == null || newGame)
                StartCoroutine(SceneLoadManager.Instance.LoadSceneAdditive(newGameSceneName));
            else
                RunManager.Instance.PrepRunScene(gameSaveData.currentRunDetails);
                       
        }

        private GameSaveData LoadGame()
        {
            return JsonDataService.Instance.LoadDataRelative<GameSaveData>(StaticGameStats.playerSaveDataFilePath, null);
        }

        public void SaveGame()
        {
            JsonDataService.Instance.SaveData(StaticGameStats.playerSaveDataFilePath, gameSaveData);
        }
    }
}