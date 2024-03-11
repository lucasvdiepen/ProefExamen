using ProefExamen.Framework.StateMachine.Attributes;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// State for losing.
    /// </summary>
    [ParentState(typeof(GameState))]
    public class LoseState : MenuState
    {

    }
}