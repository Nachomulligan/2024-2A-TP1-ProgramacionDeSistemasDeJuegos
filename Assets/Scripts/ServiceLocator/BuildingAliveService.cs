using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAliveService : MonoBehaviour
{
    private List<Building> activeBuildings = new List<Building>();

    private void Awake()
    {
        ServiceLocator.Instance.SetService("BuildingAliveService", this);
    }

    public void RegisterBuilding(Building building)
    {
        if (!activeBuildings.Contains(building))
        {
            activeBuildings.Add(building);
        }
    }

    public void UnregisterBuilding(Building building)
    {
        if (activeBuildings.Contains(building))
        {
            activeBuildings.Remove(building);
        }
    }

    public Building GetClosestBuilding(Vector3 position)
    {
        Building closestBuilding = null;
        float closestDistance = Mathf.Infinity;

        foreach (var building in activeBuildings)
        {
            float distance = Vector3.Distance(position, building.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBuilding = building;
            }
        }

        return closestBuilding;
    }
}