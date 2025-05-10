using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GeneralSettings", menuName = "Razhia/GeneralSettings", order = 0)]
public class GS : SingletonScriptableObject<GS>
{
    [TitleGroup("UI")] public float buttonsAnimateTime;
    public Ease buttonsOffEase;
    public Ease buttonsOnEase;
    public Ease fadeEase = Ease.OutQuint;
    
    [TitleGroup("Map")] public float cameraMoveToSelectedDuration;

    
    
    
    
    
    
    
    [TitleGroup("Temps")] public Ease TempEase;
    public float tempFloat;
    
    [TitleGroup("Debug")] public bool showDebugs;
    
    
    [PropertyTooltip("Tu halate test mode bazi dokme ha be onvane rad kardane marhale kar mikonan va tutorial ha neshun dade nemishan")]
    [TitleGroup("TestMode")]
    public bool isTestMode;


}