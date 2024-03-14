using UnityEngine;
using System.Collections;

using ProefExamen.Framework.StateMachine.Attributes;
using UnityEngine.UI;
using ProefExamen.Framework.Character;

namespace ProefExamen.Framework.StateMachine.States
{
    /// <summary>
    /// State for losing.
    /// </summary>
    [ParentState(typeof(GameState))]
    public class LoseState : MenuState
    {
        [SerializeField, Space]
        private RawImage gameStateView;

        [SerializeField]
        private RawImage loseStateView;

        public override IEnumerator OnStateEnter()
        {
            yield return base.OnStateEnter();

            gameStateView.gameObject.SetActive(false);
            loseStateView.gameObject.SetActive(true);

            CharacterAnimationController.Instance.CharacterAnim.SetBool("Lose", true);
        }

        public override IEnumerator OnStateExit()
        {
            yield return base.OnStateExit();

            loseStateView.gameObject.SetActive(false);
            gameStateView.gameObject.SetActive(true);

            CharacterAnimationController.Instance.CharacterAnim.SetBool("Lose", false);
        }
    }
}