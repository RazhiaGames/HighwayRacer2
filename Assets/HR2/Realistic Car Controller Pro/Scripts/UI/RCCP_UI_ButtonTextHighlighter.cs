//----------------------------------------------
//        Realistic Car Controller Pro
//
// Copyright © 2014 - 2024 BoneCracker Games
// https://www.bonecrackergames.com
// Ekrem Bugra Ozdoganlar
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

/// <summary>
/// UI text highlighter when mouse hovers.
/// </summary>
[AddComponentMenu("BoneCracker Games/Realistic Car Controller Pro/UI/RCCP UI Button Text Highlighter")]
public class RCCP_UI_ButtonTextHighlighter : RCCP_UIComponent, IPointerEnterHandler, IPointerExitHandler {

    private TextMeshProUGUI text;
    private Animator animator;

    public Color defaultTextColor = Color.white;
    public Color targetTextColor = Color.black;

    private bool hovering = false;
    public float speed = 10f;

    private void Awake() {

        text = GetComponentInChildren<TextMeshProUGUI>();
        animator = GetComponentInChildren<Animator>();

        defaultTextColor = text.color;

    }

    private void OnEnable() {

        hovering = false;
        text.color = defaultTextColor;

    }

    private void Update() {

        if (hovering)
            text.color = Color.Lerp(text.color, targetTextColor, Time.deltaTime * speed);
        else
            text.color = Color.Lerp(text.color, defaultTextColor, Time.deltaTime * speed);

    }

    public void OnPointerEnter(PointerEventData eventData) {

        hovering = true;

        if (animator)
            animator.Play(0);

    }

    public void OnPointerExit(PointerEventData eventData) {

        hovering = false;

    }

    private void OnDisable() {

        hovering = false;
        text.color = defaultTextColor;

    }

}
