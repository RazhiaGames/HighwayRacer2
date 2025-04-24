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
/// Represents a cart item used in the main menu of the game.
/// </summary>
[System.Serializable]
public class HR_CartItem {

    /// <summary>
    /// Enumeration for the different types of cart items.
    /// </summary>
    public enum CartItemType {
        /// <summary>
        /// Represents a paint item for customizing the car's color.
        /// </summary>
        Paint,
        /// <summary>
        /// Represents a wheel item for changing the car's wheels.
        /// </summary>
        Wheel,
        /// <summary>
        /// Represents a spoiler item for adding a spoiler to the car.
        /// </summary>
        Spoiler,
        /// <summary>
        /// Represents a decal item for adding decals to the car.
        /// </summary>
        Decal,
        /// <summary>
        /// Represents a neon light item for adding neon lights to the car.
        /// </summary>
        Neon,
        /// <summary>
        /// Represents an engine upgrade item.
        /// </summary>
        UpgradeEngine,
        /// <summary>
        /// Represents a handling upgrade item.
        /// </summary>
        UpgradeHandling,
        /// <summary>
        /// Represents a speed upgrade item.
        /// </summary>
        UpgradeSpeed,
        /// <summary>
        /// Represents customization item.
        /// </summary>
        Customization
    }

    /// <summary>
    /// The type of the cart item.
    /// </summary>
    public CartItemType itemType;

    /// <summary>
    /// The key used to save the item state in the player's data.
    /// </summary>
    public string saveKey;

    /// <summary>
    /// The price of the cart item.
    /// </summary>
    public int price;
}
