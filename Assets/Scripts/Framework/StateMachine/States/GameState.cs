using System.Collections;

using ProefExamen.Framework.Gameplay.LaneSystem;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// State for game.
    /// </summary>
    public class GameState : MenuState
    {
        public override IEnumerator OnStateEnter()
        {
            yield return base.OnStateEnter();

            LaneManager.Instance.PlayLevel();
        }
    }
}