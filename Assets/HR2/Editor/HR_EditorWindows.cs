//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class HR_EditorWindows : Editor {


    [MenuItem("GameObject/BoneCracker Games/Highway Racer 2/Create/HR Scene Manager", false, -100)]
    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/Create/HR Scene Manager", false, -100)]
    public static void SceneManager() {

        Selection.activeGameObject = HR_SceneManager.Instance.gameObject;

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/HR Settings", false, -100)]
    public static void OpenGeneralSettings() {

        Selection.activeObject = HR_Settings.Instance;

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/Quick Switch To Desktop", false, -50)]
    public static void QuickSwitchToDesktop() {

        Selection.activeObject = RCCP_Settings.Instance;
        RCCP_Settings.Instance.mobileControllerEnabled = false;

        EditorUtility.SetDirty(RCCP_Settings.Instance);

        if (EditorUtility.DisplayDialog("Switched Build", "RCC Controller has been switched to Keyboard mode.", "Ok"))
            Selection.activeObject = HR_Settings.Instance;

        EditorUtility.SetDirty(HR_Settings.Instance);

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/Quick Switch To Mobile", false, -50)]
    public static void QuickSwitchToMobile() {

        Selection.activeObject = RCCP_Settings.Instance;
        RCCP_Settings.Instance.mobileControllerEnabled = true;

        EditorUtility.SetDirty(RCCP_Settings.Instance);

        if (EditorUtility.DisplayDialog("Switched Build", "RCC Controller has been switched to Mobile mode.", "Ok"))
            Selection.activeObject = HR_Settings.Instance;

        EditorUtility.SetDirty(HR_Settings.Instance);

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/Configure Player Cars", false, 1)]
    public static void OpenCarSettings() {

        Selection.activeObject = HR_PlayerCars.Instance;

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/Configure Upgradable Wheels", false, 1)]
    public static void OpenWheelsSettings() {

        Selection.activeObject = RCCP_ChangableWheels.Instance;

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/PDF Documentation", false, 2)]
    public static void OpenDocs() {

        string url = "https://bonecrackergames.com/bonecrackergames.com/admin/AssetStoreDemos/HR2/Documentation.rar";
        Application.OpenURL(url);

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/Highlight Player Cars Folder", false, 100)]
    public static void OpenPlayerCarsFolder() {

        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/HR2/Prefabs/Player Vehicles");

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/Highlight Wheels Folder", false, 101)]
    public static void OpenWheelsFolder() {

        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/HR2/Prefabs/Wheels");

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/Highlight Traffic Cars Folder", false, 102)]
    public static void OpenTrafficCarsFolder() {

        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/HR2/Prefabs/Traffic Vehicles");

    }

    [MenuItem("Tools/BoneCracker Games/Highway Racer 2/Help", false, 1000)]
    static void Help() {

        EditorUtility.DisplayDialog("Contact", "Please include your invoice number while sending a contact form.", "Ok");

        string url = "https://www.bonecrackergames.com/contact/";
        Application.OpenURL(url);

    }

}
