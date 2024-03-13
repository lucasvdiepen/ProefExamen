using System.Collections;
using UnityEngine;

using ProefExamen.Framework.Gameplay.Values;

namespace ProefExamen.Framework.Gameplay.LaneSystem
{
    /// <summary>
    /// A class responsible for displaying a hit note that slowly fades.
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class DeadNote : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer _deadSprite;

        [SerializeField]
        private SpriteRenderer _hitSprite;

        private float _lerpAlpha = 1;

        private void Awake()
        {
            if(_deadSprite == null)
                _deadSprite = gameObject.GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Sets the DeadNote's values.
        /// </summary>
        /// <param name="sprite">The sprite to display.</param>
        /// <param name="newTransform">The transform of the DeadNote.</param>
        public void SetDeadNoteValues(Sprite sprite, Sprite hitSprite, Transform newTransform)
        {
            if (sprite != null)
                _deadSprite.sprite = sprite;

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

                Color fadingColor = new Color(1, 1, 1, _lerpAlpha);

                _deadSprite.color = fadingColor;
                _hitSprite.color = fadingColor;

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
