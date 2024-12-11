using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public event EventHandler<int> OnTowerPlaced;

    public static TowerManager Instance { get; private set; }

    [SerializeField]
    private List<TowerSO> towerSOs;
    [SerializeField]
    private TowerSO currentTowerSO;
    [SerializeField]
    private float placementRadius = 1f;
    [SerializeField]
    private LayerMask placementLayer;

    [SerializeField]
    private GameObject placementVisual;
    [SerializeField]
    private ResourceManager resourceManager;

    private bool isPlacing = false;
    private bool isPaused = false;

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

    private void Start()
    {
        GameManager.Instance.OnStateChanged += GameManager_OnStateChanged;

        currentTowerSO = towerSOs[0];
    }

    private void Update()
    {
        if (isPlacing && !isPaused)
        {

            placementVisual.SetActive(true);
            float yOffset = 2.55f;
            placementVisual.transform.position = MouseWorld.GetPosition() + new Vector3(0, yOffset, 0);

            if (Input.GetMouseButtonDown(0) && CanAffordTower())
            {
                Vector3 placementPosition = MouseWorld.GetPosition();

                if (IsPlaceable(placementPosition))
                {
                    Instantiate(currentTowerSO.prefab, placementPosition, Quaternion.identity);
                    DeductResources();
                    OnTowerPlaced?.Invoke(this, currentTowerSO.resourceCosts.Count);
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

    private void GameManager_OnStateChanged(GameManager.GameState obj)
    {
        isPaused = !isPaused;
    }

    private bool IsPlaceable(Vector3 position)
    {
        Collider[] hitColliders = Physics.OverlapSphere(position, placementRadius, placementLayer);
        return hitColliders.Length == 0;
    }

    private bool CanAffordTower()
    {
        foreach (ResourceCost resourceCost in currentTowerSO.resourceCosts)
        {
            if (resourceManager.GetResourceAmount(resourceCost.resourceType) < resourceCost.cost)
            {
                return false;
            }
        }

        return true;
    }

    private void DeductResources()
    {
        foreach (ResourceCost resourceCost in currentTowerSO.resourceCosts)
        {
            resourceManager.RemoveResource(resourceCost.resourceType, resourceCost.cost);
        }
    }
}

