using System;
using System.Collections.Generic;
using RobbieWagnerGames.RPG;

namespace RobbieWagnerGames.Utilities.SaveData
{
    [Serializable]
    public class GameSaveData
    {
        public string savePlayerName = "Player";

        public List<float> saveColorRGB = new List<float>() {.5f, .5f, .5f};

        public RunDetails currentRunDetails = null;
        public List<RunDetails> runHistory = new List<RunDetails>();
        public UnitData mainPlayerUnit = null;
    }
}