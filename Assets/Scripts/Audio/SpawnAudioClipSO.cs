using Audio;
using Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips", menuName = "ScriptableObjects/SpawnAudioClipScriptable")]
public class SpawnAudioClipSO : ScriptableObject
{
    public RandomContainer<AudioClipData> spawnClips;

}
