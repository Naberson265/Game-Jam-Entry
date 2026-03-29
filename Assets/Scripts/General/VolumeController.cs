using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer musicAudioMixer;
    [SerializeField] private AudioMixer sfxAudioMixer;
    [SerializeField] private AudioMixer voiceAudioMixer;

    public static int suppressMusic = 0;
    [SerializeField] private float suppressionAmount;

    private void Start()
    {
        musicAudioMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
    }
    // This is a very unfinished script, so you'll probably need to change how the noise suppression works when you Implement volume Settings
    void Update()
    {
        sfxAudioMixer.SetFloat("SfxVolume", PlayerPrefs.GetFloat("SFXVolume"));
        voiceAudioMixer.SetFloat("VoicelineVolume", PlayerPrefs.GetFloat("VoiceVolume"));
        

        float musicNormal = PlayerPrefs.GetFloat("MusicVolume");
        if (musicNormal <= -39.9f) {
            musicNormal = -80;
        }
        float currentVolume;
        musicAudioMixer.GetFloat("MusicVolume", out currentVolume);
        if (suppressMusic > 0)
        {
            musicAudioMixer.SetFloat("MusicVolume", Mathf.Lerp(currentVolume, musicNormal + suppressionAmount, 0.01f));
        } else
        {
            musicAudioMixer.SetFloat("MusicVolume", Mathf.Lerp(currentVolume, musicNormal, 0.01f));
        }
    }
}
