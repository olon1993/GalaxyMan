using System;
using UnityEngine;

namespace TheFrozenBanana
{
    public class AudioEvents
    {
        public static event Action<AudioClip> PlaySoundClip;

        public static void CallPlaySoundClip(AudioClip clip)
        {
            PlaySoundClip?.Invoke(clip);
        }
    }

}