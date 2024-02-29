using ProefExamen.Framework.StateMachine;
using System.Collections;
using UnityEngine;

public abstract class MenuState : State
{
    [SerializeField]
    private CanvasGroup _canvasGroup;

    [SerializeField]
    private float _fadeDuration = 0.5f;

    public override IEnumerator OnStateEnter() => FadeIn();

    public override IEnumerator OnStateExit() => FadeOut();

    private protected virtual IEnumerator FadeIn() => ExecuteFadeEffect(0, 1);

    private protected virtual IEnumerator FadeOut() => ExecuteFadeEffect(1, 0);

    private IEnumerator ExecuteFadeEffect(float fromAlpha, float targetAlpha)
    {
        float elapsedTime = 0;
        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(fromAlpha, targetAlpha, elapsedTime / _fadeDuration);
            yield return null;
        }
    }
}
