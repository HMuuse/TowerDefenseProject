using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerSO", menuName = "Scriptable Objects/ new TowerSO")]
public class TowerSO : ScriptableObject
{
    public GameObject prefab;
    public List<ResourceCost> resourceCosts;

}

[System.Serializable]
public class  ResourceCost
{
    public ResourceType resourceType;
    public int cost;
}
