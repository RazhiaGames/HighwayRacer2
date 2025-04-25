using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(HR_Object))]
[CanEditMultipleObjects]
public class HR_ObjectEditor : Editor {

    HR_Object prop;

    public override void OnInspectorGUI() {

        prop = (HR_Object)target;
        serializedObject.Update();

        DrawDefaultInspector();

        if (Selection.gameObjects.Length <= 1) {

            if (!prop.targetBone) {

                if (GUILayout.Button("Connect To Closest Path Point")) {

                    prop.FindClosest();
                    EditorUtility.SetDirty(prop);

                }

            } else {

                if (GUILayout.Button("Disconnect Path Point")) {

                    prop.targetBone = null;
                    EditorUtility.SetDirty(prop);

                }

            }

        } else {

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("All Connect To Closest Path Point")) {

                for (int i = 0; i < Selection.gameObjects.Length; i++) {

                    HR_Object objectScript = Selection.gameObjects[i].GetComponent<HR_Object>();

                    if (objectScript) {

                        objectScript.FindClosest();
                        EditorUtility.SetDirty(objectScript);

                    }

                }

            }

            if (GUILayout.Button("All Disconnect Path Point")) {

                for (int i = 0; i < Selection.gameObjects.Length; i++) {

                    HR_Object objectScript = Selection.gameObjects[i].GetComponent<HR_Object>();

                    if (objectScript) {

                        objectScript.targetBone = null;
                        EditorUtility.SetDirty(objectScript);

                    }

                }

            }

            EditorGUILayout.EndHorizontal();

        }

        serializedObject.ApplyModifiedProperties();

    }

}
