using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip goodItemAudioClip;
    public AudioClip badItemAudioClip;
    public AudioClip menuMusic;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 0.2f;
    }
    void Update()
    {

    }

    public void StopMusic()
    {
        //gameObject.GetComponent<AudioSource>().Stop();
        AudioSource music = gameObject.GetComponent<AudioSource>();
        music.clip = menuMusic;
        music.Play();
    }

    public void PlaySound(bool isGood)
    {

        if (isGood)
        {
            audioSource.PlayOneShot(goodItemAudioClip);
        }
        else
        {
            audioSource.PlayOneShot(badItemAudioClip);
        }


    }
}
