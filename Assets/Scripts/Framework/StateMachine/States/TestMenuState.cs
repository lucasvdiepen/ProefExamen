using System.Collections;
using UnityEngine;

namespace ProefExamen.Framework.StateMachine.States
{
    public class TestMenuState : State
    {
        public override IEnumerator OnStateEnter()
        {
            yield return base.OnStateEnter();

            Debug.Log("Test Menu State Entered");
            yield return null;
        }

        public override IEnumerator OnStateExit()
        {
            yield return base.OnStateExit();

            Debug.Log("Test Menu State Exited");
            yield return null;
        }
    }
}