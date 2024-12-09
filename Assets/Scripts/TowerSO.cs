using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerSO", menuName = "Scriptable Objects/ new TowerSO")]
public class TowerSO : ScriptableObject
{
    public GameObject prefab;
    public int towerCost;

}
