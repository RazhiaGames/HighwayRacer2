using Joyixir.GameManager.UI;
using UnityEngine;

public class MonoUtils : MonoBehaviour
{

    #region From City Builder

    public static GameObject CreateInstance(GameObject original, GameObject parent, bool isActive)
    {
        GameObject instance = Instantiate(original, parent.transform, false);
        instance.SetActive(isActive);
        return instance;
    }
        
    public static View CreateUIInstance(View original, GameObject parent, bool isActive)
    {
        View instance = Instantiate(original, parent.transform, false);
        instance.gameObject.SetActive(isActive);
        return instance;
    }
    #endregion
}
