using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using System;
public class WaveTextAnimator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveText;
    private Tween tween;
    private Vector3 originalScale;
    [SerializeField] private float duration = 0.2f;
    
    

    void Start()
    {
        originalScale = transform.localScale;
        StartCoroutine(PopingTime(WaveAnimation, 1));
    }

    private void WaveAnimation()
    {
        if (tween != null || tween.IsActive())
        {
            tween.Kill();
        }
        transform.localScale = Vector3.zero;
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(originalScale, duration).SetEase(Ease.InOutBounce));

        tween = sequence;
    }
    private IEnumerator PopingTime(Action waveAnimation, int waitingTime)
    {
        waveText.enabled = false;
        yield return new WaitForSeconds(waitingTime);
        waveText.enabled = true;
        waveAnimation();
        yield return new WaitForSeconds(waitingTime);
        waveText.enabled = false;
    }
    public void TextEditor(string text)
    {
        waveText.text = text;
    }
}
