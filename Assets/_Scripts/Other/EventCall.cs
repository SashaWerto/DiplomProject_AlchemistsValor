using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCall : MonoBehaviour
{
    [Header("References/Audio")]
    [SerializeField] private AudioSource _audioSource;
    public void PlaySound(AudioClip clip)
    {
        _audioSource.PlayOneShot(clip);
    }
}
