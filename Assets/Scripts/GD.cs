using UnityEngine;

[CreateAssetMenu(fileName = "GD", menuName = "Razhia/GameDesign", order = 0)]
public class GD : SingletonScriptableObject<GD>
{
    public float damageToFinishGame = 50;

}