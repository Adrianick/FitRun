using UnityEngine;
using UnityEngine.SceneManagement;

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
       // SceneManager.LoadSceneAsync("RestartMenu");

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

    public void OffSound()
    {
        AudioListener.volume = 0;
      
    }

    public void OnSound()
    {
        AudioListener.volume = 1.0f;
    }

    /*public void ToggleSound()
    {
        AudioListener.volume = 1 - AudioListener.volume;
    }*/

}
