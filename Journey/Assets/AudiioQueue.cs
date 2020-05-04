using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudiioQueue : MonoBehaviour
{
    public AudioSource audioSource;
    public List<AudioClip> queue;
    public bool acitve = true;
    public bool randomSounds = false;
    public List<AudioClip> sounds;
    public float min_random_time = 20f, max_random_time = 50f;
    public float time = 0f;

    private void Start() {
        time = Random.Range(min_random_time, max_random_time);
    }

    // Update is called once per frame
    void Update()
    {
        if(!acitve) { return; }

        if(randomSounds) {
            if(time > 0) {
                time -= Time.deltaTime;
            } else {
                queue.Add(sounds[Random.Range(0, sounds.Count - 1)]);
                time = Random.Range(min_random_time, max_random_time);
            }
        }

        if(audioSource.isPlaying) { return; }
        if(queue.Count > 0) {
            audioSource.clip = queue[0];
            audioSource.Play();
            queue.RemoveAt(0);
        }
    }
}
