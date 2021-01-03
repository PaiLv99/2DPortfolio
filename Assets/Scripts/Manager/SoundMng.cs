using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMng //: TSingleton<SoundMng>
{
    private Dictionary<string, AudioClip> _sfxSounds = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _bgmSounds = new Dictionary<string, AudioClip>();

    private AudioSource _sfxPlayer;
    private AudioSource _bgmPlayer;

    private float _sfxMasterVolume = 1.0f;
    private float _bgmMasterVolume = 1.0f;

    //public override void Init()
    //{

    //}

    public void Init()
    {
        GameObject root = new GameObject { name = "Sound" };
        //root.transform.parent = GameMng.Instance.transform;
        Object.DontDestroyOnLoad(root);

        if (root != null)
        {
            GameObject s = new GameObject { name = "SfxPlayer" };
            s.transform.parent = root.transform;

            _sfxPlayer = s.AddComponent<AudioSource>();
           
            GameObject go = new GameObject { name = "BGMPlayer" };
            go.transform.parent = root.transform;

            _bgmPlayer = go.AddComponent<AudioSource>();
            _bgmPlayer.loop = true;
        }

        //_sfxPlayer = Helper.CreateObject<AudioSource>(transform);
        //_bgmPlayer = Helper.CreateObject<AudioSource>(transform);

        AudioClip[] sfx = Resources.LoadAll<AudioClip>("Sounds/Sfx");
        for (int i = 0; i < sfx.Length; i++)
            _sfxSounds.Add(sfx[i].name, sfx[i]);

        AudioClip[] bgm = Resources.LoadAll<AudioClip>("Sounds/Bgm");
        for (int i = 0; i < bgm.Length; i++)
            _bgmSounds.Add(bgm[i].name, bgm[i]);
    }

    public void SfxPlay(string str, float volume = 1.0f)
    {
        if (_sfxSounds.ContainsKey(str))
            _sfxPlayer.PlayOneShot(_sfxSounds[str], volume * _sfxMasterVolume);
    }

    public void BgmPlay(string str, float volume = 1.0f)
    {
        if (_bgmSounds.ContainsKey(str))
        {
            if (_bgmPlayer.isPlaying)
                _bgmPlayer.Stop();

            _bgmPlayer.clip = _bgmSounds[str];
            _bgmPlayer.volume = volume * _bgmMasterVolume;
            _bgmPlayer.loop = true;
            _bgmPlayer.Play();
        }
    }

    public void ControllBgmMasterVolume(float volume)
    {
        _bgmMasterVolume = volume;
    }

    public void ControllSfxMasterVolume(float volume)
    {
        _sfxMasterVolume = volume;
    }

    public void Clear()
    {
        _bgmSounds.Clear();
        _sfxSounds.Clear();
    }
}
