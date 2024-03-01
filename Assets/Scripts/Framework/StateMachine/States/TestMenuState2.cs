using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine.States
{
    public class TestMenuState2 : MenuState
    {
        public override IEnumerator OnStateEnter()
        {
            yield return base.OnStateEnter();

            Debug.Log("Test Menu State 2 Entered");
            yield return null;
        }

        public override IEnumerator OnStateExit()
        {
            yield return base.OnStateExit();

            Debug.Log("Test Menu State 2 Exited");
            yield return null;
        }
    }
}