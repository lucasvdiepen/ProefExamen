using UnityEngine;

using ProefExamen.Framework.Gameplay.LaneSystem;

namespace ProefExamen.Framework.Character
{
    /// <summary>
    /// Class responsible for controlling the charater animations.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimationController : MonoBehaviour
    {
        private Animator _characterAnim = null;

        private int _leftHitHash = 0;
        private int _rightHitHash = 0;

        private void Awake()
        {
            _characterAnim = GetComponent<Animator>();

            _leftHitHash = Animator.StringToHash("HitLeft");
            _rightHitHash = Animator.StringToHash("HitRight");
        }

        private void OnEnable()
        {
            LaneManager.Instance.OnNoteHit += HandleNoteHit;
        }

        private void HandleNoteHit(HitStatus status, int laneID)
        {
            if (status == HitStatus.Miss)
                return;

            bool hitLeftLane = laneID == 0 || laneID == 1;
            _characterAnim.SetTrigger(hitLeftLane ? _leftHitHash : _rightHitHash);
        }

        private void OnDisable()
        {
            LaneManager.Instance.OnNoteHit -= HandleNoteHit;
        }
    }
}
