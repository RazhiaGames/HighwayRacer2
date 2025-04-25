//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HR_Player))]
public class HR_PlayerEditor : Editor {

    HR_Player prop;
    static bool info;

    public override void OnInspectorGUI() {

        prop = (HR_Player)target;
        serializedObject.Update();

        DrawDefaultInspector();

        info = EditorGUILayout.ToggleLeft("Show Info About Upgrades", info);

        if (info) {

            EditorGUILayout.HelpBox("Editing Engine Torque, Handling, Brake, and Maximum Speed\n\nClick 'Engine' component of the vehicle and change the maximum engine torque value.\nClick 'Stability' component of the vehicle and change the 'Steering Helper Strength' with 'Traction Helper Strength' values.\nClick 'Differential' component of the vehicle and change the final drive ratio. Lower values mean higher top speed but slower acceleration. Higher values mean lower top speed, but faster acceleration.", MessageType.Info);
            EditorGUILayout.HelpBox("Customizer\n\nVehicle must be equipped with the Customizer to use customization features. Otherwise, the customization buttons in the main menu will be disabled for this vehicle.", MessageType.Info);
            EditorGUILayout.HelpBox("NOS\n\nVehicle must be equipped with the NOS (In 'Other Addons' component).", MessageType.Info);
            EditorGUILayout.HelpBox("Maximum Upgradable Values\n\nThese values will be calculated by multiplying the default values with 1.2x. With this way, each vehicle will have fair upgrade stats.", MessageType.Info);

        }

        bool isPersistent = EditorUtility.IsPersistent(prop.gameObject);

        if (isPersistent)
            GUI.enabled = false;

        if (!Application.isPlaying) {

            if (PrefabUtility.GetCorrespondingObjectFromSource(prop.gameObject) == null) {

                EditorGUILayout.HelpBox("You'll need to create a new prefab for the vehicle first.", MessageType.Info);
                Color defColor = GUI.color;
                GUI.color = Color.red;

                if (GUILayout.Button("Create Prefab"))
                    CreatePrefab();

                GUI.color = defColor;

            } else {

                EditorGUILayout.HelpBox("Don't forget to save changes.", MessageType.Info);
                Color defColor = GUI.color;
                GUI.color = Color.green;

                if (GUILayout.Button("Save Prefab"))
                    SavePrefab();

                GUI.color = defColor;

            }

            GUI.enabled = true;

            bool foundPrefab = false;

            for (int i = 0; i < HR_PlayerCars.Instance.cars.Length; i++) {

                if (HR_PlayerCars.Instance.cars[i].playerCar != null) {

                    if (prop.transform.name == HR_PlayerCars.Instance.cars[i].playerCar.transform.name) {

                        foundPrefab = true;
                        break;

                    }

                }

            }

            if (!foundPrefab) {

                EditorGUILayout.HelpBox("Player vehicles list doesn't include this vehicle yet!", MessageType.Info);
                Color defColor = GUI.color;
                GUI.color = Color.green;

                if (GUILayout.Button("Add Prefab To Player Vehicles List")) {

                    if (PrefabUtility.GetCorrespondingObjectFromSource(prop.gameObject) == null)
                        CreatePrefab();
                    else
                        SavePrefab();

                    AddToList();

                }

                GUI.color = defColor;

            }

        }

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

    private void CreatePrefab() {

        PrefabUtility.SaveAsPrefabAssetAndConnect(prop.gameObject, "Assets/HR2/Prefabs/Player Vehicles/" + prop.gameObject.name + ".prefab", InteractionMode.UserAction);
        Debug.Log("Created Prefab");

        EditorUtility.SetDirty(prop);

    }

    private void SavePrefab() {

        PrefabUtility.SaveAsPrefabAssetAndConnect(prop.gameObject, "Assets/HR2/Prefabs/Player Vehicles/" + prop.gameObject.name + ".prefab", InteractionMode.UserAction);
        Debug.Log("Saved Prefab");

        EditorUtility.SetDirty(prop);

    }

    private void AddToList() {

        PlayerPrefs.SetInt("SelectedPlayerCarIndex", 0);
        Debug.Log("Added Prefab To The Player Vehicles List");

        HR_PlayerCars.Cars newCar = new HR_PlayerCars.Cars();
        newCar.vehicleName = "New Player Vehicle " + Random.Range(0, 100).ToString();
        newCar.playerCar = PrefabUtility.GetCorrespondingObjectFromSource(prop.gameObject);

        HR_PlayerCars.Instance.lastAdd = new HR_PlayerCars.Cars();
        HR_PlayerCars.Instance.lastAdd = newCar;
        Selection.activeObject = HR_PlayerCars.Instance;

    }

}
