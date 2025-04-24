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
/// UI button for removing the item from the cart.
/// </summary>
public class HR_UI_RemoveItem : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// The cart item associated with this remove button.
    /// </summary>
    public HR_CartItem item;

    /// <summary>
    /// Handles the click event to remove the item from the cart.
    /// </summary>
    /// <param name="eventData">Event data for the pointer click.</param>
    public void OnPointerClick(PointerEventData eventData) {

        HR_MainMenuManager.Instance.RemoveItemFromCart(item);

    }

}
