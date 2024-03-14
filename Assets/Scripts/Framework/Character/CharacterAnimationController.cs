using UnityEngine;

using ProefExamen.Framework.Gameplay.LaneSystem;
using ProefExamen.Framework.Utils;

namespace ProefExamen.Framework.Character
{
    /// <summary>
    /// Class responsible for controlling the charater animations.
    /// </summary>
    public class CharacterAnimationController : AbstractSingleton<CharacterAnimationController>
    {
        /// <summary>
        /// Public getter for accesing and controlling animations.
        /// </summary>
        public Animator CharacterAnim { get; private set; }

        private int _leftHitHash = 0;
        private int _rightHitHash = 0;

        private void Awake()
        {
            CharacterAnim = GetComponentInChildren<Animator>();

            _leftHitHash = Animator.StringToHash("HitLeft");
            _rightHitHash = Animator.StringToHash("HitRight");
        }

        private void OnEnable() => LaneManager.Instance.OnNoteHit += HandleNoteHit;

        private void HandleNoteHit(HitStatus status, int laneID)
        {
            if (status == HitStatus.Miss)
                return;

            bool hitLeftLane = laneID == 0 || laneID == 1;
            CharacterAnim.SetTrigger(hitLeftLane ? _rightHitHash : _leftHitHash);
        }

        private void OnDisable() => LaneManager.Instance.OnNoteHit -= HandleNoteHit;
    }
}
