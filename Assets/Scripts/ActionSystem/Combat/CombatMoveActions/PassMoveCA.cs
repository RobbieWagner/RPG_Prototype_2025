namespace RobbieWagnerGames.RPG
{
    public class PassMoveCA : GameAction
    {
        public Unit user;
        public Pass passEffect;
        public override ActionScope Scope => ActionScope.EXECUTION_PHASE;

        public PassMoveCA(Unit user)
        {
            this.user = user;
        }
    }
}