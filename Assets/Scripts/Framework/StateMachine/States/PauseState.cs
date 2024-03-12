using ProefExamen.Framework.StateMachine.Attributes;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// State for pausing.
    /// </summary>
    [ParentState(typeof(GameState))]
    public class PauseState : MenuState
    {

    }
}