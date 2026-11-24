using UnityEngine;

public class BuildManager : MonoBehaviour
{
    
    [SerializeField] private GridManager gridManager;
    [SerializeField] private Transform ghost;
    [SerializeField] private float ghostSmoothing = 15f;
    


    void Update()
    {
        if (gridManager.TryGetMouseCell(out Vector2Int cell))
        {
            ghost.gameObject.SetActive(true);
            
            Vector3 target = gridManager.GetCellCenter(cell);
            
            ghost.position = Vector3.Lerp(ghost.position, target, ghostSmoothing * Time.deltaTime);
        } else 
        {
            ghost.gameObject.SetActive(false);
        }
    }
}
