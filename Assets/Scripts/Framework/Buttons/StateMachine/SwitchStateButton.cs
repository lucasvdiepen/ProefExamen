using UnityEngine;

using ProefExamen.Framework.Buttons;
using ProefExamen.Framework.StateMachine;

public class SwitchStateButton : BasicButton
{
    [SerializeField]
    private State _targetState;

    private protected override void OnButtonPressed() => StateMachine.Instance.GoToState(_targetState);
}
