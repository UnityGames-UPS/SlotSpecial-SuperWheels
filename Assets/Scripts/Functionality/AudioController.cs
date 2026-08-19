using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
internal class AudioController : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgMusicSource;
    [SerializeField] private AudioSource gameSoundSource;
    // [SerializeField] private AudioSource uiSource;

    [Header("Background")]
    [SerializeField] private AudioClip bgMusic;
    [SerializeField] private AudioClip bonusbgMusic;

    [Header("Game Sounds")]
    [SerializeField] private AudioClip WheelArrowTick;
    [SerializeField] private AudioClip WheelBlackOverlay;
    [SerializeField] private AudioClip BigWin;
    [SerializeField] private AudioClip WheelArrowStop;
    [SerializeField] private AudioClip uiButton;
    [SerializeField] private AudioClip normalWin;
    [SerializeField] private AudioClip bonusComplete;
    [SerializeField] private AudioClip ReelHit;
    [SerializeField] private AudioClip reelSpinning;
    [SerializeField] private AudioClip bonusHit;

    // [Header("UI Sounds")]
    // [SerializeField] private AudioClip uiButton;

    // [Header("Sound Buttons")]
    // [SerializeField] private Button SoundButton;
    // [SerializeField] private Button SoundMuteButton;
    // [SerializeField] private Button MusicButton;
    // [SerializeField] private Button MusicMuteButton;

    private bool isGameMuted = false;
    private bool isMusicMuted = false;

    private readonly Dictionary<AudioSource, bool> preFocusMuteState = new Dictionary<AudioSource, bool>();
    private bool isForceMuted = false;

    private const string PrefKeyMusicVol = "audio_music_volume";
    private const string PrefKeySfxVol = "audio_sfx_volume";

    private float _musicVolume = 0.5f;
    private float _sfxVolume = 1.0f;

    internal float MusicVolume => _musicVolume;
    internal float SfxVolume => _sfxVolume;

    private void Awake()
    {
        _musicVolume = PlayerPrefs.GetFloat(PrefKeyMusicVol, 0.5f);
        _sfxVolume = PlayerPrefs.GetFloat(PrefKeySfxVol, 1.0f);
    }

    internal void SetMusicVolume(float volume)
    {
        _musicVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(PrefKeyMusicVol, _musicVolume);
        PlayerPrefs.Save();
        if (bgMusicSource) bgMusicSource.volume = _musicVolume;
    }

    internal void SetSfxVolume(float volume)
    {
        _sfxVolume = Mathf.Clamp01(volume);
        PlayerPrefs.SetFloat(PrefKeySfxVol, _sfxVolume);
        PlayerPrefs.Save();
        if (gameSoundSource) gameSoundSource.volume = _sfxVolume;
    }

    private void Start()
    {
        if (bgMusicSource) bgMusicSource.volume = _musicVolume;
        if (gameSoundSource) gameSoundSource.volume = _sfxVolume;

        // if (SoundButton)
        // {
        //     SoundButton.onClick.RemoveAllListeners();
        //     SoundButton.onClick.AddListener(ToggleGameSound);
        // }

        // if (MusicButton)
        // {
        //     MusicButton.onClick.RemoveAllListeners();
        //     MusicButton.onClick.AddListener(ToggleBackgroundMusic);
        // }

        // if (SoundMuteButton)
        // {
        //     SoundMuteButton.onClick.RemoveAllListeners();
        //     SoundMuteButton.onClick.AddListener(ToggleGameSound);
        // }

        // if (MusicMuteButton)
        // {
        //     MusicMuteButton.onClick.RemoveAllListeners();
        //     MusicMuteButton.onClick.AddListener(ToggleBackgroundMusic);
        // }

        PlayBackground();
    }

    // private void ToggleGameSound()
    // {
    //     Debug.Log("button pressed!");
    //     if (!isGameMuted)
    //     {
    //         SoundMuteButton.gameObject.SetActive(true);
    //         SoundButton.gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         SoundButton.gameObject.SetActive(true);
    //         SoundMuteButton.gameObject.SetActive(false);
    //     }
    //     isGameMuted = !isGameMuted;
    //     MuteGame(isGameMuted);
    // }

    // private void ToggleBackgroundMusic()
    // {
    //     if (!isMusicMuted)
    //     {
    //         MusicMuteButton.gameObject.SetActive(true);
    //         MusicButton.gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         MusicButton.gameObject.SetActive(true);
    //         MusicMuteButton.gameObject.SetActive(false);
    //     }
    //     isMusicMuted = !isMusicMuted;
    //     MuteBackground(isMusicMuted);
    // }


    internal void PlayBackground()
    {
        if (!bgMusic) return;

        bgMusicSource.clip = bgMusic;
        bgMusicSource.loop = true;
        if (!bgMusicSource.isPlaying)
            bgMusicSource.Play();
    }

    internal void PlayBonusBackground()
    {
        if (!bonusbgMusic) return;

        bgMusicSource.clip = bonusbgMusic;
        bgMusicSource.loop = true;
        if (!bgMusicSource.isPlaying)
            bgMusicSource.Play();
    }

    internal void StopBackground()
    {
        bgMusicSource.Stop();
    }

    internal void PlayWheelArrowTick(bool loop)
    {
        PlayGame(WheelArrowTick, loop);
    }

    internal void PlayWheelBlackOverlay(bool loop)
    {
        PlayGame(WheelBlackOverlay, loop);
    }

    internal void PlayBigWin(bool loop)
    {
        PlayGame(BigWin, loop);
    }
    internal void PlayWheelArrowStop(bool loop)
    {
        PlayGame(WheelArrowStop, loop);
    }
    internal void PlayUIButton(bool loop)
    {
        PlayGame(uiButton, loop);
    }
    internal void PlayNormalWin(bool loop)
    {
        PlayGame(normalWin, loop);
    }
    internal void PlayBonusComplete(bool loop)
    {
        PlayGame(bonusComplete, loop);
    }
    internal void PlayReelHit(bool loop)
    {
        PlayGame(ReelHit, loop);
    }
    internal void PlayReelSpinning(bool loop)
    {
        PlayGame(reelSpinning, loop);
    }
    internal void PlayBonusHit(bool loop)
    {
        PlayGame(bonusHit, loop);
    }

    private void PlayGame(AudioClip clip, bool loop)
    {
        if (!clip) return;

        gameSoundSource.Stop();
        gameSoundSource.clip = clip;
        gameSoundSource.loop = loop;
        gameSoundSource.Play();
    }

    internal void StopGameAudio()
    {
        gameSoundSource.Stop();
        gameSoundSource.loop = false;
    }

    // internal void PlayUIButton()
    // {
    //     gameSoundSource.PlayOneShot(uiButton);
    // }

    // Focus-driven — called from BOTH UIManager.OnFocusChanged (JS path) and OnApplicationFocus below.
    internal void SetMuteAll(bool forceMute)
    {
        if (forceMute == isForceMuted) return;
        isForceMuted = forceMute;

        AudioSource[] sources = { bgMusicSource, gameSoundSource };
        foreach (var source in sources)
        {
            if (source == null) continue;
            if (forceMute)
            {
                preFocusMuteState[source] = source.mute;
                source.mute = true;
            }
            else
            {
                source.mute = preFocusMuteState.TryGetValue(source, out bool prevMuted) ? prevMuted : source.mute;
            }
        }
    }

    internal void MuteBackground(bool mute)
    {
        bgMusicSource.mute = mute;
        if (isForceMuted) preFocusMuteState[bgMusicSource] = mute;
    }

    internal void MuteGame(bool mute)
    {
        gameSoundSource.mute = mute;
        if (isForceMuted) preFocusMuteState[gameSoundSource] = mute;
    }
    // internal void MuteUI(bool mute) => uiSource.mute = mute;

    private void OnApplicationFocus(bool hasFocus)
    {
        SetMuteAll(!hasFocus);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        AudioListener.volume = pauseStatus ? 0.0f : 1.0f;
    }
}
