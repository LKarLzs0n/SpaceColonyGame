using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour
{
    
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform ghost;
    [SerializeField] private float ghostSmoothing = 15f;
    [SerializeField] private GameObject buildingPrefab;


    void PlaceBuilding(Vector2Int cell)
    {
        if (!gridManager.IsCellFree(cell)) return;
        
        Vector3 position = gridManager.GetCellCenter(cell);
        
        GameObject building = Instantiate(buildingPrefab, position, Quaternion.identity);
        gridManager.OccupyCell(cell, building);
    }

    void RemoveBuilding(Vector2Int cell)
    {
        if (gridManager.TryGetBuilding(cell, out GameObject building))
        {
            Destroy(building);
            gridManager.FreeCell(cell);
        }
    }

    void Update()
    {
        
        if (gridManager.TryGetMouseCell(out Vector2Int cell))
        {
            ghost.gameObject.SetActive(true);
            
            Vector3 target = gridManager.GetCellCenter(cell);

            ghost.position = Vector3.Lerp(ghost.position, target, ghostSmoothing * Time.deltaTime);
            
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                PlaceBuilding(cell);
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
