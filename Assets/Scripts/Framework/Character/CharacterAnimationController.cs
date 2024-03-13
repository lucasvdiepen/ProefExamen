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

        [SerializeField]
        private float _rotateAmount = 10;

        [SerializeField]
        private float _rotateDuration = .35f;

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
            _characterAnim.SetTrigger(hitLeftLane ? _leftHitHash : _rightHitHash);

            // Rotate character body based on hit laneID.
            if (laneID == 0) transform.DORotate(new Vector3(0, -_rotateAmount, 0), _rotateDuration);
            else if (laneID == 3) transform.DORotate(new Vector3(0, _rotateAmount, 0), _rotateDuration);
            else transform.DORotate(Vector3.zero, _rotateDuration);
        }

        private void OnDisable()
        {
            LaneManager.Instance.OnNoteHit -= HandleNoteHit;
        }
    }
}
