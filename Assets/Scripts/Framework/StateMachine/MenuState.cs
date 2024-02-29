using ProefExamen.Framework.StateMachine;
using System.Collections;
using UnityEngine;

public abstract class MenuState : State
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    public override IEnumerator OnStateEnter()
    {
        throw new System.NotImplementedException();
    }
}
