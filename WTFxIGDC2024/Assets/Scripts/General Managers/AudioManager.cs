using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    [SerializeField] private AudioSource _bgSource;
    [SerializeField] private AudioSource _sfxAudioSource;

    [SerializeField] private AudioClip _bgMusic;
    [SerializeField] private AudioClip _repairSFX;
    [SerializeField] private AudioClip _deploySFX;
    [SerializeField] private AudioClip _damagedSFX;


    public bool MusicStatus = true;
    public bool SFXStatus = true;

    private void Start()
    {
        _bgSource.clip = _bgMusic;
        _bgSource.playOnAwake = true;
        _bgSource.Play();
    }

    public void TurnMusic()
    {
        if (MusicStatus)
        {
            _bgSource.clip = _bgMusic;
            _bgSource.playOnAwake = true;
            _bgSource.Play();
        }
        else
        {

            _bgSource.Stop();
        }
    }

    public void PlayRepairSFX()
    {
        if (SFXStatus)
            AudioSource.PlayClipAtPoint(_repairSFX, Camera.main.transform.position);
    }
    public void PlayDeploySFX()
    {
        if (SFXStatus)
            AudioSource.PlayClipAtPoint(_deploySFX, Camera.main.transform.position);

    }
    public void PlayDamagedSFX()
    {
        if (SFXStatus)
            AudioSource.PlayClipAtPoint(_damagedSFX, Camera.main.transform.position);

    }
}
