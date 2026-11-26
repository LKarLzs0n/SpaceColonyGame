using System.Collections.Generic;
using UnityEngine;

public class ColonyManager : MonoBehaviour
{

    private List<ProductionBuilding> workplaces = new List<ProductionBuilding>();
    
    public void AddWorkplace(ProductionBuilding workplace)
    {
        workplaces.Add(workplace);
    }

    public void RemoveWorkplace(ProductionBuilding workplace)
    {
        workplaces.Remove(workplace);
    }

    public ProductionBuilding FindFreeWorkplace(Vector3 from)
    {
        ProductionBuilding nearest = null;
        float neareswtDistance = Mathf.Infinity;

        foreach (ProductionBuilding workplace in workplaces)
        {
            if (workplace.worker != null) continue;

            float distance = Vector3.Distance(from, workplace.transform.position);

            if (distance < neareswtDistance)
            {
                nearest = workplace;
                neareswtDistance = distance;
            }
        }
        return nearest;
    }
    
}
