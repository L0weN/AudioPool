using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mert.AudioSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundEmitter : MonoBehaviour
    {
        public SoundData SoundData { get; private set; }
        public LinkedListNode<SoundEmitter> Node { get; set; }

        AudioSource audioSource;
        Coroutine playingCoroutine;

        private void Awake()
        {
            audioSource = gameObject.GetOrAdd<AudioSource>();
        }

        public void Initialize(SoundData soundData)
        {
            SoundData = soundData;
            audioSource.clip = soundData.clip;
            audioSource.outputAudioMixerGroup = soundData.mixerGroup;
            audioSource.loop = soundData.loop;
            audioSource.playOnAwake = soundData.playOnAwake;

            audioSource.mute = soundData.mute;
            audioSource.bypassEffects = soundData.bypassEffects;
            audioSource.bypassListenerEffects = soundData.bypassListenerEffects;
            audioSource.bypassReverbZones = soundData.bypassReverbZones;

            audioSource.priority = soundData.priority;
            audioSource.volume = soundData.volume;
            audioSource.pitch = soundData.pitch;
            audioSource.panStereo = soundData.panStereo;
            audioSource.spatialBlend = soundData.spatialBlend;
            audioSource.reverbZoneMix = soundData.reverbZoneMix;
            audioSource.dopplerLevel = soundData.dopplerLevel;
            audioSource.spread = soundData.spread;

            audioSource.minDistance = soundData.minDistance;
            audioSource.maxDistance = soundData.maxDistance;

            audioSource.ignoreListenerVolume = soundData.ignoreListenerVolume;
            audioSource.ignoreListenerPause = soundData.ignoreListenerPause;

            audioSource.rolloffMode = soundData.rolloffMode;
        }

        public void Play()
        {
            if (playingCoroutine != null)
            {
                StopCoroutine(playingCoroutine);
            }

            audioSource.Play();
            playingCoroutine = StartCoroutine(WaitForSoundToEnd());
        }

        IEnumerator WaitForSoundToEnd()
        {
            yield return new WaitWhile(() => audioSource.isPlaying);
            SoundManager.Instance.ReturnToPool(this);
        }

        public void Stop()
        {
            if (playingCoroutine != null)
            {
                StopCoroutine(playingCoroutine);
                playingCoroutine = null;
            }

            audioSource.Stop();
            SoundManager.Instance.ReturnToPool(this);
        }

        public void WithRandomPitch(float min = -0.05f, float max = 0.05f)
        {
            audioSource.pitch += Random.Range(min, max);
        }
    }
}
