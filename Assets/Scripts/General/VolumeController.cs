using UnityEngine;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private AudioMixer musicAudioMixer;

    public static int suppressMusic = 0;
    [SerializeField] private float suppressionAmount;

    // This is a very unfinished script, so you'll probably need to change how the noise suppression works when you Implement volume Settings
    void Update()
    {
        float currentVolume;
        musicAudioMixer.GetFloat("MusicVolume", out currentVolume);
        if (suppressMusic > 0)
        {
            musicAudioMixer.SetFloat("MusicVolume", Mathf.Lerp(currentVolume, suppressionAmount, 0.01f));
        } else
        {
            musicAudioMixer.SetFloat("MusicVolume", Mathf.Lerp(currentVolume, 0, 0.01f));
        }
    }
}
