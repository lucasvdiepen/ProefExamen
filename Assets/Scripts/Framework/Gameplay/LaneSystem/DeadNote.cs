using ProefExamen.Framework.Gameplay.Values;
using System.Collections;
using UnityEngine;

namespace ProefExamen.Framework.Gameplay.LaneSystem
{
    public class DeadNote : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _spriteRenderer;

        private float _lerpAlpha = 1;

        private void Start()
        {
            if(_spriteRenderer == null)
                _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        }

        public void SetDeadNoteValues(Sprite sprite, Transform newTransform)
        {
            _spriteRenderer.sprite = sprite;

            gameObject.transform.position = newTransform.position;
            gameObject.transform.rotation = newTransform.rotation;
            gameObject.transform.localScale = newTransform.localScale;

            StartCoroutine(FadeDeadNote());
        }

        private IEnumerator FadeDeadNote()
        {
            while (_lerpAlpha > 0)
            {
                if (SessionValues.Instance.paused)
                {
                    yield return null;
                    continue;
                }
                _lerpAlpha -= Time.deltaTime / SessionValues.Instance.deadNoteFadeTime;

                _spriteRenderer.color = new Color(1, 1, 1, _lerpAlpha);
                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
