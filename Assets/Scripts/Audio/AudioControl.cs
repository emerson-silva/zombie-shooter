using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControl : MonoBehaviour
{
    private AudioSource audioSource;
    public static AudioSource asInstance;

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        asInstance = audioSource;
    }
}
