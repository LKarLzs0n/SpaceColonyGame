using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Stockpile : MonoBehaviour
{
    
    private Dictionary<ResourceData, int> amounts = new Dictionary<ResourceData, int>();

    [SerializeField] private ResourceData[] debugResources;
    
    public int GetAmount(ResourceData resource)
    {
        if (amounts.TryGetValue(resource, out int amount))
        {
            return amount;
        }
        return 0;
    }

    public void Add(ResourceData resource, int amount)
    {
        amounts[resource] = GetAmount(resource) + amount;
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            foreach (ResourceData resource in debugResources)
            {
                Add(resource, 10);
            }
        }
    }

    public bool CanAfford(ResourceAmount[] cost)
    {
        foreach (ResourceAmount item in cost)
        {
            if (GetAmount(item.resource) < item.amount)
            {
                return false;
            }
        }

        return true;
    }

    public void Spend(ResourceAmount[] cost)
    {
        foreach (ResourceAmount item in cost)
        {
            Add(item.resource, -item.amount);
        }
    }
    
    public void AddAll(ResourceAmount[] cost)
    {
        foreach (ResourceAmount item in cost)
        {
            Add(item.resource, item.amount);
        }
    }

    void OnGUI()
    {
        GUI.skin.label.fontSize = 24;
        foreach (KeyValuePair<ResourceData, int> pair in amounts)
        {
            GUILayout.Label(pair.Key.resourceName + ": " + pair.Value);
        }
    }
}
