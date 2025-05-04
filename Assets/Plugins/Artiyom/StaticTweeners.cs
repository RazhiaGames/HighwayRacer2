using System.Threading.Tasks;
using DG.Tweening;
using RTLTMPro;
using UnityEngine;

public static class StaticTweeners
{
    public static async Task DoYoyoScale(Transform item, float endScale = 2, float duration = 0.6f,
        Ease ease = Ease.OutCubic)
    {
        await item.transform.DOScale(endScale, duration).SetLoops(2, LoopType.Yoyo).SetEase(ease)
            .AsyncWaitForCompletion();
    }

    public static void DoFade(RTLTextMeshPro numberElementTransform, float duration = 0.2f, float delay = 0,
        float endVal = 0, Ease ease = Ease.InBack)
    {
        numberElementTransform.DOFade(endVal, duration).SetEase(ease).SetDelay(delay);
    }

    public static async Task AnimateUp(Transform item, float endScale = 1, float duration = 0.6f,
        Ease ease = Ease.OutBack)
    {
        await item.transform.DOScale(endScale, duration).From(Vector3.zero).SetEase(ease).AsyncWaitForCompletion();
    }

    public static async Task AnimateDown(Transform item, float endScale = 0, float duration = 0.6f,
        Ease ease = Ease.InBack)
    {
        await item.transform.DOScale(endScale, duration).From(Vector3.one).SetEase(ease).AsyncWaitForCompletion();
    }



    public static async Task TextScaleChange(Transform container, RTLTextMeshPro text, string newText,
        Color scaledColor, float endScale = 1.6f, float duration = 0.5f, Ease ease = Ease.InSine)
    {
        var originalColor = text.color;
        container.DOScale(new Vector3(endScale, endScale), duration)
            .SetEase(ease);

        text.DOColor(scaledColor, duration)
            .SetEase(ease);

        await Task.Delay((int)(duration * 1000)); // Properly converts duration to milliseconds

        text.text = newText;

        container.DOScale(Vector3.one, duration)
            .SetEase(Ease.OutSine);
    
        text.DOColor(originalColor, duration)
            .SetEase(Ease.OutSine);
    }

}