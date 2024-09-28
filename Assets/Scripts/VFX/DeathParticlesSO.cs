using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Particles", menuName = "ScriptableObjects/DeathParticles")]
public class DeathParticlesSO : ScriptableObject
{
    public RandomContainer<ParticleSystem> deathParticles;
}
