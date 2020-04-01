using UnityEngine.Audio;
using UnityEngine;
using System;
//Basically an array tht holds audio
public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;
    // Start is called before the first frame update, same as start but called right before.
    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}