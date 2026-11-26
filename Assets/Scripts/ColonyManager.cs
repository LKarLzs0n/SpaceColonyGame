using System.Collections.Generic;
using UnityEngine;

public class ColonyManager : MonoBehaviour
{

    private List<ProductionBuilding> workplaces = new List<ProductionBuilding>();
    private List<Gatherable> markedGatherables = new List<Gatherable>();
    
    public void AddMarked(Gatherable g) {markedGatherables.Add(g);}
    
    public void RemoveMarked(Gatherable g) {markedGatherables.Remove(g);}

    public Gatherable FindNearestMarked(Vector3 from)
    {
        Gatherable nearest = null;
        float neareswtDistance = Mathf.Infinity;

        foreach (Gatherable g in markedGatherables)
        {
            if (g.carrier != null) continue;

            float distance = Vector3.Distance(from, g.transform.position);

            if (distance < neareswtDistance)
            {
                nearest = g;
                neareswtDistance = distance;
            }
        }
        return nearest;
    }
    
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
