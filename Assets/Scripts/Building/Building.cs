using HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private HealthComponent healthComponent;
    public float respawnTime = 5f;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
    }

    private void OnEnable()
    {
        healthComponent.OnDeath += HandleDeath;

        var buildingAliveService = ServiceLocator.Instance.GetService("BuildingAliveService") as BuildingAliveService;
        if (buildingAliveService != null)
        {
            buildingAliveService.RegisterBuilding(this);
        }
    }

    private void OnDisable()
    {
        healthComponent.OnDeath -= HandleDeath;

        var buildingAliveService = ServiceLocator.Instance.GetService("BuildingAliveService") as BuildingAliveService;
        if (buildingAliveService != null)
        {
            buildingAliveService.UnregisterBuilding(this);
        }
    }

    private void HandleDeath()
    {
        gameObject.SetActive(false);
        FindObjectOfType<BuildingManager>().HandleBuildingDeath(this);
    }

    public void Reactivate()
    {
        gameObject.SetActive(true);
        healthComponent.Heal(healthComponent.GetMaxHealth());
    }
}