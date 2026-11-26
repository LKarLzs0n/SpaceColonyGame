using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData data;
    public Vector2Int corner;
    public Vector2Int size;
    public Stockpile stockpile;
    public ColonyManager colony;

    public Vector3 GetNearestPoint(Vector3 from)
    {
        return GetComponent<Collider>().ClosestPoint(from);
    }
    
    
}
