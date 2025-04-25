//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class HR_InitOnLoad {

    [InitializeOnLoadMethod]
    static void InitOnLoad() {

        EditorApplication.delayCall += EditorUpdate;

    }

    public static void EditorUpdate() {

        //HR_SetScriptingSymbol.SetEnabled("BCG_HR_PHOTON", false);
        //HR_SetScriptingSymbol.SetEnabled("PHOTON_UNITY_NETWORKING", false);
        //HR_SetScriptingSymbol.SetEnabled("PUN_2_0_OR_NEWER", false);
        //HR_SetScriptingSymbol.SetEnabled("PUN_2_OR_NEWER", false);
        //HR_SetScriptingSymbol.SetEnabled("PUN_2_19_OR_NEWER", false);
        //HR_SetScriptingSymbol.SetEnabled("BCG_RCC", false);
        //HR_SetScriptingSymbol.SetEnabled("BCG_HR2", false);
        //HR_SetScriptingSymbol.SetEnabled("BCG_SGS", false);
        //HR_SetScriptingSymbol.SetEnabled("AMPLIFY_SHADER_EDITOR", false);

        bool hasKey = false;

#if BCG_HR2
        hasKey = true;
#endif

        if (!hasKey) {

            EditorUtility.DisplayDialog("Regards from BoneCracker Games", "Thank you for purchasing Highway Racer 2. Please read the documentation before use. Also check out the online documentation for updated info. Have fun :)", "Let's get started");
            EditorUtility.DisplayDialog("Current Controller Type", "Current controller type is ''Desktop''. You can swith it from Highway Racer --> Switch to Keyboard / Mobile. You can set initial money value from Highway Racer --> General Settings.", "Ok");

            if (EditorUtility.DisplayDialog("Restart Unity", "Please restart Unity after importing the package. Otherwise inputs may not work for the first time.", "Restart Unity After Compile", "Don't Restart"))
                EditorApplication.OpenProject(Directory.GetCurrentDirectory());

        }

        HR_SetScriptingSymbol.SetEnabled("BCG_HR2", true);

    }

}