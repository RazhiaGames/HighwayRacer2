//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class HR_PrePlayModeActions {

    static HR_PrePlayModeActions() {

        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state) {

        if (state == PlayModeStateChange.ExitingEditMode) {

            HR_DisableBeforePlay disableBeforePlay = HR_DisableBeforePlay.Instance;

            if (!disableBeforePlay)
                return;

            // Find your specific GameObjects and disable them
            foreach (GameObject obj in disableBeforePlay.targetGameobjectsToDisable)
                obj.SetActive(false);

        }

        if (state == PlayModeStateChange.EnteredEditMode) {

            HR_DisableBeforePlay disableBeforePlay = HR_DisableBeforePlay.Instance;

            if (!disableBeforePlay)
                return;

            // Find your specific GameObjects and disable them
            foreach (GameObject obj in disableBeforePlay.targetGameobjectsToDisable)
                obj.SetActive(true);

        }

    }

}
