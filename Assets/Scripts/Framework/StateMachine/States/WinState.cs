using ProefExamen.Framework.StateMachine.Attributes;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// State for winning.
    /// </summary>
    [ParentState(typeof(GameState))]
    public class WinState : MenuState
    {

    }
}