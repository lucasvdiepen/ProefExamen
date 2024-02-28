using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine.States
{
    public class MainMenuState : State
    {
        private protected override void RegisterState() => StateMachine.RegisterState(this, true);

        public override void OnStateEnter()
        {
            Debug.Log("Main Menu State Entered");
        }

        public override void OnStateExit()
        {
            Debug.Log("Main Menu State Exited");
        }
    }
}