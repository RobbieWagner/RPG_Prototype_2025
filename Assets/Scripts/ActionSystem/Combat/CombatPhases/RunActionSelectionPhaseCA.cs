using System.Collections.Generic;
using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class RunActionSelectionPhaseCA : GameAction
    {
        public Unit unit;

        public RunActionSelectionPhaseCA(Unit currentActingUnit)
        {
            unit = currentActingUnit;
            //Debug.Log($"{unit} is now acting");
        }
    }
}