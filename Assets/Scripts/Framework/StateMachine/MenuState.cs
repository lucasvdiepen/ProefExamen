using ProefExamen.Framework.StateMachine;
using UnityEngine;

public abstract class MenuState : State
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    public override void OnStateEnter()
    {
        throw new System.NotImplementedException();
    }
}
