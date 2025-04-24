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

public class HR_UI_CreditsText : MonoBehaviour {

    /// <summary>
    /// Reference to the Text component
    /// </summary>
    private Text text;

    /// <summary>
    /// Text to display in the credits
    /// </summary>
    [TextArea] public string creditsText = "2014 - 2023 BoneCracker Games";

    /// <summary>
    /// Called when the object becomes enabled and active
    /// </summary>
    private void OnEnable() {

        // Get the Text component attached to this GameObject
        text = GetComponent<Text>();

        // Set the text of the Text component
        text.text = creditsText;

    }

}
