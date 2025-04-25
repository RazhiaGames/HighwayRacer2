//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(HR_Settings))]
public class HR_SettingsEditor : Editor {

    HR_Settings prop;

    Vector2 scrollPos;
    GameObject[] playerCars;
    GameObject[] upgradableWheels;

    GUISkin skin;
    Color orgColor;

    private int _Width = 500;
    public int Width {

        get {

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            Rect scale = GUILayoutUtility.GetLastRect();

            if (scale.width != 1)
                _Width = System.Convert.ToInt32(scale.width);

            return _Width;

        }

    }

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("HR_Gui");
        orgColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (HR_Settings)target;
        serializedObject.Update();
        GUI.skin = skin;

        orgColor = GUI.color;

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Highway Racer Properties Editor Window", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("This editor will keep update necessary .asset files in your project. Don't change directory of the ''Resources/''.", EditorStyles.helpBox);
        EditorGUILayout.Space();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, false, false);

        EditorGUIUtility.labelWidth = 180f;

        GUILayout.Label("General Settings", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("minimumSpeedForGainScore"), new GUIContent("Min Speed For Gain Score"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minimumSpeedForHighSpeed"), new GUIContent("Min Speed For High Speed"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minimumCollisionForGameOver"), new GUIContent("Min Collision Impulse"), false);

        EditorGUILayout.Space();

        GUILayout.Label("Default Config At First Initialization", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("initialMoney"), new GUIContent("Initial Money"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultAudioVolume"), new GUIContent("Default Audio Volume"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultMusicVolume"), new GUIContent("Default Music Volume"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultDrawDistance"), new GUIContent("Default Draw Distance"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultShadows"), new GUIContent("Default Shadows"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defaultPP"), new GUIContent("Default PostProcessing"), false);

        EditorGUILayout.Space();

        GUILayout.Label("Score Multipliers", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalDistanceMoneyMP"), new GUIContent("Total Distance MP"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalNearMissMoneyMP"), new GUIContent("Total Near Miss MP"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalOverspeedMoneyMP"), new GUIContent("Total Over Speed MP"), false);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("totalOppositeDirectionMP"), new GUIContent("Total Opposite Direction MP"), false);

        EditorGUILayout.Space();

        GUILayout.Label("Sound Effects", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(serializedObject.FindProperty("buttonClickAudioClip"), new GUIContent("Button Click SFX"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("nearMissAudioClip"), new GUIContent("Near Miss SFX"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("labelSlideAudioClip"), new GUIContent("Label Slide SFX"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("countingPointsAudioClip"), new GUIContent("Counting Points SFX"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("bombTimerAudioClip"), new GUIContent("Bomb Timer Beep SFX"));
        EditorGUILayout.Space();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("sirenAudioClip"), new GUIContent("Police Siren SFX"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("hornClip"), new GUIContent("Horn SFX"));

        EditorGUILayout.Space();

        GUI.color = new Color(.5f, 1f, 1f, 1f);
        GUILayout.Label("Select Main Controller Type", EditorStyles.boldLabel);

        if (GUILayout.Button("Switch Controller from RCCP Settings"))
            Selection.activeObject = RCCP_Settings.Instance;

        GUI.color = orgColor;

        EditorGUILayout.Space();

        GUILayout.Label("Selectable Player Cars", EditorStyles.boldLabel);

        playerCars = new GameObject[HR_PlayerCars.Instance.cars.Length];

        for (int i = 0; i < playerCars.Length; i++) {

            playerCars[i] = HR_PlayerCars.Instance.cars[i].playerCar;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.ObjectField("Player Car " + i, playerCars[i], typeof(GameObject), false);
            EditorGUILayout.EndVertical();

        }

        GUI.color = Color.cyan;

        if (GUILayout.Button("Configure Player Cars"))
            Selection.activeObject = Resources.Load("HR_PlayerCars") as HR_PlayerCars;

        GUI.color = orgColor;

        EditorGUILayout.Space();

        GUILayout.Label("Upgradable Wheels", EditorStyles.boldLabel);

        upgradableWheels = new GameObject[RCCP_ChangableWheels.Instance.wheels.Length];

        for (int i = 0; i < upgradableWheels.Length; i++) {

            upgradableWheels[i] = RCCP_ChangableWheels.Instance.wheels[i].wheel;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.ObjectField("Upgradable Wheels " + i, upgradableWheels[i], typeof(GameObject), false);
            EditorGUILayout.EndVertical();

        }

        GUI.color = Color.cyan;

        if (GUILayout.Button("Configure Upgradable Wheels"))
            Selection.activeObject = RCCP_ChangableWheels.Instance;

        GUI.color = orgColor;

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("trafficCarsLayer"), new GUIContent("Layer Of The Traffic Cars"));

        EditorGUILayout.Space();

        GUILayout.Label("Resources", EditorStyles.boldLabel);

        prop.gameplayCamera = (HR_Camera)EditorGUILayout.ObjectField("Gameplay Camera", prop.gameplayCamera, typeof(HR_Camera), false);
        prop.showroomCamera = (HR_Camera_Showroom)EditorGUILayout.ObjectField("Showroom Camera", prop.showroomCamera, typeof(HR_Camera_Showroom), false);
        prop.road = (HR_CurvedRoad)EditorGUILayout.ObjectField("Curved Road", prop.road, typeof(HR_CurvedRoad), false);
        prop.UI_GameplayPanel = (GameObject)EditorGUILayout.ObjectField("Gameplay UI Canvas", prop.UI_GameplayPanel, typeof(GameObject), false);
        prop.UI_GameoverPanel = (GameObject)EditorGUILayout.ObjectField("Gameover UI Canvas", prop.UI_GameoverPanel, typeof(GameObject), false);
        prop.UI_MainmenuPanel = (GameObject)EditorGUILayout.ObjectField("Mainmenu UI Canvas", prop.UI_MainmenuPanel, typeof(GameObject), false);
        prop.explosionEffect = (GameObject)EditorGUILayout.ObjectField("Explosion Particles", prop.explosionEffect, typeof(GameObject), false);
        prop.resetDecal = (HR_ResetDecal)EditorGUILayout.ObjectField("Reset Decal Renderer", prop.resetDecal, typeof(HR_ResetDecal), false);

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Highway Racer 2 V" + HR_Version.version + "\nDeveloped by Ekrem Bugra Ozdoganlar\nBoneCracker Games", EditorStyles.centeredGreyMiniLabel, GUILayout.MaxHeight(50f));

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        serializedObject.ApplyModifiedProperties();

    }

}
