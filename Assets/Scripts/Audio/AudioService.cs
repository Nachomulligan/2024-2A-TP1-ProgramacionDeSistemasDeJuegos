using Audio;
using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private RandomContainer<AudioClipData> spawnClips;
    [SerializeField] private RandomContainer<AudioClipData> explosionClips;
    [SerializeField] private AudioPlayer audioSourcePrefab;

    private void Awake()
    {
        ServiceLocator.Instance.SetService("AudioService", this);
    }

    public RandomContainer<AudioClipData> GetSpawnClips()
    {
        return spawnClips;
    }

    public RandomContainer<AudioClipData> GetExplosionClips()
    {
        return explosionClips;
    }

    public AudioPlayer GetAudioSourcePrefab()
    {
        return audioSourcePrefab;
    }
}
