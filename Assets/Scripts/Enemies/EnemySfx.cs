using Audio;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Enemy))]
    public class EnemySfx : MonoBehaviour
    {
        private AudioService audioService;
        private Enemy _enemy;

        private void Reset() => FetchComponents();

        private void Awake() => FetchComponents();

        private void FetchComponents()
        {
            _enemy ??= GetComponent<Enemy>();
        }

        private void OnEnable()
        {

            audioService = ServiceLocator.Instance.GetService("AudioService") as AudioService;

            _enemy.OnSpawn += HandleSpawn;
            _enemy.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            _enemy.OnSpawn -= HandleSpawn;
            _enemy.OnDeath -= HandleDeath;
        }

        private void HandleDeath()
        {
            PlayRandomClip(audioService.GetExplosionClips(), audioService.GetAudioSourcePrefab());
        }

        private void HandleSpawn()
        {
            PlayRandomClip(audioService.GetSpawnClips(), audioService.GetAudioSourcePrefab());
        }

        private void PlayRandomClip(RandomContainer<AudioClipData> container, AudioPlayer sourcePrefab)
        {
            if (!container.TryGetRandom(out var clipData))
                return;

            SpawnSource(sourcePrefab).Play(clipData);
        }

        private AudioPlayer SpawnSource(AudioPlayer prefab)
        {
            return Instantiate(prefab, transform.position, transform.rotation);
        }
    }
}
