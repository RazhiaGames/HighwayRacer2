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
using TMPro;

/// <summary>
/// UI button for purchasing the items.
/// </summary>
public class HR_UI_PurchaseItem : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// The cart item associated with this button.
    /// </summary>
    public HR_CartItem item;

    /// <summary>
    /// The panel displaying the price.
    /// </summary>
    public GameObject pricePanel;

    /// <summary>
    /// The text displaying the item's price.
    /// </summary>
    public TextMeshProUGUI priceText;

    /// <summary>
    /// The button component for purchasing the item.
    /// </summary>
    private Button button;

    /// <summary>
    /// Indicates whether the item is purchased.
    /// </summary>
    public bool isPurchased = false;

    /// <summary>
    /// Initializes the button state and UI elements.
    /// </summary>
    private void Awake() {

        button = GetComponent<Button>();
        isPurchased = CheckPurchase();
        pricePanel.SetActive(!isPurchased);
        priceText.text = isPurchased ? "" : "$" + item.price.ToString("F0");

    }

    /// <summary>
    /// Updates the button state and UI elements when the object is enabled.
    /// </summary>
    public void OnEnable() {

        isPurchased = CheckPurchase();
        pricePanel.SetActive(!isPurchased);
        priceText.text = isPurchased ? "" : "$" + item.price.ToString("F0");

    }

    /// <summary>
    /// Checks whether the item is purchased.
    /// </summary>
    /// <returns>True if the item is purchased, otherwise false.</returns>
    public bool CheckPurchase() {

        isPurchased = PlayerPrefs.HasKey(item.saveKey);
        pricePanel.SetActive(!isPurchased);
        priceText.text = isPurchased ? "" : "$" + item.price.ToString("F0");
        return isPurchased;

    }

    /// <summary>
    /// Handles the click event to purchase the item.
    /// </summary>
    /// <param name="eventData">Event data for the pointer click.</param>
    public void OnPointerClick(PointerEventData eventData) {

        if (!button.interactable || !button.gameObject.activeSelf)
            return;

        isPurchased = CheckPurchase();
        HR_UI_MainmenuPanel.Instance.CheckItemPurchased(item);

    }

}
