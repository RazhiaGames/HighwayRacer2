//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Represents a UI cart item displayed in the cart panel.
/// </summary>
public class HR_UI_CartItem : MonoBehaviour {

    /// <summary>
    /// The cart item, including its type and properties.
    /// </summary>
    public HR_CartItem item;

    /// <summary>
    /// Text component displaying the item's name.
    /// </summary>
    public TextMeshProUGUI itemNameText;

    /// <summary>
    /// Text component displaying the item's price.
    /// </summary>
    public TextMeshProUGUI priceText;

    /// <summary>
    /// Sets the item and updates the UI with the item's details.
    /// </summary>
    /// <param name="newItem">The new cart item to be set.</param>
    public void SetItem(HR_CartItem newItem) {

        // Assign the new item to the item field.
        item = newItem;

        // Update the item name text to reflect the item's type.
        itemNameText.text = item.itemType.ToString();

        // Update the price text to reflect the item's price.
        priceText.text = "$ " + item.price.ToString("F0");

    }

}
