using UnityEngine;

public class StorageDepot : Building
{
    void Start() {stockpile.AddDepot(this);}
    
    void OnDestroy() {stockpile.RemoveDepot(this);}
}
