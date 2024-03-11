using System.Collections;

using ProefExamen.Framework.Gameplay.LaneSystem;

namespace ProefExamen.Framework.StateMachine.States
{
    public class GameState : MenuState
    {
        public override IEnumerator OnStateEnter()
        {
            yield return base.OnStateEnter();

            LaneManager.Instance.PlayLevel();
        }
    }
}