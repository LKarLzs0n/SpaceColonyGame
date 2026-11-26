using UnityEngine;

public class ProductionBuilding : Building
{

    private float timer;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer < data.workTime) {return;}

        if (!stockpile.CanAfford(data.input))
        {
            return;
        }
        
        stockpile.Spend(data.input);
        stockpile.AddAll(data.output);
        timer = 0;
    }
}
