//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Back UI button used in the main menu customization panel. Used to prevent lefted items in the cart.
/// </summary>
public class HR_UI_CustomizationBackButton : MonoBehaviour {

    /// <summary>
    /// The main menu panel to return to.
    /// </summary>
    public GameObject mainMenuPanel;

    /// <summary>
    /// Called when the back button is clicked.
    /// </summary>
    public void OnClick() {

        HR_UI_MainmenuPanel mainmenuPanel = HR_UI_MainmenuPanel.Instance;

        if (!mainmenuPanel)
            return;

        // Inform if player left items in the cart before going back.
        if (mainmenuPanel.itemsInCart.Count > 0) {
            HR_UI_InfoDisplayer.Instance.ShowInfo("You've left items in your cart, please clear or purchase the items!");
            return;
        }

        // Return to the main menu.
        mainmenuPanel.EnableMenu(mainMenuPanel);

        // Set the panel title to "Main Menu".
        mainmenuPanel.SetPanelTitleText("Main Menu");

    }

}
