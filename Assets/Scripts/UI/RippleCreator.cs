using System;
using Michsky.MUIP;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RippleCreator : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private GameObject rippleParent;

    [SerializeField]
    private ButtonManager.RippleUpdateMode rippleUpdateMode = ButtonManager.RippleUpdateMode.UnscaledTime;

    [SerializeField] private Canvas targetCanvas;
    public Sprite rippleShape;
    [Range(0.1f, 5)] public float speed = 1f;
    [Range(0.5f, 25)] public float maxSize = 4f;
    public Color startColor = new Color(1f, 1f, 1f, 0.2f);
    public Color transitionColor = new Color(1f, 1f, 1f, 0f);
    [SerializeField] private bool renderOnTop = false;
    [SerializeField] private bool centered = false;

    private void OnEnable()
    {
        if (targetCanvas == null)
        {
            targetCanvas = GetComponentInParent<Canvas>();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetCanvas != null && (targetCanvas.renderMode == RenderMode.ScreenSpaceCamera ||
                                     targetCanvas.renderMode == RenderMode.WorldSpace))
        {
            CreateRipple(targetCanvas.worldCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }
#if UNITY_IOS || UNITY_ANDROID
        else
        {
            CreateRipple(Touchscreen.current.primaryTouch.position.ReadValue());
        }
#else
                        else { CreateRipple(Mouse.current.position.ReadValue()); }
#endif
    }


    public void CreateRipple(Vector2 pos)
    {
        if (rippleParent != null)
        {
            GameObject rippleObj = new GameObject();
            rippleObj.AddComponent<Image>();
            rippleObj.GetComponent<Image>().sprite = rippleShape;
            rippleObj.name = "Ripple";
            rippleParent.SetActive(true);
            rippleObj.transform.SetParent(rippleParent.transform);

            if (renderOnTop == true)
            {
                rippleParent.transform.SetAsLastSibling();
            }
            else
            {
                rippleParent.transform.SetAsFirstSibling();
            }

            if (centered == true)
            {
                rippleObj.transform.localPosition = new Vector2(0f, 0f);
            }
            else
            {
                rippleObj.transform.position = pos;
            }

            rippleObj.AddComponent<Ripple>();
            Ripple tempRipple = rippleObj.GetComponent<Ripple>();
            tempRipple.speed = speed;
            tempRipple.maxSize = maxSize;
            tempRipple.startColor = startColor;
            tempRipple.transitionColor = transitionColor;

            if (rippleUpdateMode == ButtonManager.RippleUpdateMode.Normal)
            {
                tempRipple.unscaledTime = false;
            }
            else
            {
                tempRipple.unscaledTime = true;
            }
        }
    }
}