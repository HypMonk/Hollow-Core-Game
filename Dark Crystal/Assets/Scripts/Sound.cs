using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)] public float volume;
    [Range(.1f, 3f)] public float pitch;

    public bool loop;
    public bool interruptSelf;

    [Header("3D")]
    public bool threeDimensional;
    [Range(0f, 5f)] public float dopplerLevel;
    [Range(0f, 360f)] public float spread;
    [Range(0f, 360f)] public float maxDistance;

    [HideInInspector] public AudioSource source;
}
