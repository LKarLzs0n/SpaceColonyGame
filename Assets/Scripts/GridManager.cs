using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cellSize = 1f;
    
    private Dictionary<Vector2Int, GameObject> occupiedCells = new Dictionary<Vector2Int, GameObject>();
    
    public Vector3 GetCellCenter(Vector2Int cell)
    {
        return new Vector3(
            cell.x * cellSize + cellSize / 2f,
            cellSize / 2f,
            cell.y * cellSize + cellSize / 2f);
    }

    public bool IsCellFree(Vector2Int cell)
    {
        return !occupiedCells.ContainsKey(cell);
    }
     
    public void OccupyCell(Vector2Int cell, GameObject building)
    {
        occupiedCells.Add(cell, building);
    }
     
    public bool TryGetBuilding(Vector2Int cell, out GameObject building)
    {
        return occupiedCells.TryGetValue(cell, out building);
    }
     
    public void FreeCell(Vector2Int cell)
    {
        occupiedCells.Remove(cell);
    }
    
    public bool TryGetMouseCell(out Vector2Int cell)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) { cell = default; return false; }

        Vector3 point = hit.point - hit.normal * 0.01f;
        
        int x = Mathf.FloorToInt(point.x / cellSize);
        int z = Mathf.FloorToInt(point.z / cellSize);
        cell = new Vector2Int(x, z);
        return true;
    }
    
}
