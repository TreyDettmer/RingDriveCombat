using UnityEngine.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Scene
{
    Loading,
    StartMenu,
    MainGame
}

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    bool KeepFadingIn;
    bool KeepFadingOut;
    public Scene currentScene;
    public Sound[] musicTracks;
    int currentMusicTrackIndex = 0;

    // Start is called before the first frame update
    void Awake()
    {
        currentScene = Scene.Loading;
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound m in musicTracks)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }

    }

    private void Start()
    {
        //Play("Theme");

        
    }

    private void Update()
    {
        
        if (currentScene == Scene.StartMenu)
        {
            if (musicTracks[currentMusicTrackIndex].source.isPlaying == false)
            {
                if (currentMusicTrackIndex + 1 > 1)
                {
                    currentMusicTrackIndex = 0;
                    PlayMusic(currentMusicTrackIndex);
                }
                else
                {
                    currentMusicTrackIndex += 1;
                    PlayMusic(currentMusicTrackIndex);
                }

            }
        }
        if (currentScene == Scene.MainGame)
        {
            if (musicTracks[currentMusicTrackIndex].source.isPlaying == false)
            {
                if (currentMusicTrackIndex + 1 > 2)
                {
                    currentMusicTrackIndex = 1;
                    PlayMusic(currentMusicTrackIndex);
                }
                else
                {
                    currentMusicTrackIndex += 1;
                    PlayMusic(currentMusicTrackIndex);
                }
            }
        }
        
    }



    public void PlayMusic(int index)
    {
        //Sound s = Array.Find(sounds, sound => sound.name == name);
        //s.source.Play();
        StopMusic();
        if (musicTracks[index] != null)
        {
            currentMusicTrackIndex = index;
            Sound s = musicTracks[currentMusicTrackIndex] as Sound;
            s.source.Play();
            StartCoroutine(FadeIn(s,.1f, Mathf.Clamp(s.volume * (FindObjectOfType<GameSettings>().musicVolume / 11f),0f,1f)));
        }
        else
        {
            //Debug.Log("Null");
        }
        
        

    }

    public void StopMusic()
    {
        for (int i = 0; i < musicTracks.Length; i++)
        {
            musicTracks[i].source.Stop();
        }
    }

    public void StopSoundEffects()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            sounds[i].source.Stop();
        }
    }

    public void StopSoundEffect(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (s.loop == true)
        {
            s.source.loop = false;
        }
        s.source.Stop();
    }

    public void ChangeSoundEffectPitch(string name, float pitch)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.pitch = pitch;
    }

    public void UpdateMusicVolume()
    {
        for (int i = 0; i < musicTracks.Length;i++)
        {
            musicTracks[i].source.volume = Mathf.Clamp(musicTracks[i].volume * (FindObjectOfType<GameSettings>().musicVolume / 11f), 0f, 1f);
        }
        
    }

    public void Play(string name)
    {

        if (name == "BulletImpact")
        {
            int choice = UnityEngine.Random.Range(0, 3);
            if (choice == 0)
            {
                name = "BulletImpact1";
            }
            else if (choice == 1)
            {
                name = "BulletImpact2";
            }
            else
            {
                name = "BulletImpact3";
            }
        }
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        if (s.loop == true)
        {
            s.source.loop = true;
        }
        s.source.volume = Mathf.Clamp(s.volume * (FindObjectOfType<GameSettings>().effectsVolume / 11f),0f,1f);
        s.source.Play();

    }

    IEnumerator FadeIn(Sound s, float speed, float maxVolume)
    {
        KeepFadingIn = true;
        KeepFadingOut = false;
        s.source.volume = 0;
        float audioVolume = s.source.volume;
        while (s.source.volume < maxVolume && KeepFadingIn)
        {
            audioVolume += speed;
            s.source.volume = audioVolume;
            
            yield return new WaitForSecondsRealtime(.1f);
        }

    }

    IEnumerator FadeOut(float speed)
    {
        KeepFadingIn = false;
        KeepFadingOut = true;
        float audioVolume = sounds[currentMusicTrackIndex].source.volume;
        while (sounds[currentMusicTrackIndex].source.volume >= speed && KeepFadingOut)
        {
            audioVolume -= speed;
            sounds[currentMusicTrackIndex].source.volume = audioVolume;
            yield return new WaitForSecondsRealtime(.1f);
        }

    }

}
