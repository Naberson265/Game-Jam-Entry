using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource audioClip;
    [SerializeField] private bool triggerOnce = true;
    [SerializeField] private bool turnDownMusic = true;
    private bool triggered = false;

    private float audioTime;
    private bool suppressing;

    private bool defaultActivation = false;

    private void Update()
    {
        audioTime -= Time.deltaTime;
        //Recommend making a VolumeController
        if (audioTime < 0 && suppressing)
        {
            VolumeController.suppressMusic -= 1;
            suppressing = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3 && (!triggered || !triggerOnce))
        {
            triggered = true;
            audioTime = audioClip.clip.length;
            audioClip.Play();
            if (audioClip.isPlaying && turnDownMusic)
            {
                VolumeController.suppressMusic += 1;
                suppressing = true;
            }
        }
    }
}
