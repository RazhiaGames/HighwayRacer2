#if USING_PHOTOSESSION
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace Rowlan.AutoScreenshot
{
    [CustomEditor(typeof(AutoScreenshot))]
    public class AutoScreenshotEditor : Editor
    {
        AutoScreenshot editorTarget;
        AutoScreenshotEditor editor;

        public void OnEnable()
        {
            editor = this;
            editorTarget = (AutoScreenshot)target;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
		
            base.DrawDefaultInspector();

            EditorGUILayout.BeginVertical();
            {
                EditorGUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Change Materials and Capture Screenshots"))
                    {
                        PerformAction();
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
			
			serializedObject.ApplyModifiedProperties();
        }

        private void PerformAction()
        {
            EditorCoroutineUtility.StartCoroutine(CaptureScene(), this);
        }

        /// <summary>
        /// Load all material configs
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<T> LoadRoadMaterialConfigs<T>() where T : RoadMaterialConfig
        {
            var list = AssetDatabase.FindAssets($"t: {typeof(T).Name}")
            .Select(AssetDatabase.GUIDToAssetPath)
            .Select(AssetDatabase.LoadAssetAtPath<T>)
            .ToList();

            return list;

        }

        /// <summary>
        /// Get current material config, get all materials of that config, iterate through them and assign them, then take a screenshot for each using the current PhotoSession setup.
        /// </summary>
        /// <returns></returns>
        private IEnumerator CaptureScene()
        {
            Debug.Log("Task started");

            // find the currently selected config of the road system
            RoadMaterialConfig[] configs = LoadRoadMaterialConfigs<RoadMaterialConfig>().ToArray();

            RoadMaterialConfig currentConfig = null;
            foreach( RoadMaterialConfig config in configs )
            {
                if(config.contentID == editorTarget.roadSystem.contentID)
                {
                    currentConfig = config;
                    break;
                }
            }

            if (currentConfig == null)
                yield return null;

            // get template materials
            List<RoadMaterialConfig.Entry> templateMaterials = currentConfig.templateMaterials;
            
            // assign each material and create a screenshot
            foreach (RoadMaterialConfig.Entry entry in templateMaterials)
            {
                yield return new WaitForSeconds(1); // just some random waiting time
                yield return new WaitForEndOfFrame();

                editorTarget.roadSystem.templateMaterial = entry.material;
                editorTarget.roadSystem.UpdateMaterialOverrides();
                editorTarget.roadSystem.UpdateAll();

                yield return new WaitForSeconds(1);  // just some random waiting time
                yield return new WaitForEndOfFrame();

                // take screenshot
                PhotoSession.PhotoSession ps = editorTarget.photoSession;
                ps.CaptureSceneView();

            }

            Debug.Log("Task finished");
        }

    }
}
#endif