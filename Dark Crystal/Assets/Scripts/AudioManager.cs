using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
    [Header("Global Sounds")]
    public Sound[] worldSounds;

    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
            return;
        }

        foreach (Sound s in worldSounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;

            s.source.loop = s.loop;

            if (s.threeDimensional)
            {
                s.source.spatialBlend = 1;

                s.source.dopplerLevel = s.dopplerLevel;
                s.source.spread = s.spread;
                s.source.maxDistance = s.maxDistance;
            }
        }
    }

    public void Play(string name)
    {
        Sound s = null;

        if (Array.Find(worldSounds, sound => sound.name == name) != null)
        {
            s = Array.Find(worldSounds, sound => sound.name == name);
        }

        if (s == null) { Debug.Log("Audio Not Found"); return; }

        if (s.interruptSelf)
        {
            s.source.PlayOneShot(s.clip);
        } else
        {
            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
        }
    }

    public void Play(string name, float volume, float pitch)
    {
        Sound s = null;

        if (Array.Find(worldSounds, sound => sound.name == name) != null)
        {
            s = Array.Find(worldSounds, sound => sound.name == name);
        }

        if (s == null) { Debug.Log("Audio Not Found"); return; }

        //Default if 0
        if (pitch != 0)
        {
            s.source.pitch = pitch;
        }

        //Default if 0
        if (volume != 0)
        {
            s.source.volume = volume;
        }

        if (s.interruptSelf)
        {
            s.source.PlayOneShot(s.clip);
        }
        else
        {
            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
        }
    }

    //for random clips
    public void Play(string name, AudioClip clip, float volume, float pitch)
    {
        Sound s = null;

        if (Array.Find(worldSounds, sound => sound.name == name) != null)
        {
            s = Array.Find(worldSounds, sound => sound.name == name);
        }

        if (s == null) { Debug.Log("Audio Not Found"); return; }

        s.source.clip = clip;
        s.clip = clip;

        //Default if 0
        if (pitch != 0)
        {
            s.source.pitch = pitch;
        }

        //Default if 0
        if (volume != 0)
        {
            s.source.volume = volume;
        }

        if (s.interruptSelf)
        {
            s.source.PlayOneShot(s.clip);
        }
        else
        {
            if (!s.source.isPlaying)
            {
                s.source.Play();
            }
        }
    }

    public void Stop(string name)
    {
        Sound s = null;

        if (Array.Find(worldSounds, sound => sound.name == name) != null)
        {
            s = Array.Find(worldSounds, sound => sound.name == name);
        }

        if (s == null) { Debug.Log("Audio Not Found"); return; }

        s.source.Stop();
    }
}
