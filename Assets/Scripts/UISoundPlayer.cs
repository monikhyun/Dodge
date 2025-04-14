using UnityEngine;

public class UISoundPlayer : MonoBehaviour
{
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayClickSound()
    {
        if (audioSource != null && audioSource.clip != null)
            audioSource.Play();
    }
}