using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXService : MonoBehaviour
{
    [SerializeField] private DeathParticlesSO deathParticlesSO;

    private void Awake()
    {
        ServiceLocator.Instance.SetService("VFXService", this);
    }

    public RandomContainer<ParticleSystem> GetDeathParticles()
    {
        return deathParticlesSO.deathParticles;
    }
}