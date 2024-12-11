using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public event EventHandler<int> OnGoldChanged;
    public event EventHandler<int> OnStoneChanged;
    public event EventHandler<int> OnWoodChanged;

    public static ResourceManager Instance { get; private set; }

    [SerializeField]
    private List<Resource> resources;

    public ResourceManager()
    {
        resources = new List<Resource>()
        {
            new Resource(ResourceType.Gold, 100, 1000),
            new Resource(ResourceType.Stone, 100, 1000),
            new Resource(ResourceType.Wood, 100, 1000)
        };
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple ResourceManager instances found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

        public Resource GetResource(ResourceType resourceType)
    {
        return resources.Find(r => r.resourceType == resourceType);
    }

    public void AddResource(ResourceType resourceType, int amount)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].resourceType == resourceType)
            {
                Resource resource = resources[i];
                resource.Add(amount);
                resources[i] = resource;

                TriggerResourceChangedEvent(resourceType, resource.amount);
                return;
            }
        }
    }

    public void RemoveResource(ResourceType resourceType, int amount)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (resources[i].resourceType == resourceType)
            {
                Resource resource = resources[i];
                resource.Remove(amount);
                resources[i] = resource;

                TriggerResourceChangedEvent(resourceType, resource.amount);
                return;
            }
        }
    }

    public int GetResourceAmount(ResourceType resourceType)
    {
        Resource resource = GetResource(resourceType);
        return resource != null ? resource.amount : 0;
    }

    private void TriggerResourceChangedEvent(ResourceType resourceType, int newAmount)
    {
        switch (resourceType)
        {
            case ResourceType.Gold:
                OnGoldChanged?.Invoke(this, newAmount);
                break;
            case ResourceType.Stone:
                OnStoneChanged?.Invoke(this, newAmount);
                break;
            case ResourceType.Wood:
                OnWoodChanged?.Invoke(this, newAmount);
                break;
        }
    }
}
