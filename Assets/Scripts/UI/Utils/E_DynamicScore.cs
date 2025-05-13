using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class E_DynamicScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public Image haloImage;
    private Vector3 initialPos;
    private Color initialTextColor;
    private Color initialHaloColor;

    public void Initialize()
    {
        initialTextColor = scoreText.color;
        initialHaloColor = haloImage.color;
        initialPos = transform.position;
        scoreText.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, 0);
        haloImage.color = new Color(initialHaloColor.r, initialHaloColor.g, initialHaloColor.b, 0);
    }

    public void DoJuiceAnimation()
    {
        scoreText.color = initialTextColor;
        haloImage.color = initialHaloColor;
        
        transform.DOMoveY(initialPos.y + 100f, 1.5f).SetEase(Ease.InCubic).OnComplete(MoveBack);
        haloImage.DOFade(0, 1.5f).SetDelay(0.5f).SetId("haloTween");
        scoreText.DOFade(0, 1.5f).SetDelay(0.5f).SetId("scoreTween");
    }

    private void MoveBack()
    {
        DOTween.Kill("haloTween");
        DOTween.Kill("scoreTween");
        scoreText.color = new Color(initialTextColor.r, initialTextColor.g, initialTextColor.b, 0);
        haloImage.color = new Color(initialHaloColor.r, initialHaloColor.g, initialHaloColor.b, 0);
        transform.position = initialPos;
    }
}