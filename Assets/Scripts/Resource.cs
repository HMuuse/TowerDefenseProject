using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceType
{
    Gold,
    Wood,
    Stone
}

[Serializable]
public struct Resource : IEquatable<Resource>
{
    public ResourceType resourceType;
    public int amount;
    public int maxAmount;

    public Resource(ResourceType resourceType, int initialAmount, int maxAmount)
    {
        this.resourceType = resourceType;
        this.amount = initialAmount;
        this.maxAmount = maxAmount;
    }

    public void Add(int amountToBeAdded)
    {
        amount += Mathf.Min(amountToBeAdded, maxAmount - amount);
    }

    public void Remove(int amountToBeRemoved)
    {
        amount = Mathf.Max(0, amount - amountToBeRemoved);
    }

    public int GetAmount()
    {
        return amount;
    }

    public bool Equals(Resource other)
    {
        return resourceType == other.resourceType &&
               amount == other.amount &&
               maxAmount == other.maxAmount;
    }

    // Override GetHashCode for hash-based collections
    public override int GetHashCode()
    {
        return HashCode.Combine(resourceType, amount, maxAmount);
    }

    // Define equality operator
    public static bool operator ==(Resource left, Resource right)
    {
        return left.Equals(right);
    }

    // Define inequality operator
    public static bool operator !=(Resource left, Resource right)
    {
        return !(left == right);
    }
}
