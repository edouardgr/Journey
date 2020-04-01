using UnityEngine.Audio;
using UnityEngine;

[System.Serializable] //makes array 
public class Sound
{
    public string name; //Name of audio clip

    public AudioClip clip;

    [Range(0f,1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    [HideInInspector] // Hides in inspector so it doesnt show
    public AudioSource source;
}