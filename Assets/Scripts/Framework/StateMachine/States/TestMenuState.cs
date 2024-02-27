using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine.States
{
    public class TestMenuState : State
    {
        public override void OnStateEnter()
        {
            Debug.Log("Test Menu State Entered");
        }

        public override void OnStateExit()
        {
            Debug.Log("Test Menu State Exited");
        }
    }
}