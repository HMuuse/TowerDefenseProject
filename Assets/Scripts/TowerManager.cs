using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public event EventHandler<int> OnTowerPlaced;

    public static TowerManager Instance { get; private set; }

    [SerializeField]
    private TowerSO towerSO;
    [SerializeField]
    private float placementRadius = 1f;
    [SerializeField]
    private LayerMask placementLayer;

    [SerializeField]
    private GameObject placementVisual;

    private bool isPlacing = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple TowerManager instances found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isPlacing)
        {

            placementVisual.SetActive(true);
            float yOffset = 2.55f;
            placementVisual.transform.position = MouseWorld.GetPosition() + new Vector3(0, yOffset, 0);

            if (Input.GetMouseButtonDown(0) && GameManager.Instance.GetScore() >= towerSO.towerCost)
            {
                Vector3 placementPosition = MouseWorld.GetPosition();

                if (IsPlaceable(placementPosition))
                {
                    Instantiate(towerSO.prefab, placementPosition, Quaternion.identity);
                    OnTowerPlaced?.Invoke(this, towerSO.towerCost);
                }
                else
                {
                    Debug.Log("Cannot place tower here: position is occupied.");
                }
            }
        }
        else
        {
            placementVisual.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Tab)) 
        {
            isPlacing = !isPlacing;
        }
    }

    private bool IsPlaceable(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, placementRadius, placementLayer);
        return hitColliders.Length == 0;
    }
}

