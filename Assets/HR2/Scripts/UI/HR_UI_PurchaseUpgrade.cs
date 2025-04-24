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
/// UI upgrader button for engine, handling, and speed.
/// </summary>
public class HR_UI_PurchaseUpgrade : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// Upgradable item.
    /// </summary>
    public HR_CartItem item;

    /// <summary>
    /// Gets the upgrade level from the button's level text.
    /// </summary>
    public int UpgradeLevel {

        get {

            return int.Parse(button.levelText.text);

        }

    }

    /// <summary>
    /// Gets the calculated upgrade price based on the upgrade level.
    /// </summary>
    public int UpgradePrice {

        get {

            return (int)(Mathf.InverseLerp(0, 6, UpgradeLevel + 1) * (defaultPrice * 1.5f));

        }

    }

    /// <summary>
    /// Default price of the item.
    /// </summary>
    private int defaultPrice = 0;

    /// <summary>
    /// Maximum upgrade level.
    /// </summary>
    public int maximumLevel = 5;

    /// <summary>
    /// The panel displaying the upgrade price.
    /// </summary>
    public GameObject pricePanel;

    /// <summary>
    /// The text displaying the upgrade price.
    /// </summary>
    public TextMeshProUGUI priceText;

    /// <summary>
    /// Reference to the upgrade button component.
    /// </summary>
    private RCCP_UI_Upgrade button;

    /// <summary>
    /// Indicates whether the item is fully upgraded.
    /// </summary>
    public bool isUpgraded = false;

    /// <summary>
    /// Initializes the button and checks the upgrade state.
    /// </summary>
    private void Awake() {

        defaultPrice = item.price;
        CheckPurchase();

    }

    /// <summary>
    /// Updates the button state and UI elements when the object is enabled.
    /// </summary>
    public void OnEnable() {

        if (!button)
            button = GetComponent<RCCP_UI_Upgrade>();

        if (!button)
            return;

        isUpgraded = CheckPurchase();

    }

    /// <summary>
    /// Checks if the item is fully upgraded.
    /// </summary>
    /// <returns>True if the item is fully upgraded, otherwise false.</returns>
    public bool CheckPurchase() {

        if (!button)
            button = GetComponent<RCCP_UI_Upgrade>();

        if (!button)
            return false;

        button.Check();
        isUpgraded = UpgradeLevel >= maximumLevel;
        button.enabled = !isUpgraded;
        pricePanel.SetActive(!isUpgraded);
        priceText.text = isUpgraded ? "" : "$" + UpgradePrice.ToString("F0");

        return isUpgraded;

    }

    /// <summary>
    /// Handles the click event to purchase the upgrade.
    /// </summary>
    /// <param name="eventData">Event data for the pointer click.</param>
    public void OnPointerClick(PointerEventData eventData) {

        if (!button)
            button = GetComponent<RCCP_UI_Upgrade>();

        if (!button)
            return;

        if (!button.isActiveAndEnabled || !button.gameObject.activeSelf)
            return;

        isUpgraded = CheckPurchase();

        if (isUpgraded)
            return;

        HR_CartItem upgradeItem = item;
        upgradeItem.price = UpgradePrice;
        HR_UI_MainmenuPanel.Instance.CheckUpgradePurchased(item);

    }

}
