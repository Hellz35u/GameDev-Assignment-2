using UnityEngine;
using TMPro;
using DG.Tweening;

public class WaveTextAnimator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;

    [SerializeField] private float scaleDurationInSeconds = 0.2f;
    [SerializeField] private float holdDurationInSeconds = 0.2f;
    [SerializeField] private float fadeOutDurationInSeconds = 0.2f;

    private Tween tween;
    private Vector3 originalScale;
    private Color originalColor;

    private void Start()
    {
        originalScale = transform.localScale;
        originalColor = waveText.color;
    }

    public void DisplayWaveText(string text)
    {
        waveText.text = text;
        TextAnimation();
    }

    private void TextAnimation()
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
        }

        waveText.enabled = true;
        waveText.color = originalColor;
        
        transform.localScale = Vector3.zero;
        Color transparent = originalColor;
        transparent.a = 0f;

        Sequence sequence = DOTween.Sequence();

        //from zero size to original scale
        sequence.Append(transform
        .DOScale(originalScale, scaleDurationInSeconds)
        .SetEase(Ease.InOutBounce));

        //wait some time to show the text
        sequence.AppendInterval(holdDurationInSeconds);

        //the text is fading out
        sequence.Append(waveText.DOColor(transparent, fadeOutDurationInSeconds));

        //at the end after the fading out the visablity is set to false
        sequence.OnComplete(() => waveText.enabled = false);

        tween = sequence;
    }
}