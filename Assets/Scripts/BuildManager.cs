using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    [SerializeField] private ColonyManager colony;
    [SerializeField] private Stockpile stockpile;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform ghost;
    [SerializeField] private float ghostSmoothing = 15f;

    private BuildingData selectedBuilding;
    [SerializeField] private BuildingData[] buildingCards;
    
    private int rotationSteps;

    void Start()
    {
        SelectBuilding(0);
    }
    
    void PlaceBuilding(Vector2Int cell)
    {
        Vector2Int size = GetFootprintSize();
        
        if (!gridManager.IsAreaFree(cell, size)) return;
        
        if (!stockpile.CanAfford(selectedBuilding.cost)) return;
        
        Vector3 position = gridManager.GetFootprintCenter(cell, size);
        
        GameObject building = Instantiate(selectedBuilding.prefab, position, GetRotation());
        building.name = selectedBuilding.buildingName;
        building.transform.localScale = new Vector3(selectedBuilding.size.x, 1f, selectedBuilding.size.y);
        building.GetComponent<Renderer>().material.color = selectedBuilding.color;
        Building b = building.GetComponent<Building>();
        b.data = selectedBuilding;
        b.stockpile = stockpile;
        b.colony = colony;
        b.corner = cell;
        b.size = size;
        gridManager.OccupyArea(cell, size, building);
        stockpile.Spend(selectedBuilding.cost);
    }

    void RemoveBuilding(Vector2Int cell)
    {
        if (gridManager.TryGetBuilding(cell, out GameObject building))
        {
            Building b = building.GetComponent<Building>();
            gridManager.FreeArea(b.corner, b.size);
            stockpile.AddAll(b.data.cost);
            Destroy(building);
        }
    }

    void SelectBuilding(int index)
    {
        if (index >= buildingCards.Length)  return;
        selectedBuilding = buildingCards[index];
        Debug.Log("Selected " + selectedBuilding.buildingName);
    }

    Vector2Int GetFootprintSize()
    {
        Vector2Int size = selectedBuilding.size;

        if (rotationSteps % 2 == 1)
        {
            return new Vector2Int(size.y, size.x);
        }

        return size;
    }

    Quaternion GetRotation()
    {
        return Quaternion.Euler(0f, rotationSteps * 90f, 0f);
    }
    
    
    void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            rotationSteps = (rotationSteps + 1) % 4;
        }
        
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SelectBuilding(0);
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SelectBuilding(1);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            SelectBuilding(2);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            SelectBuilding(3);
        }
        
        
        if (gridManager.TryGetMouseCell(out Vector2Int cell))
        {
            ghost.gameObject.SetActive(true);

            Vector2Int corner = cell - GetFootprintSize() / 2;
            
            Vector3 target = gridManager.GetFootprintCenter(corner, GetFootprintSize());

            ghost.localScale = new Vector3(selectedBuilding.size.x, 1f, selectedBuilding.size.y) * 1.02f;
            ghost.position = Vector3.Lerp(ghost.position, target, ghostSmoothing * Time.deltaTime);
            ghost.rotation = Quaternion.Slerp(ghost.rotation, GetRotation(), ghostSmoothing * Time.deltaTime);
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                PlaceBuilding(corner);
            }

            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                RemoveBuilding(cell);
            }
        } else 
        {
            ghost.gameObject.SetActive(false);
        }
    }
}
