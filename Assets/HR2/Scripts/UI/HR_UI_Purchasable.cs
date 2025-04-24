//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Handles purchasing items through the UI.
/// </summary>
public class HR_UI_Purchasable : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// Reference to various purchasable components
    /// </summary>
    RCCP_UI_Color color;
    RCCP_UI_Decal decal;
    RCCP_UI_Neon neon;
    RCCP_UI_Siren siren;
    RCCP_UI_Spoiler spoiler;
    RCCP_UI_Wheel wheel;

    /// <summary>
    /// Indicates whether the item can be purchased
    /// </summary>
    private bool canPurchase = true;

    /// <summary>
    /// Reference to the Button component
    /// </summary>
    private Button button;

    /// <summary>
    /// Indicates whether the item is unlocked
    /// </summary>
    public bool unlocked = false;

    /// <summary>
    /// Price of the item
    /// </summary>
    public int price = 1000;

    /// <summary>
    /// Text component to display the price
    /// </summary>
    public Text priceText;

    /// <summary>
    /// GameObject to display a lock icon
    /// </summary>
    public GameObject lockImage;

    /// <summary>
    /// AudioClip to play when the item is unlocked
    /// </summary>
    public AudioClip unlockAudioclip;

    /// <summary>
    /// Event handler for pointer click events
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerClick(PointerEventData eventData) {

        if (!button)
            button = GetComponent<Button>();

        if (!button) {

            Debug.LogError("Button is not found on " + transform.name + ". Disabling it.");
            enabled = false;
            return;

        }

        if (!unlocked) {

            int currentMoney = HR_API.GetCurrency();

            if (currentMoney >= price) {

                unlocked = true;
                HR_API.ConsumeCurrency(price);
                PlayerPrefs.SetInt("Unlocked_" + transform.name, 1);

                if (unlockAudioclip)
                    RCCP_AudioSource.NewAudioSource(gameObject, unlockAudioclip.name, 0f, 0f, 1f, unlockAudioclip, false, true, true);

            } else {

                if (HR_UI_InfoDisplayer.Instance)
                    HR_UI_InfoDisplayer.Instance.ShowInfo((price - HR_API.GetCurrency()).ToString() + " More Money To Purchase This Item!");

            }

        }

    }

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Awake() {

        if (!button)
            button = GetComponent<Button>();

        color = GetComponent<RCCP_UI_Color>();
        decal = GetComponent<RCCP_UI_Decal>();
        neon = GetComponent<RCCP_UI_Neon>();
        siren = GetComponent<RCCP_UI_Siren>();
        spoiler = GetComponent<RCCP_UI_Spoiler>();
        wheel = GetComponent<RCCP_UI_Wheel>();

    }

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        if (PlayerPrefs.HasKey("Unlocked_" + transform.name))
            unlocked = true;

    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    private void Update() {

        if (HR_API.GetCurrency() >= price)
            canPurchase = true;
        else
            canPurchase = false;

        if (unlocked) {

            ToggleComponent(true);

        } else {

            if (canPurchase)
                ToggleComponent(true);
            else
                ToggleComponent(false);

        }

        if (button)
            button.interactable = unlocked;

        if (lockImage)
            lockImage.SetActive(!unlocked);

        if (priceText) {

            if (!unlocked)
                priceText.text = price.ToString("F0");
            else
                priceText.text = "";

        }

    }

    /// <summary>
    /// Toggle the enabled state of the purchasable components
    /// </summary>
    /// <param name="state">Enabled state</param>
    private void ToggleComponent(bool state) {

        if (color)
            color.enabled = state;

        if (decal)
            decal.enabled = state;

        if (neon)
            neon.enabled = state;

        if (siren)
            siren.enabled = state;

        if (spoiler)
            spoiler.enabled = state;

        if (wheel)
            wheel.enabled = state;

    }

}
