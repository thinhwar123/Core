using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using TW.Utility.DesignPattern;

public enum AudioType
{
    BgmGamePlay,
    BgmLevelSelect,
    BgmGacha


}
public class AudioManager : Singleton<AudioManager>
{
    [field: SerializeField] public AudioConfig[] SoundFxArray {get; private set;}
    [field: SerializeField] public AudioConfig[] MusicArray {get; private set;}

    [field: SerializeField] public AudioSource SoundFxAudioSource { get; private set;}
    [field: SerializeField] public AudioSource MusicAudioSource { get; private set;}

    [field: SerializeField] public UnityAction<bool> OnSoundFxChange {get; set;}

    private Tween m_ScaleVolumeTween;

    private void Start()
    {
        AudioManager.Instance.PlayMusic(AudioType.BgmLevelSelect);
    }

    public void PlaySoundFx(AudioType audioType, int delay = 0)
    {
        AudioConfig audioConfig = SoundFxArray.FirstOrDefault(x => x.AudioType == audioType);
        if (audioConfig == null) return;
        PlaySoundDelay(audioConfig.AudioClip, delay);
    }
    public void PlayMusic(AudioType audioType)
    {
        AudioConfig audioConfig = MusicArray.FirstOrDefault(x => x.AudioType == audioType);
        if (audioConfig == null) return;
        MusicAudioSource.clip = audioConfig.AudioClip;
        MusicAudioSource.Play();
    }
    public void ChangeMusic(AudioType audioType, float time)
    {
        m_ScaleVolumeTween?.Kill();
        m_ScaleVolumeTween = DOTween.To(() => MusicAudioSource.volume, (x) => MusicAudioSource.volume = x, 0, time / 2f)
            .OnComplete(() =>
            {
                AudioConfig audioConfig = MusicArray.FirstOrDefault(x => x.AudioType == audioType);
                if (audioConfig == null) return;
                MusicAudioSource.clip = audioConfig.AudioClip;
                MusicAudioSource.Play();

                m_ScaleVolumeTween = DOTween.To(() => MusicAudioSource.volume, (x) => MusicAudioSource.volume = x, 1, time / 2f);
            });

    }

    private async void PlaySoundDelay(AudioClip audioClip, int delay = 0)
    {
        await UniTask.Delay(delay);
        SoundFxAudioSource.PlayOneShot(audioClip);
    }
    public void SetSoundFx(bool value)
    {
        SoundFxAudioSource.mute = !value;
        OnSoundFxChange?.Invoke(value);
    }
    public void SetMusic(bool value)
    {
        MusicAudioSource.mute = !value;
    }
}

[System.Serializable]
public class AudioConfig
{
    [field: SerializeField] public AudioType AudioType {get; private set;}
    [field: SerializeField] public AudioClip AudioClip {get; private set;}
}
