using SweetSugar.Scripts.Level;
using SweetSugar.Scripts.MapScripts;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapManager : Singleton<MapManager>
{
    void Awake()
    {
        Application.targetFrameRate = 60;
        // Application.runInBackground = true;
    }


    void OnEnable()
    {
        LevelsMap.LevelSelected += OnLevelClicked;
        LevelsMap.OnLevelReached += OnLevelReached;
    }

    void OnDisable()
    {
        LevelsMap.LevelSelected -= OnLevelClicked;
        LevelsMap.OnLevelReached -= OnLevelReached;
    }

    public void SaveLevelStarsCount(int level, int starsCount)
    {
        Debug.Log(string.Format("Stars count {0} of level {1} saved.", starsCount, level));
        PlayerPrefs.SetInt(GetLevelKey(level), starsCount);
    }

    private string GetLevelKey(int number)
    {
        return string.Format("Level.{0:000}.StarsCount", number);
    }


    public void OnLevelClicked(object sender, LevelReachedEventArgs args)
    {
        if (EventSystem.current.IsPointerOverGameObject(-1))
            return;
    }


    void OnLevelReached()
    {
        var num = PlayerPrefs.GetInt("OpenLevel");
        if (CrosssceneData.openNextLevel && CrosssceneData.totalLevels >= num)
        {
            OpenMenuPlay(num);
        }
    }

    public static void OpenMenuPlay(int num)
    {
        PlayerPrefs.SetInt("OpenLevel", num);
        PlayerPrefs.Save();
        CrosssceneData.openNextLevel = false;
    }
}