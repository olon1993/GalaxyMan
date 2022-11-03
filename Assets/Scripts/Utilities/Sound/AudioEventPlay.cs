using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheFrozenBanana
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioEventPlay : MonoBehaviour
    {
        [SerializeField] AudioSource audioData;

        void Start()
        {
            audioData = GetComponent<AudioSource>();
            audioData.Play(0);
            Debug.Log("started");
        }

        void OnGUI()
        {
            if (GUI.Button(new Rect(10, 70, 150, 30), "Pause"))
            {
                audioData.Pause();
                Debug.Log("Pause: " + audioData.time);
            }

            if (GUI.Button(new Rect(10, 170, 150, 30), "Continue"))
            {
                audioData.UnPause();
            }
        }
    }
}

