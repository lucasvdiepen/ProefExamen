using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine.States
{
    public class TestMenuState2 : State
    {
        public override void OnStateEnter()
        {
            Debug.Log("Test Menu State 2 Entered");
        }

        public override void OnStateExit()
        {
            Debug.Log("Test Menu State 2 Exited");
        }
    }
}