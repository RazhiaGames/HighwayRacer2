using System;
using System.Collections.Generic;
using System.Linq;
using SweetSugar.Scripts.Level;
using SweetSugar.Scripts.System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace SweetSugar.Scripts.MapScripts
{
    public class LevelsMap : Singleton<LevelsMap>
    {
        public static IMapProgressManager _mapProgressManager = new PlayerPrefsMapProgressManager();

        public bool IsGenerated;

        [FormerlySerializedAs("MapLevelPrefab")]
        public MapItem mapItemPrefab;

        public Transform CharacterPrefab;
        public int Count = 10;

        public WaypointsMover WaypointsMover;
        public MapItem selectedItem;
        public MapItem currentClickedItem;
        public MapItem prevClickedItem; 

        public TranslationType TranslationType;

        public bool StarsEnabled;
        public StarsType StarsType;

        public bool ScrollingEnabled;
        public bool IsClickEnabled;


        public void OnEnable()
        {
            if (IsGenerated)
            {
                Reset();
            }
        }


        public static List<MapItem> GetMapLevels()
        {
            List<MapItem> MapLevels = new List<MapItem>();
            if (MapLevels.Count == 0) //1.4.4
                MapLevels = FindObjectsOfType<MapItem>().OrderBy(ml => ml.Number).WhereNotNull().ToList();

            return MapLevels;
        }

        public void Reset()
        {
            UpdateMapLevels();
            PlaceCharacterToLastUnlockedLevel();
            int number = GetLastestReachedLevel();
            if (number > 1 && CrosssceneData.win)
                WalkToLevelInternal(number);
            else TeleportToLevelInternal(number, true);
            SetCameraToCharacter();
        }

        private void UpdateMapLevels()
        {
            foreach (MapItem mapLevel in GetMapLevels())
            {
                mapLevel.UpdateState(
                    _mapProgressManager.LoadLevelStarsCount(mapLevel.Number),
                    IsLevelLocked(mapLevel.Number));
            }
        }

        private void PlaceCharacterToLastUnlockedLevel()
        {
            int lastUnlockedNumber = GetMapLevels().Where(l => !l.IsLocked).Select(l => l.Number).Max() - 1;
            lastUnlockedNumber = Mathf.Clamp(lastUnlockedNumber, 1, lastUnlockedNumber);
            // TeleportToLevelInternal(lastUnlockedNumber, true);
        }

        public static int GetLastestReachedLevel()
        {
            //1.3.3
            return GetMapLevels().Where(l => !l.IsLocked).Select(l => l.Number).Max();
        }

        private void SetCameraToCharacter()
        {
            MapCameraManager.Instance.ZoomInToSelected(WaypointsMover.transform);
        }

        #region Events

        public static event EventHandler<LevelReachedEventArgs> MapItemSelected;
        public static event EventHandler<LevelReachedEventArgs> MapItemClicked;
        public static event EventHandler<LevelReachedEventArgs> MapItemReached;

        #endregion

        #region Static API

        public static void CompleteLevel(int number)
        {
            CompleteLevelInternal(number, 1);
        }

        public static void CompleteLevel(int number, int starsCount)
        {
            CompleteLevelInternal(number, starsCount);
        }

        internal static void OnLevelSelected(int number)
        {
            if (MapItemSelected != null)
            {

                if (!IsLevelLocked(number))
                {
                    Instance.selectedItem = Instance.GetLevel(number);
                }

                MapItemSelected(Instance, new LevelReachedEventArgs(number, IsLevelLocked(number)));
            }

            InvokeLevelClicked(Instance.GetLevel(number));

        }

        public static void GoToLevel(int number)
        {
            switch (Instance.TranslationType)
            {
                case TranslationType.Teleportation:
                    Instance.TeleportToLevelInternal(number, false);
                    break;
                case TranslationType.Walk:
                    Instance.WalkToLevelInternal(number);
                    break;
            }
        }

        public static bool IsLevelLocked(int number)
        {
            return number > 1 && _mapProgressManager.LoadLevelStarsCount(number - 1) == 0;
        }

        public static void OverrideMapProgressManager(IMapProgressManager mapProgressManager)
        {
            _mapProgressManager = mapProgressManager;
        }

        public static void ClearAllProgress()
        {
            Instance.ClearAllProgressInternal();
        }

        public static bool IsStarsEnabled()
        {
            return Instance.StarsEnabled;
        }

        public static bool GetIsClickEnabled()
        {
            return Instance.IsClickEnabled;
        }

        #endregion

        private static void CompleteLevelInternal(int number, int starsCount)
        {
            if (IsLevelLocked(number))
            {
                Debug.Log(string.Format("Can't complete locked level {0}.", number));
            }
            else if (starsCount < 1 || starsCount > 3)
            {
                Debug.Log(string.Format("Can't complete level {0}. Invalid stars count {1}.", number, starsCount));
            }
            else
            {
                int curStarsCount = _mapProgressManager.LoadLevelStarsCount(number);
                int maxStarsCount = Mathf.Max(curStarsCount, starsCount);
                _mapProgressManager.SaveLevelStarsCount(number, maxStarsCount);

                if (Instance != null)
                    Instance.UpdateMapLevels();
            }
        }

        private void TeleportToLevelInternal(int number, bool isQuietly)
        {
            Debug.Log("TeleportToLevelInternal");
            MapItem mapItem = GetLevel(number);
            mapItem.SetEffect();
            if (mapItem.IsLocked)
            {
                Debug.Log(string.Format("Can't jump to locked level number {0}.", number));
            }
            else
            {
                WaypointsMover.transform.position =
                    mapItem.PathPivot.transform.position; //need to fix in the map plugin
                selectedItem = mapItem;
                InvokeLevelClicked(mapItem);
                if (!isQuietly)
                    RaiseLevelReached(number);
            }
        }

        public delegate void ReachedLevelEvent();

        public static ReachedLevelEvent OnLevelReached;

        private void WalkToLevelInternal(int number)
        {
            MapItem mapItem = GetLevel(number);
            mapItem.SetEffect();
            selectedItem = GetLevel(number - 1);

            if (mapItem.IsLocked)
            {
                Debug.Log(string.Format("Can't go to locked level number {0}.", number));
            }
            else
            {
                WaypointsMover.Move(selectedItem.PathPivot, mapItem.PathPivot,
                    () =>
                    {
                        RaiseLevelReached(number);
                        selectedItem = mapItem;
                        InvokeLevelClicked(selectedItem);
                        OnLevelReached?.Invoke();
                    });
            }
        }

        private static void InvokeLevelClicked(MapItem currentItem)
        {
            Instance.prevClickedItem = Instance.currentClickedItem;
            Instance.currentClickedItem = currentItem;
            MapItemClicked?.Invoke(Instance, new LevelReachedEventArgs(currentItem.Number, IsLevelLocked(currentItem.Number)));
        }

        private void RaiseLevelReached(int number)
        {
            MapItem mapItem = GetLevel(number);
            mapItem.SetEffect();

            if (MapItemReached != null)
            {
                MapItemReached(this, new LevelReachedEventArgs(number, IsLevelLocked(number)));
                InvokeLevelClicked(Instance.GetLevel(number));
            }
        }

        public MapItem GetLevel(int number)
        {
            return GetMapLevels().SingleOrDefault(ml => ml.Number == number);
        }

        private void ClearAllProgressInternal()
        {
            foreach (MapItem mapLevel in GetMapLevels())
                _mapProgressManager.ClearLevelProgress(mapLevel.Number);
            Reset();
        }

        public void SetStarsEnabled(bool bEnabled)
        {
            StarsEnabled = bEnabled;
            int starsCount = 0;
            foreach (MapItem mapLevel in GetMapLevels().WhereNotNull())
            {
                mapLevel.UpdateStars(starsCount);
                starsCount = (starsCount + 1) % 4;
                mapLevel.StarsHoster.gameObject.SetActive(bEnabled);
                //mapLevel.SolidStarsHoster.gameObject.SetActive(bEnabled);
            }
        }

        public void SetStarsType(StarsType starsType)
        {
            StarsType = starsType;
            foreach (MapItem mapLevel in GetMapLevels().WhereNotNull())
                mapLevel.UpdateStarsType(starsType);
        }
    }
}