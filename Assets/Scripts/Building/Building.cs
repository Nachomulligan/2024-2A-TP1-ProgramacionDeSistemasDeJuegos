using HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private IHealth health;
    public float respawnTime = 5f;
    [SerializeField] private int shieldAmount = 20;
    private BuildingAliveService buildingAliveService;
    private void Awake()
    {
        //initialeze health with shield and fetches the BuildingAliveService
        health = new ShieldDecorator(new Health(10), shieldAmount);
        buildingAliveService = ServiceLocator.Instance.GetService("BuildingAliveService") as BuildingAliveService;
    }

    private void OnEnable()
    {
        // subscribe to death event and register in buildingAliveService
        health.OnDeath += HandleDeath;
        if (buildingAliveService != null)
        {
            buildingAliveService.RegisterBuilding(this);
        }
    }

    private void OnDisable()

    {
        // unsubscribe from death event and unregister from buildingAliveService
        health.OnDeath -= HandleDeath;
        if (buildingAliveService != null)
        {
            buildingAliveService.UnregisterBuilding(this);
        }
    }

    private void HandleDeath()
    {
        // deactivate building and notify buildingManager
        gameObject.SetActive(false);
        FindObjectOfType<BuildingManager>().HandleBuildingDeath(this);
    }

    public void Reactivate()
    {
        // reactivate building and heal to max health
        gameObject.SetActive(true);
        health.Heal(health.GetMaxHealth());
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    public int GetCurrentHealth() => health.GetCurrentHealth();
}