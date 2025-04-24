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

[RequireComponent(typeof(RCCP_UI_Upgrade))]
public class HR_UI_Purchasable_ForUpgrades : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// Reference to the RCCP_UI_Upgrade component
    /// </summary>
    private RCCP_UI_Upgrade upgraderButton;

    /// <summary>
    /// Indicates whether the item can be upgraded
    /// </summary>
    private bool canUpgrade = true;

    /// <summary>
    /// Indicates whether the item can be purchased
    /// </summary>
    private bool canPurchase = true;

    /// <summary>
    /// Maximum upgrade level for the item
    /// </summary>
    public int maxLevel = 5;

    /// <summary>
    /// Price of the item
    /// </summary>
    public int price = 1000;

    /// <summary>
    /// Text component to display the price
    /// </summary>
    public Text priceText;

    /// <summary>
    /// Text component to display the current level
    /// </summary>
    public Text levelText;

    /// <summary>
    /// AudioClip to play when the item is unlocked
    /// </summary>
    public AudioClip unlockAudioclip;

    /// <summary>
    /// Event handler for pointer click events
    /// </summary>
    /// <param name="eventData">Pointer event data</param>
    public void OnPointerClick(PointerEventData eventData) {

        if (!upgraderButton)
            return;

        if (!levelText)
            return;

        int.TryParse(levelText.text, out int level);

        if (level > maxLevel)
            canUpgrade = false;

        if (canUpgrade) {

            int currentMoney = HR_API.GetCurrency();

            if (currentMoney >= price) {

                HR_API.ConsumeCurrency(price);

                if (unlockAudioclip)
                    RCCP_AudioSource.NewAudioSource(gameObject, unlockAudioclip.name, 0f, 0f, 1f, unlockAudioclip, false, true, true);

            } else {

                if (HR_UI_InfoDisplayer.Instance)
                    HR_UI_InfoDisplayer.Instance.ShowInfo((price - HR_API.GetCurrency()).ToString("F0") + " More Money Needed To Purchase This Item!");

            }

        }

    }

    /// <summary>
    /// Called when the object is initialized
    /// </summary>
    private void Awake() {

        upgraderButton = GetComponent<RCCP_UI_Upgrade>();

    }

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        upgraderButton.Check();

        int.TryParse(levelText.text, out int level);

        if (level >= maxLevel)
            canUpgrade = false;
        else
            canUpgrade = true;

    }

    /// <summary>
    /// Update method called once per frame
    /// </summary>
    private void Update() {

        if (HR_API.GetCurrency() >= price)
            canPurchase = true;
        else
            canPurchase = false;

        if (upgraderButton) {

            if (canPurchase && canUpgrade)
                upgraderButton.GetComponent<Button>().interactable = true;
            else
                upgraderButton.GetComponent<Button>().interactable = false;

        }

        if (priceText) {

            if (canUpgrade)
                priceText.text = price.ToString("F0");
            else
                priceText.text = "";

        }

        int.TryParse(levelText.text, out int level);

        if (level >= maxLevel)
            canUpgrade = false;

    }

}
