//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HR_DisableBeforePlay : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_DisableBeforePlay instance;
    public static HR_DisableBeforePlay Instance {

        get {

            if (instance == null)
                instance = FindObjectOfType<HR_DisableBeforePlay>();

            return instance;

        }

    }
    #endregion

    public GameObject[] targetGameobjectsToDisable;

}
