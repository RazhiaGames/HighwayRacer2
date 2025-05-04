using Sirenix.OdinInspector;
using UnityEngine;

public class Toggler : MonoBehaviour
{
    [SerializeField] private GameObject low;
    [SerializeField] private GameObject high;

    [SerializeField] private bool t_bool;
    [Button]
    public void ToggleTheThing()
    {
        low.SetActive(t_bool);
        high.SetActive(!t_bool);

        t_bool = !t_bool;
    }

    public void SetGameObjects(GameObject go1, GameObject go2)
    {
        low = go1;
        high = go2;
    }
}
