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
        private static Transform townCenter;
        private HealthComponent healthComponent;

        public event Action OnSpawn = delegate { };
        public event Action OnDeath = delegate { };

        private void Reset() => FetchComponents();

        private void Awake()
        {
            FetchComponents();
            InitializeTownCenter();
        }

        private void FetchComponents()
        {
            agent ??= GetComponent<NavMeshAgent>();
        }

        private void InitializeTownCenter()
        {
            if (townCenter == null)
            {
                var townCenterObject = GameObject.FindGameObjectWithTag("TownCenter");
                if (townCenterObject == null)
                {
                    Debug.LogError("No se encontró el Town Center.");
                    return;
                }
                townCenter = townCenterObject.transform;
            }
        }

        public void OnSpawnFromPool(Vector3 spawnPosition)
        {
            if (!agent.isActiveAndEnabled)
            {
                agent.enabled = true;
            }
            agent.Warp(spawnPosition);

            SetDestinationToTownCenter();
            StartCoroutine(AlertSpawn());
        }

        private void SetDestinationToTownCenter()
        {
            Vector3 destination = townCenter.position;
            destination.y = transform.position.y; 
            agent.SetDestination(destination);
        }

        private IEnumerator AlertSpawn()
        {
            //Waiting one frame because event subscribers could run their onEnable after us.
            yield return null;
            OnSpawn();
        }

        private void Update()
        {
            if (agent.hasPath && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance)
            {
                Debug.Log($"{name}: I'll die for my people!");
                DamageBuilding();
                Die();
            }
        }

        private void DamageBuilding()
        {
            if (townCenter != null)
            {
                HealthComponent buildingHealth = townCenter.GetComponent<HealthComponent>();
                if (buildingHealth != null)
                {
                    int damageAmount = 100;
                    buildingHealth.TakeDamage(damageAmount);
                    Debug.Log($"{name} dealt {damageAmount} damage to {townCenter.name}.");
                }
                else
                {
                    Debug.LogWarning("HealthComponent not found on " + townCenter.name);
                }
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
