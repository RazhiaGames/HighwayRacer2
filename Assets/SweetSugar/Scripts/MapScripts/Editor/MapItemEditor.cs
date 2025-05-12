// // ©2015 - 2023 Candy Smith
// // All rights reserved
// // Redistribution of this software is strictly not allowed.
// // Copy of this software can be obtained from unity asset store only.
// // THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// // IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// // FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
// // AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// // LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// // OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// // THE SOFTWARE.

using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SweetSugar.Scripts.MapScripts.Editor
{
    [CustomEditor(typeof(MapItem))]
    public class MapItemEditor : LevelsEditorBase
    {
        private MapItem _mapItem;

        private static GameObject _pendingDeletedGameObject;

        public void OnEnable()
        {
            _mapItem = target as MapItem;
            DeletePendingGameObject();
        }
        void OnSceneGUI()
        {
            Event e = Event.current;
            if (e.type == EventType.KeyUp && FindObjectsOfType(typeof(MapItem)).Count() == _mapItem.Number)
            {
                if (e.keyCode == KeyCode.G)
                    AddAfter();

            }
        }

        public override void OnInspectorGUI()
        {
            GUILayout.BeginVertical("Box");
            GUILayout.Space(5);

            if (GUILayout.Button("Insert before"))
            {
                List<MapItem> mapLevels = GetMapLevels();
                int ind = mapLevels.IndexOf(_mapItem);
                InsertMapLevel(ind, mapLevels);
            }

            if (GUILayout.Button("Insert after"))
            {
                AddAfter();
            }

            if (GUILayout.Button("Delete"))
            {
                Delete();
            }

            UpdateSceneName();

            GUILayout.Space(5);
            GUILayout.EndVertical();

            base.OnInspectorGUI();
        }

        private void AddAfter()
        {
            List<MapItem> mapLevels = GetMapLevels();
            int ind = mapLevels.IndexOf(_mapItem);
            InsertMapLevel(ind + 1, mapLevels);
        }

        private void UpdateSceneName()
        {
            string oldSceneName = _mapItem.SceneName;
            string newSceneName = _mapItem.LevelScene == null ? null : _mapItem.LevelScene.name;
            if (oldSceneName != newSceneName)
            {
                _mapItem.SceneName = newSceneName;
                EditorUtility.SetDirty(_mapItem);
            }
        }

        private void InsertMapLevel(int ind, List<MapItem> mapLevels)
        {
            Vector2 position = GetInterpolatedPosition(ind, mapLevels);
            LevelsMap levelsMap = FindObjectOfType<LevelsMap>();
            MapItem mapItem = CreateMapLevel(position, ind, levelsMap.mapItemPrefab);
            mapItem.transform.parent = _mapItem.transform.parent;
            mapItem.transform.SetSiblingIndex(ind);
            mapLevels.Insert(ind, mapItem);
            UpdateLevelsNumber(mapLevels);
            UpdatePathWaypoints(mapLevels);
            SetStarsEnabled(levelsMap, levelsMap.StarsEnabled);
            Selection.activeGameObject = mapItem.gameObject;
        }

        private Vector2 GetInterpolatedPosition(int ind, List<MapItem> mapLevels)
        {
            Vector3 startPosition = mapLevels[Mathf.Max(0, ind - 1)].transform.position;
            Vector3 finishPosition = mapLevels[Mathf.Min(ind, mapLevels.Count - 1)].transform.position;

            if (ind == 0 && mapLevels.Count > 1)
                finishPosition = startPosition + (startPosition - mapLevels[1].transform.position);

            if (ind == mapLevels.Count && mapLevels.Count > 1)
                finishPosition = startPosition + (startPosition - mapLevels[ind - 2].transform.position);

            return (startPosition + finishPosition) / 2;
        }

        private void Delete()
        {
            List<MapItem> mapLevels = GetMapLevels();
            int ind = mapLevels.IndexOf(_mapItem);
            mapLevels.Remove(_mapItem);
            UpdateLevelsNumber(mapLevels);
            UpdatePathWaypoints(mapLevels);
            LevelsMap levelsMap = FindObjectOfType<LevelsMap>();
            Selection.activeGameObject =
                mapLevels.Any()
                    ? mapLevels[Mathf.Max(0, ind - 1)].gameObject
                    : levelsMap.gameObject;
            SetStarsEnabled(levelsMap, levelsMap.StarsEnabled);
            _pendingDeletedGameObject = _mapItem.gameObject;
        }

        private void DeletePendingGameObject()
        {
            if (_pendingDeletedGameObject != null)
            {
                DestroyImmediate(_pendingDeletedGameObject);
                _pendingDeletedGameObject = null;
            }
        }
    }
}
