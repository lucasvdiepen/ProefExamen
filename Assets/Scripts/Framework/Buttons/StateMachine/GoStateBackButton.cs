using ProefExamen.Framework.Buttons;
using ProefExamen.Framework.StateMachine;

public class GoStateBackButton : BasicButton
{
    private protected override void OnButtonPressed() => StateMachine.Instance.GoBack();
}
