using UnityEngine;

[CreateAssetMenu(fileName = "Razhia", menuName = "Razhia/Levels/RacingLevelData")]
public class RacingLevelConfig : ScriptableObject
{
    public Common.levelType levelType;
    public int levelPrize;
}
