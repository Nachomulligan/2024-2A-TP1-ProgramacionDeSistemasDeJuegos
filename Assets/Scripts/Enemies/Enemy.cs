using HealthSystem;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private NavMeshAgent agent;
        private Transform targetBuilding;
        private BuildingAliveService buildingAliveService;
        private HealthComponent healthComponent;

        public event Action OnSpawn = delegate { };
        public event Action OnDeath = delegate { };

        private void Reset() => FetchComponents();

        private void Awake()
        {
            FetchComponents();
            buildingAliveService = ServiceLocator.Instance.GetService("BuildingAliveService") as BuildingAliveService;
            UpdateClosestBuilding();
        }

        private void FetchComponents()
        {
            agent ??= GetComponent<NavMeshAgent>();
            healthComponent = GetComponent<HealthComponent>();
        }

        private void UpdateClosestBuilding()
        {
            if (buildingAliveService != null)
            {
                Building closestBuilding = buildingAliveService.GetClosestBuilding(transform.position);
                if (closestBuilding != null)
                {
                    targetBuilding = closestBuilding.transform; 
                    Vector3 destination = targetBuilding.position;
                    destination.y = transform.position.y;
                    StartCoroutine(SetDestinationToClosestBuildingAfterWaiting(destination)); 
                }
            }
        }

        public void OnSpawnFromPool(Vector3 spawnPosition)
        {
            if (!agent.isActiveAndEnabled)
            {
                agent.enabled = true;
            }
            agent.Warp(spawnPosition);

            healthComponent.Heal(healthComponent.GetMaxHealth());
            UpdateClosestBuilding(); 
            StartCoroutine(AlertSpawn());
        }

        private IEnumerator SetDestinationToClosestBuildingAfterWaiting(Vector3 destination)
        {
            if (targetBuilding != null)
            {
                yield return 2; 
                agent.SetDestination(destination);
            }
        }

        private IEnumerator AlertSpawn()
        {
            yield return null; 
            OnSpawn();
        }

        private void Update()
        {
            if (targetBuilding != null && !targetBuilding.gameObject.activeInHierarchy)
            {
                Debug.Log($"{targetBuilding.name} is inactive. Finding new target...");
                UpdateClosestBuilding();
                return;
            }

            if (agent.hasPath && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
            {
                Debug.Log($"{name}: I'll die for my people!");
                DamageBuilding();
                DamageSelf();
                CheckIfDead();
            }
        }

        private void DamageBuilding()
        {
            if (targetBuilding != null)
            {
                HealthComponent buildingHealth = targetBuilding.GetComponent<HealthComponent>();
                if (buildingHealth != null)
                {
                    int damageAmount = 10;
                    buildingHealth.TakeDamage(damageAmount);
                    Debug.Log($"{name} dealt {damageAmount} damage to {targetBuilding.name}.");
                }
                else
                {
                    Debug.LogWarning("HealthComponent not found on " + targetBuilding.name);
                }
            }
        }

        private void DamageSelf()
        {
            int selfDamage = 10; 
            healthComponent.TakeDamage(selfDamage);
        }
        private void CheckIfDead()
        {
            if (healthComponent.GetCurrentHealth() <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            OnDeath();
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            agent.enabled = false;
        }
    }
}