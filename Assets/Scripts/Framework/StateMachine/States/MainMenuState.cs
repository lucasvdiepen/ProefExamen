using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine.States
{
    public class MainMenuState : State
    {
        private protected override void RegisterState() => StateMachine.Instance.RegisterState(this, true);

        public override IEnumerator OnStateEnter()
        {
            Debug.Log("Main Menu State Entered");
            yield return null;
        }

        public override IEnumerator OnStateExit()
        {
            Debug.Log("Main Menu State Exited");
            yield return null;
        }
    }
}