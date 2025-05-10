using Joyixir.GameManager.UI;
using UnityEngine;

public class MapView : View
{

    public void DoStuff()
    {
        Debug.Log("Doing Stuff");
    }
    protected override void OnBackBtn()
    {
    }

    public void Initialize(RacingLevelConfig selectedLevelConfig)
    {
        Debug.Log(selectedLevelConfig.levelPrize);
    }
}
