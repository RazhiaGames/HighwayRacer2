using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonElement : MonoBehaviour
{
    [FoldoutGroup("ButtonElement")] public UnityEvent onClick;

    [FoldoutGroup("ButtonElement")] [SerializeField]
    protected Button button;

    [FoldoutGroup("ButtonElement")] public Animator animator;
    [FoldoutGroup("ButtonElement")] public List<Image> images = new List<Image>();
    [FoldoutGroup("ButtonElement")] public Color pressedColor = new Color(0.7f, 0.7f, 0.7f, 1.000f);
    [FoldoutGroup("ButtonElement")] public Color selectedColor = Color.green;


    private List<Color> defaultColors = new List<Color>();

    private void Awake()
    {
        if (images.Count > 0)
        {
            for (int i = 0; i < images.Count; i++)
            {
                defaultColors.Add(images[i].color);
            }
        }
    }

    protected virtual void OnEnable()
    {
        button.onClick.AddListener(InvokeClick);
    }


    protected virtual void OnDisable()
    {
        button.onClick.RemoveListener(InvokeClick);
        transform.DOKill();
        SetNormalColorNoTween();
    }

    public void SetDisabled()
    {
        button.enabled = false;
    }

    public void SetPressedColor()
    {
        if (images.Count > 0)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].DOKill();
                images[i].DOColor(pressedColor, 0.2f);
            }
        }
    }

    public virtual void SetNormalColor()
    {
        if (images.Count > 0)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].DOKill();
                images[i].DOColor(defaultColors[i], 0.05f);
            }
        }
    }

    public virtual void SetSelectedColor()
    {
        if (images.Count > 0)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].DOKill();
                images[i].DOColor(selectedColor, 0.05f);
            }
        }
    }


    public void SetNormalColorNoTween()
    {
        if (images.Count > 0)
        {
            for (int i = 0; i < images.Count; i++)
            {
                images[i].DOKill();
                images[i].color = defaultColors[i];
            }
        }
    }


    protected virtual void InvokeClick()
    {
        // button.enabled = false;
        onClick?.Invoke();
        // ReenableButtonAfterDelay().Forget(); // Fire-and-forget async method
    }
    
    private async UniTaskVoid ReenableButtonAfterDelay()
    {
        await UniTask.Delay(50); // delay in milliseconds
        button.enabled = true;
    }


    public void SetEnabled()
    {
        button.enabled = true;
    }


    public async Task Hide()
    {
        animator.enabled = false;
        await transform.DOScale(Vector3.zero, GS.INS.buttonsAnimateTime).From(Vector3.one)
            .SetEase(GS.INS.buttonsOffEase).AsyncWaitForCompletion();
    }

    public async Task Show()
    {
        await transform.DOScale(Vector3.one, GS.INS.buttonsAnimateTime).From(Vector3.zero)
            .SetEase(GS.INS.buttonsOnEase).AsyncWaitForCompletion();
        animator.enabled = true;
    }
}