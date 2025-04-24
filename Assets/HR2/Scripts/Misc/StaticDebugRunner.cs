using UnityEngine;

public class StaticDebugRunner : MonoBehaviour
{
    private static StaticDebugRunner _instance;

    public static StaticDebugRunner Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("StaticDebugRunner");
                _instance = go.AddComponent<StaticDebugRunner>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }
}
