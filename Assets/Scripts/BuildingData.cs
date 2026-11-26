using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Scriptable Objects/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public Vector2Int size = Vector2Int.one;
    public Color color =  Color.white;
    public ResourceAmount[] cost;
    
    public GameObject prefab;
    public float workTime = 3f;
    public ResourceAmount[] input;
    public ResourceAmount[] output;
}
