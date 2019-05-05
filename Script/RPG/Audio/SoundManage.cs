using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SoundManage : MonoSingleton<SoundManage>
{
    public AudioMixer Mixer;

    public AudioMixerSnapshot _VolumeLowerSnap;
    public AudioMixerSnapshot _VolumeNormalSnap;
    public AudioMixerSnapshot _VolumeZero;

    public AudioMixerGroup _MasterMixerGroup;
    public AudioMixerGroup _2DBGMGroup;
    public AudioMixerGroup _2DEffectGroup;
    public AudioMixerGroup _3DEffectGroup;
    public AudioMixerGroup _3DVoiceGroup;

    public AudioSource _2DBGMAudio;
    public AudioSource _2DEffectAudio;
    public AudioClip[] effects;//effect赋值到某个物体上播放
    private string GetBgmResourcePath(int BGM_id)
    {
        return TextUtil.GetResourcesFullPath("bgm_" + BGM_id.ToString(), "Audio", "BGM");
    }
    public void PlayMusic(AudioClip musicClip, float atTime = 0)
    {
        if (musicClip == null) { Debug.LogError("clip is null "); return; }
        _2DBGMAudio.clip = musicClip;
        _2DBGMAudio.time = atTime;     
        _2DBGMAudio.Play();
    }

    public void StopMusic()
    {
        _2DBGMAudio.Stop();
    }
    public void PlaySound(AudioClip soundClip, float volume = 1.0f)
    {
        if (soundClip == null) { Debug.LogError("clip is null "); return; }
        _2DEffectAudio.PlayOneShot(soundClip, volume);
    }

    public virtual void PlaySoundAtTime(AudioClip soundClip, float volume, float atTime)
    {
        if (soundClip == null) { Debug.LogError("clip is null "); return; }
        _2DEffectAudio.time = atTime;                      // This may not work BK
        _2DEffectAudio.PlayOneShot(soundClip, volume);
    }
    #region  BGM操作

    public void PlayBGMImmediate(int BGMid, bool NormalVolume = false)
    {
        _2DBGMAudio.clip = Resources.Load(GetBgmResourcePath(BGMid)) as AudioClip;
        _2DBGMAudio.Play();
        if (NormalVolume)
            NormalBGM();
    }
    public void PlayBGMFade(int BGMid, float timeToReach)
    {
        StopBGM(timeToReach);
        GameUtil.DelayFunc(this, delegate
         {
             _2DBGMAudio.clip = Resources.Load(GetBgmResourcePath(BGMid)) as AudioClip;
             _2DBGMAudio.Play();
             NormalBGM(timeToReach);
         }, timeToReach);
    }
    public void StopBGMImmediate()
    {
        _2DBGMAudio.Stop();
    }
    public void StopBGM(float timeToReach)
    {
        _VolumeZero.TransitionTo(timeToReach);
        GameUtil.DelayFunc(this, StopBGMImmediate, timeToReach);
    }
    //
    //输入一个0~10之间的数字
    //
    public void SetBGMVolume(int volume)
    {
        Mixer.SetFloat("2DBGMVolume", VolumeIntToDB(volume));
    }
    public void LowerBGM(float timeToReach = 0.5f)
    {
        _VolumeLowerSnap.TransitionTo(timeToReach);
    }
    public void NormalBGM(float timeToReach = 0.5f)
    {
        _VolumeNormalSnap.TransitionTo(timeToReach);
    }
    #endregion

    #region 2DEffect
    public void Play2DEffect(int index)//绑定到当前摄像机来播放2d音频
    {
        if (index > effects.Length)
            return;
        _2DEffectAudio.PlayOneShot(effects[index]);
    }
    #endregion

    #region 3DEffect
    public void Play3DEffect(GameObject obj, int index)//在某物体上播放3D音频
    {
        if (index > effects.Length)
            return;
        AudioSource asource = MiscUtil.GetComponentNotNull<AudioSource>(obj);
        asource.outputAudioMixerGroup = _3DEffectGroup;
        asource.PlayOneShot(effects[index]);
    }
    #endregion

    #region 3DVoice
    public void Play3DVoice(GameObject obj, int index)
    {

    }
    #endregion
    #region 公用
    //输入一个-80.0-20.0之间的数字
    public void SetMasterVolume(int volume)
    {
        Mixer.SetFloat("MasterVolume", VolumeIntToDB(volume));
    }

    public static float VolumeIntToDB(int volume)
    {
        volume = Mathf.Clamp(volume, 0, 10);
        float v = (float)volume * 5 - 50.0f;
        return v;
    }
    public void SetVolumeMute()
    {
        SetMasterVolume(0);
    }
    public void SetVolumeOpen()
    {
        SetMasterVolume(10);
    }
    #endregion
}
