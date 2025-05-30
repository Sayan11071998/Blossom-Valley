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

    public void PlayMusic(SoundType soundType, int clipIndex = 0)
    {
        AudioClip clip = GetClip(soundType, clipIndex);
        if (clip != null)
        {
            musicSource.clip = clip;
            musicSource.loop = true;
            musicSource.Play();
        }
    }

    public void PlaySFX(SoundType soundType, int clipIndex = 0)
    {
        AudioClip clip = GetClip(soundType, clipIndex);
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayRandomSFX(SoundType soundType)
    {
        SoundCategory category = GetSoundCategory(soundType);
        if (category != null && category.clips.Count > 0)
        {
            int randomIndex = Random.Range(0, category.clips.Count);
            sfxSource.PlayOneShot(category.clips[randomIndex]);
        }
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    private AudioClip GetClip(SoundType soundType, int clipIndex)
    {
        SoundCategory category = GetSoundCategory(soundType);
        if (category != null && clipIndex < category.clips.Count)
        {
            return category.clips[clipIndex];
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