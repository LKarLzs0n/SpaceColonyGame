using UnityEngine;

public class Gatherable : MonoBehaviour
{
    public ResourceAmount contents;
    public bool marked;
    private Color startColor;
    public Worker carrier;

    private void Awake()
    {
        startColor = GetComponent<Renderer>().material.color;
    }

    public Vector3 GetNearestPoint(Vector3 from)
    {
        return GetComponent<Collider>().ClosestPoint(from);
    }
    
    public void ToggleMark()
    {
        marked = !marked;
        if (marked)
        {
            GetComponent<Renderer>().material.color = Color.yellow;
        }
        else
        {
            GetComponent<Renderer>().material.color = startColor;
        }
    }
}
