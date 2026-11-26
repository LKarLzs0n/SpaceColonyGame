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

    public Vector3 GetFootprintCenter(Vector2Int corner, Vector2Int size)
    {
        return new Vector3(
            corner.x * cellSize + size.x * cellSize / 2f,
            cellSize / 2f,
            corner.y * cellSize + size.y * cellSize / 2f);
    }

    public void FreeArea(Vector2Int corner, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int c = corner + new Vector2Int(x, z);
                FreeCell(c);
            }
        }
    }

    public bool IsAreaFree(Vector2Int corner, Vector2Int size)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int c = corner + new Vector2Int(x, z);
                
                if (!IsCellFree(c)) { return false; }
            }
        }
        return true;
    }

    public void OccupyArea(Vector2Int corner, Vector2Int size, GameObject building)
    {
        for (int x = 0; x < size.x; x++)
        {
            for (int z = 0; z < size.y; z++)
            {
                Vector2Int c = corner + new Vector2Int(x, z);
                OccupyCell(c, building);
            }
        }
    }
    
    public void FreeCell(Vector2Int cell)
    {
        occupiedCells.Remove(cell);
    }

    public bool TryGetMouseHit(out RaycastHit hit)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        return Physics.Raycast(ray, out hit);
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
