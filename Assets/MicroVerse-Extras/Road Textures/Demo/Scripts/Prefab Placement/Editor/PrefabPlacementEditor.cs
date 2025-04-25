using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Rowlan.PrefabPlacement
{
    [CustomEditor(typeof(PrefabPlacement))]
    public class PrefabPlacementEditor : Editor
    {
        PrefabPlacement editorTarget;
        PrefabPlacementEditor editor;

        public void OnEnable()
        {
            editor = this;
            editorTarget = (PrefabPlacement)target;
        }

        public override void OnInspectorGUI()
        {
			serializedObject.Update();
		
            base.DrawDefaultInspector();

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Create Grid"))
                    {
                        PerformFixedSizeGridPlacement();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
			
			serializedObject.ApplyModifiedProperties();
        }

        private List<T> LoadMaterials<T>( string pathFilter) where T: Material
        {
            var list = AssetDatabase.FindAssets($"t: {typeof(T).Name}")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Where(t => t.Contains(pathFilter))
            .Select(AssetDatabase.LoadAssetAtPath<T>)
            .ToList();

            return list;
        }

        private void PerformFixedSizeGridPlacement()
        {
            // load
            Material[] materials = LoadMaterials<Material>( editorTarget.pathFilter).ToArray();
            int totalCount = materials.Length;

            // clear
            for ( int i= editorTarget.transform.childCount - 1; i >= 0; i--)
            {
                Editor.DestroyImmediate( editorTarget.transform.GetChild(i).gameObject);
            }

            // create grid
            float x = 0;
            float z = 0;

            for( int i = 0; i < totalCount; i++)
            {
                Material material = materials[i];

                // x
                if (i % editorTarget.columnCount == 0)
                {
                    x = 0;
                }
                else
                {
                    x += editorTarget.margin;
                    x += editorTarget.size;
                }

                // z
                // z = (int)(i / editorTarget.columnCount);

                // go top-town in z direction
                float directionZ = -1;

                if (i % editorTarget.columnCount == 0 && i >= editorTarget.columnCount) 
                {
                    z += editorTarget.margin * directionZ;
                    z += editorTarget.size * directionZ;
                }

                GameObject prefab = (GameObject) PrefabUtility.InstantiatePrefab( editorTarget.prefab, editorTarget.transform);
                GameObjectUtility.SetParentAndAlign(prefab, editorTarget.transform.gameObject);

                prefab.name = material.name; // editorTarget.namePrefix + i;

                prefab.transform.localPosition = new Vector3(x, 0, z);

                // assign material
                prefab.GetComponent<Renderer>().material = material;

                // text gameobject
                var go = new GameObject();
                GameObjectUtility.SetParentAndAlign(go, editorTarget.transform.gameObject);

                go.transform.parent = editorTarget.transform;
                go.name = "Text " + material.name;

                TextMesh text = go.AddComponent<TextMesh>();
                text.text = prefab.name;
                text.fontSize = editorTarget.fontSize;

                text.anchor = TextAnchor.MiddleCenter;
                text.alignment = TextAlignment.Center;

                // center below gameobject, ie in the margin area
                float textZ = editorTarget.size / 2f * directionZ + editorTarget.margin / 2f * directionZ + editorTarget.textOffsetZ;

                text.transform.localPosition = prefab.transform.localPosition + new Vector3(0, 0, textZ);

                text.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                text.transform.localScale = Vector3.one;

            }



            // header
            {
                // text gameobject
                string headerText = Path.GetFileName( editorTarget.pathFilter);

                var go = new GameObject();
                GameObjectUtility.SetParentAndAlign(go, editorTarget.transform.gameObject);

                go.transform.parent = editorTarget.transform;
                go.name = "Header " + headerText;

                TextMesh text = go.AddComponent<TextMesh>();
                text.text = headerText;
                text.fontSize = editorTarget.headerFontSize;

                text.anchor = TextAnchor.MiddleLeft;
                text.alignment = TextAlignment.Center;

                text.transform.localPosition = new Vector3(editorTarget.headerOffsetX, 0, editorTarget.headerOffsetZ);

                text.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));
                text.transform.localScale = Vector3.one;
            }
        }

        public static bool ContainsIgnoreCase(string text, string filterText)
        {
            return CultureInfo.CurrentCulture.CompareInfo.IndexOf(text, filterText, CompareOptions.IgnoreCase) >= 0;
        }
    }
}

