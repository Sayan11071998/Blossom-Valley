using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private List<SoundCategory> soundCategories;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    public void PlayMusic(SoundType soundType)
    {
        AudioClip clip = GetClip(soundType);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(SoundType soundType)
    {
        AudioClip clip = GetClip(soundType);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    private AudioClip GetClip(SoundType soundType)
    {
        SoundCategory category = GetSoundCategory(soundType);
        if (category != null && category.clip != null)
        {
            return category.clip;
        }
        return null;
    }

    private SoundCategory GetSoundCategory(SoundType soundType)
    {
        foreach (SoundCategory category in soundCategories)
        {
            if (category.soundType == soundType)
                return category;
        }
        return null;
    }
}