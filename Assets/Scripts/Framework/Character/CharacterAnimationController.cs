using UnityEngine;

using ProefExamen.Framework.Gameplay.LaneSystem;
using DG.Tweening;

namespace ProefExamen.Framework.Character
{
    /// <summary>
    /// Class responsible for controlling the charater animations.
    /// </summary>
    public class CharacterAnimationController : MonoBehaviour
    {
        private Animator _characterAnim = null;

        private int _leftHitHash = 0;
        private int _rightHitHash = 0;

        private void Awake()
        {
            _characterAnim = GetComponentInChildren<Animator>();

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
            _characterAnim.SetTrigger(hitLeftLane ? _rightHitHash : _leftHitHash);
        }

        private void OnDisable()
        {
            LaneManager.Instance.OnNoteHit -= HandleNoteHit;
        }
    }
}
