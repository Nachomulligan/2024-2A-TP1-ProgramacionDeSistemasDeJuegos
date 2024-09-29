using HealthSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private IHealth health;
    public float respawnTime = 5f;
    [SerializeField] private int shieldAmount = 20; // Cantidad de escudo para el edificio

    private void Awake()
    {
        health = new ShieldDecorator(new Health(100), shieldAmount); // Crear una instancia de Health con escudo
    }

    private void OnEnable()
    {
        health.OnDeath += HandleDeath;

        var buildingAliveService = ServiceLocator.Instance.GetService("BuildingAliveService") as BuildingAliveService;
        if (buildingAliveService != null)
        {
            buildingAliveService.RegisterBuilding(this);
        }
    }

    private void OnDisable()
    {
        health.OnDeath -= HandleDeath;

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
        health.Heal(health.GetMaxHealth()); // Reiniciar la salud al máximo
    }

    public void TakeDamage(int damage)
    {
        health.TakeDamage(damage);
    }

    public int GetCurrentHealth() => health.GetCurrentHealth();
}