using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using Utils;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class SoundController : MonoBehaviour
{
    static SoundController instance;

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

    public static SoundController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = Instantiate(Resources.Load(TextUtil.GetResourcesFullPath("MasterAudio", "Prefab")) as GameObject);
                go.name = "SoundManager";
                DontDestroyOnLoad(go);
                instance = go.GetComponent<SoundController>();
            }
            
            return instance;
        }
    }
    public void PlayMusic(AudioClip musicClip, float atTime = 0)
    {
        _2DBGMAudio.clip = musicClip;
        _2DBGMAudio.time = atTime;      // May be inaccurate if the audio source is compressed http://docs.unity3d.com/ScriptReference/AudioSource-time.html BK
        _2DBGMAudio.Play();
    }

    /**
     * Stops playing game music.
     */
    public void StopMusic()
    {
        _2DBGMAudio.Stop();
    }

    /**
     * Plays a sound effect once, at the specified volume.
     * Multiple sound effects can be played at the same time.
     * @param soundClip The sound effect clip to play
     * @param volume The volume level of the sound effect
     */
    public void PlaySound(AudioClip soundClip, float volume=1.0f)
    {
        _2DEffectAudio.PlayOneShot(soundClip, volume);
    }

    public virtual void PlaySoundAtTime(AudioClip soundClip, float volume, float atTime)
    {
        _2DEffectAudio.time = atTime;                      // This may not work BK
        _2DEffectAudio.PlayOneShot(soundClip, volume);
    }
    #region  BGM操作
    public void PlayBGMImmediate(int BGMid, bool NormalVolume = false)
    {
        _2DBGMAudio.clip = Resources.Load(TextUtil.GetResourcesFullPath("bgm_" + BGMid.ToString(), "Sound", "BGM")) as AudioClip;
        _2DBGMAudio.Play();
        if (NormalVolume)
            RestoreBGMVolume();
    }
    public void PlayBGMFade(int BGMid, float timeToReach)
    {
        StopBGM(timeToReach);
        GameUtil.DelayFunc(this, delegate
         {
             _2DBGMAudio.clip = Resources.Load(TextUtil.GetResourcesFullPath("bgm_" + BGMid.ToString(), "Sound", "BGM")) as AudioClip;
             _2DBGMAudio.Play();
             RestoreBGMVolume(timeToReach);
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
    //输入一个0~1.0之间的数字
    //
    public void SetBGMVolume(float volume)
    {
        Mixer.SetFloat("2DBGMVolume", VolumeIntToDB(volume));
    }
    public void LowerBGMVolume(float timeToReach = 0.5f)
    {
        _VolumeLowerSnap.TransitionTo(timeToReach);
    }
    public void RestoreBGMVolume(float timeToReach = 0.5f)
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
    public void SetMasterVolume(float volume)
    {
        Mixer.SetFloat("MasterVolume", VolumeIntToDB(volume));
    }

    public static float VolumeIntToDB(float volume)
    {
        volume = Mathf.Clamp(volume, 0f, 1.0f);
        float v = (float)volume * 50 - 50.0f;
        return v;
    }
    private float beforeMuteVolume;
    public void SetVolumeMute()
    {
         Mixer.GetFloat("MasterVolume",out beforeMuteVolume);
        SetMasterVolume(0);
    }
    public void SetVolumeOpen()
    {
        SetMasterVolume(beforeMuteVolume);
    }
    #endregion
}
