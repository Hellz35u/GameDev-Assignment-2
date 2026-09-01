using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;
using System;
public class WaveTextAnimator : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI waveText;
    [SerializeField] private float duration = 0.2f;
    //[SerializeField] private WaveManager waveManager;
    
    private Tween tween;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        //waveManager.OnWaveChanged += UpdateWaveText;
    }

    private void UpdateWaveText(int waveNumber)
    {
        waveText.text = $"WAVE {waveNumber}";
        StartCoroutine(PopingTime(WaveAnimation, waveNumber));
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
}
