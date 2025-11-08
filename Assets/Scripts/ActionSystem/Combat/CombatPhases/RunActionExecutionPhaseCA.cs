using UnityEngine;

namespace RobbieWagnerGames.RPG
{
    public class RunActionExecutionPhaseCA : GameAction
    {
        public Unit unit = null;

        public RunActionExecutionPhaseCA(Unit currentActingUnit)
        {
            unit = currentActingUnit;
        }
    }
}