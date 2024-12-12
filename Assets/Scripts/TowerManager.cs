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
    private GameObject upgradeText;
    [SerializeField]
    private int upgradeCost;

    private bool isPlacing = false;
    private bool isUpgrading = false;
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

        if (isUpgrading && !isPaused)
        {
            upgradeText.SetActive(true);

            if (Input.GetMouseButtonDown(0) && CanAffordTower())
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    GameObject hitObject = hitInfo.collider.gameObject;
                    if (hitObject.CompareTag("Tower"))
                    {
                        if (hitObject.TryGetComponent<BaseTower>(out var towerScript))
                        {
                            if (!towerScript.IsMaxLevel())
                            {
                                towerScript.UpgradeTower(10, 2f, 0.5f);
                                DeductResources();
                                Debug.Log("Tower upgraded successfully!");
                            }
                            else
                            {
                                Debug.Log("Tower is already at maximum level.");
                            }
                        }
                        else
                        {
                            Debug.Log("No BaseTower script found on the hit object.");
                        }
                    }
                    else
                    {
                        Debug.Log("Hit object is not a Tower.");
                    }
                }
            }
        }
        else
        {
            upgradeText.SetActive(false);
        }


        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (!isUpgrading)
            {
                isPlacing = !isPlacing;
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isPlacing)
            {
                isUpgrading = !isUpgrading;
            }
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
            if (ResourceManager.Instance.GetResourceAmount(resourceCost.resourceType) < resourceCost.cost)
            {
                return false;
            }
        }

        return true;
    }

    private void DeductResources()
    {
        if (isPlacing)
        {
            foreach (ResourceCost resourceCost in currentTowerSO.resourceCosts)
            {
                ResourceManager.Instance.RemoveResource(resourceCost.resourceType, resourceCost.cost);
            }
        }
        if (isUpgrading)
        {
            ResourceManager.Instance.RemoveResource(ResourceType.Gold, upgradeCost);
        }
    }
}

