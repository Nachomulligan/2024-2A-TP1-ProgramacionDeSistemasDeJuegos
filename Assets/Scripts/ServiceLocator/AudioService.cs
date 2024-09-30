using Audio;
using Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioService : MonoBehaviour
{
    [SerializeField] private SpawnAudioClipSO spawnClipsSO; 
    [SerializeField] private ExplosionAudioClipSO explosionClipsSO;
    [SerializeField] private AudioPlayer audioSourcePrefab;

    private void Awake()
    {
        //set service in service locator
        ServiceLocator.Instance.SetService("AudioService", this);
    }

    public RandomContainer<AudioClipData> GetSpawnClips()
    {
        //returns clip from scriptable
        return spawnClipsSO.spawnClips;
    }

    public RandomContainer<AudioClipData> GetExplosionClips()
    {
        //returns clip from scriptable
        return explosionClipsSO.explosionClips;
    }

    public AudioPlayer GetAudioSourcePrefab()
    {
        return audioSourcePrefab;
    }
}
