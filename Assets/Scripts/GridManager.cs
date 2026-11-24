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
    }

    void PlaceCube()
    {
        
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        
        RaycastHit hit;
        if (!Physics.Raycast(ray, out hit)) return;
        
        int x = Mathf.FloorToInt(hit.point.x / cellSize);
        int z = Mathf.FloorToInt(hit.point.z / cellSize);
        Vector2Int cell = new Vector2Int(x, z);

        if (occupiedCells.ContainsKey(cell)) return;

        Vector3 position = new Vector3(
            x * cellSize + cellSize / 2f,
            cellSize / 2f,
            z * cellSize + cellSize / 2f);
        
        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
        occupiedCells.Add(cell, cube);

    }
    
}
