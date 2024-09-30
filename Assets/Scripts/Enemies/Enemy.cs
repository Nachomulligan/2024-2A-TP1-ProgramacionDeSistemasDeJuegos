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
        private IHealth health; 
        private bool hasDamagedBuilding; 

        public event Action OnSpawn = delegate { };
        public event Action OnDeath = delegate { };

        private void Reset() => FetchComponents();

        private void Awake()
        {
            // initialize components, health, and find closest building
            FetchComponents();
            buildingAliveService = ServiceLocator.Instance.GetService("BuildingAliveService") as BuildingAliveService;
            health = new Health(10); 
            UpdateClosestBuilding();
        }

        private void FetchComponents()
        {
            // fetch NavMeshAgent
            agent ??= GetComponent<NavMeshAgent>();
        }

        private void UpdateClosestBuilding()
        {
            // update the target to the closest building using BuildingAliveService
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
            // reset agent position, health and find the closest building when spawning
            if (!agent.isActiveAndEnabled)
            {
                agent.enabled = true;
            }
            agent.Warp(spawnPosition);
            health.Heal(health.GetMaxHealth()); 
            UpdateClosestBuilding();
            StartCoroutine(AlertSpawn());
        }

        private IEnumerator SetDestinationToClosestBuildingAfterWaiting(Vector3 destination)
        {
            if (targetBuilding != null)
            {
                yield return new WaitForSeconds(2);
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
            // check if target building is active and find new target
            if (targetBuilding != null && !targetBuilding.gameObject.activeInHierarchy)
            {
                Debug.Log($"{targetBuilding.name} is inactive. Finding new target...");
                UpdateClosestBuilding();
                return;
            }

            //check if enemy reached target and damage target and himself
            if (agent.hasPath && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
            {
                if (!hasDamagedBuilding) 
                {
                    Debug.Log($"{name}: I'll die for my people!");
                    DamageBuilding();
                    DamageSelf();
                    hasDamagedBuilding = true; 
                }
            }
            else
            {
                hasDamagedBuilding = false; 
            }
        }

        private void DamageBuilding()
        {
            // apply damage to the building if target is valid
            if (targetBuilding != null)
            {
                Building building = targetBuilding.GetComponent<Building>(); 
                if (building != null)
                {
                    int damageAmount = 10;
                    building.TakeDamage(damageAmount);
                    Debug.Log($"{name} dealt {damageAmount} damage to {targetBuilding.name}.");
                }
                else
                {
                    Debug.LogWarning("Building not found on " + targetBuilding.name);
                }
            }
        }

        private void DamageSelf()
        {
            int selfDamage = 10;
            health.TakeDamage(selfDamage);
            CheckIfDead();
        }

        private void CheckIfDead()
        {
            if (health.GetCurrentHealth() <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            // trigger death event and deactivate the enemy
            OnDeath();
            gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            agent.enabled = false;
        }
    }
}