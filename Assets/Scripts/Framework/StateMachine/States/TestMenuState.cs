using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine.States
{
    public class TestMenuState : State
    {
        public override IEnumerator OnStateEnter()
        {
            Debug.Log("Test Menu State Entered");
            yield return null;
        }

        public override IEnumerator OnStateExit()
        {
            Debug.Log("Test Menu State Exited");
            yield return null;
        }
    }
}