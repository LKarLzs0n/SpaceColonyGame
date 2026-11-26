using UnityEngine;

public class House : Building
{

    [SerializeField] private Worker workerPrefab;
    [SerializeField] private int colonists = 2;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < colonists; i++)
        {
            Worker w = Instantiate(workerPrefab, GetDoorPoint(), Quaternion.identity);
            w.colony = colony;
            w.stockpile = stockpile;
            w.home = this;
        }
    }

    Vector3 GetDoorPoint()
    {
        Vector3 outside = transform.position + transform.forward * 5f;
        return GetNearestPoint(outside) + transform.forward * 0.5f;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
