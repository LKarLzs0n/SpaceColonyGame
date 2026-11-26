using UnityEngine;

public class ProductionBuilding : Building
{
    
    public Worker worker;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colony.AddWorkplace(this);
    }
    

    void OnDestroy()
    {
        colony.RemoveWorkplace(this);
    }
    
}
