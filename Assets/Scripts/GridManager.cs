using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private GameObject cubePrefab;
    
    private Dictionary<Vector2Int, GameObject> occupiedCells = new Dictionary<Vector2Int, GameObject>();
    

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            PlaceCube();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            RemoveCube();
        }
    }

    void PlaceCube()
    {

        if (!TryGetMouseCell(out Vector2Int cell)) return;

        if (occupiedCells.ContainsKey(cell)) return;
        
        Vector3 position = GetCellCenter(cell);
        
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        occupiedCells.Add(cell, cube);

    }

    void RemoveCube()
    {
        if (!TryGetMouseCell(out Vector2Int cell)) return;
        if (occupiedCells.TryGetValue(cell, out GameObject cube))
        {
            Destroy(cube);
            occupiedCells.Remove(cell);
        }
    }

    public Vector3 GetCellCenter(Vector2Int cell)
    {
        return new Vector3(
            cell.x * cellSize + cellSize / 2f,
            cellSize / 2f,
            cell.y * cellSize + cellSize / 2f);
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
