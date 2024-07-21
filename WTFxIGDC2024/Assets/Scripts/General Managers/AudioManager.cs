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

    private void Start()
    {
        _bgSource.clip = _bgMusic;
        _bgSource.playOnAwake = true;
        _bgSource.Play();
    }

    public void PlayRepairSFX()
    {
        AudioSource.PlayClipAtPoint(_repairSFX, Camera.main.transform.position);
    }
    public void PlayDeploySFX()
    {
        AudioSource.PlayClipAtPoint(_deploySFX, Camera.main.transform.position);

    }
    public void PlayDamagedSFX()
    {
        AudioSource.PlayClipAtPoint(_damagedSFX, Camera.main.transform.position);

    }
}
