using Audio;
using Enemies;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips", menuName = "ScriptableObjects/ExplosionAudioClipScriptable")]
public class ExplosionAudioClipSO : ScriptableObject
{
    public RandomContainer<AudioClipData> explosionClips;

}
