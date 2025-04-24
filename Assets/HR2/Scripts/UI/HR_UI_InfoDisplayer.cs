//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

/// <summary>
/// Singleton pattern for displaying various info messages on the UI.
/// </summary>
public class HR_UI_InfoDisplayer : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_UI_InfoDisplayer instance;
    public static HR_UI_InfoDisplayer Instance {
        get {
            if (instance == null) {
                instance = FindObjectOfType<HR_UI_InfoDisplayer>();
            }

            return instance;
        }
    }
    #endregion

    /// <summary>
    /// GameObject for the not enough money message
    /// </summary>
    public GameObject content;

    /// <summary>
    /// Text component for the general info description
    /// </summary>
    public TextMeshProUGUI descText;

    /// <summary>
    /// Show the specified info message
    /// </summary>
    /// <param name="description">Description of the message</param>
    public void ShowInfo(string description) {

        descText.text = description;
        content.SetActive(true);
        StartCoroutine("CloseInfoDelayed");

    }

    /// <summary>
    /// Close the info message
    /// </summary>
    public void CloseInfo() {

        content.SetActive(false);

    }

    /// <summary>
    /// Coroutine to close the info message after a delay
    /// </summary>
    /// <returns>IEnumerator</returns>
    private IEnumerator CloseInfoDelayed() {

        yield return new WaitForSeconds(3);

        content.SetActive(false);

    }

}
