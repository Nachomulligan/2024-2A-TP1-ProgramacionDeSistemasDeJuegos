using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingAliveService : MonoBehaviour
{
    // list to keep track of active buildings
    private List<Building> activeBuildings = new List<Building>();

    private void Awake()
    {
        ServiceLocator.Instance.SetService("BuildingAliveService", this);
    }

    public void RegisterBuilding(Building building)
    {
        //register building if its not registerd
        if (!activeBuildings.Contains(building))
        {
            activeBuildings.Add(building);
        }
    }

    public void UnregisterBuilding(Building building)
    {
        //unregister building if its registerd
        if (activeBuildings.Contains(building))
        {
            activeBuildings.Remove(building);
        }
    }

    public Building GetClosestBuilding(Vector3 position)
    {
        // finds and returns the closest building to a given position
        Building closestBuilding = null;
        float closestDistance = Mathf.Infinity; // maximum possible distance

        foreach (var building in activeBuildings)
        {
            float distance = Vector3.Distance(position, building.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestBuilding = building; // update the closest building
            }
        }

        return closestBuilding;
    }
}