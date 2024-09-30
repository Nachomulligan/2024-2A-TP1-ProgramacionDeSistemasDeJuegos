using System;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyVfx : MonoBehaviour
    {
        private Enemy _enemy;
        private VFXService vfxService;
        private void Reset() => FetchComponents();

        private void Awake() => FetchComponents();

        private void FetchComponents()
        {
            _enemy = GetComponent<Enemy>();
        }

        private void OnEnable()
        {
            // registers the VFXService and subscribes to the enemy death
            vfxService = ServiceLocator.Instance.GetService("VFXService") as VFXService;
            _enemy.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            // unsubscribes from enemy death
            _enemy.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            //gets random prefab from service and instantiates it
            var deathPrefabs = vfxService.GetDeathParticles();
            if (!deathPrefabs.TryGetRandom(out var prefab))
                return;
            var vfx = Instantiate(prefab, transform.position, transform.rotation);
            var mainModule = vfx.main;
            mainModule.stopAction = ParticleSystemStopAction.Destroy;
        }
    }
}
