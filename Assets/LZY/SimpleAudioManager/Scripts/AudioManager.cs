using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LZY.SimpleAudioManager
{
    public class AudioManager : MonoBehaviour
    {
        private static AudioManager _instance;

        [SerializeField] private AudioBankConfig audioBankConfig;
        [SerializeField] private AudioSource BGMAudioSource;
        [SerializeField] private int SFXAudioSourceSize = 10;
        [SerializeField] private int highPrioritySFXCount = 5;
        [SerializeField, Range(0, 1f)] private float lowVolumePercentage = .3f;
        [SerializeField, Range(0, 1f)] private float lowVolumeFadeDuration = .5f;

        private Stack<AudioSourceData> SFXAudioSources = new Stack<AudioSourceData>();
        private List<AudioSourceData> highPrioritySFXAudioSources = new List<AudioSourceData>();

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
            PopulateSFXAudioSources();
        }

        private void PopulateSFXAudioSources()
        {
            var remainingSize = Mathf.Max(0, SFXAudioSourceSize - SFXAudioSources.Count);
            for (int i = 0; i < remainingSize; i++)
                SpawnAudioSource();
        }

        private void SpawnAudioSource()
        {
            var audioSource = new GameObject("SFXAudioSource").AddComponent<AudioSource>();
            audioSource.transform.SetParent(transform);
            SFXAudioSources.Push(new AudioSourceData() { source = audioSource });
        }

        public static void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            _instance?.InternalPlaySFX(clip, volume, pitch);
        }

        public static void PlaySFX(AudioData data)
        {
            _instance?.InternalPlaySFX(data);
        }

        public static void PlaySFX(string id)
        {
            _instance?.InternalPlaySFX(id);
        }

        private void InternalPlaySFX(string id)
        {
            if (audioBankConfig.AudioMap.ContainsKey(id))
            {
                InternalPlaySFX(audioBankConfig.AudioMap[id]);
            }
        }
        
        private void InternalPlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            StartCoroutine(InternalPlaySFXCoroutine(clip, volume, pitch));
        }

        private void InternalPlaySFX(AudioData data)
        {
            StartCoroutine(InternalPlaySFXCoroutine(data));
        }

        private IEnumerator InternalPlaySFXCoroutine(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            if (clip == null) yield break;
            
            var audioSource = GetAudioSourceData();
            if (audioSource == null) yield break;
            
            yield return InternalPlayAudioSourceSFXCoroutine(audioSource, clip, volume, pitch);
        }

        private IEnumerator InternalPlaySFXCoroutine(AudioData data)
        {
            var clip = data.GetRandomizedClip();
            if (clip == null) yield break;
            
            var audioSource = GetAudioSourceData();
            if (audioSource == null) yield break;
            
            if (data.delay > 0)
                yield return new WaitForSeconds(data.delay);
            
            yield return InternalPlayAudioSourceSFXCoroutine(audioSource, clip, data.GetProcessedVolume(), data.GetProcessedPitch());
        }

        private IEnumerator InternalPlayAudioSourceSFXCoroutine(AudioSourceData audioSource, AudioClip clip, float volume, float pitch)
        {
            if (audioSource.fadeTween != null && audioSource.fadeTween.active)
                audioSource.fadeTween.Kill();
            
            audioSource.source.clip = clip;
            audioSource.source.pitch = pitch;
            audioSource.source.volume = volume;
            audioSource.source.Play();

            if (highPrioritySFXAudioSources.Count >= highPrioritySFXCount)
            {
                var lowPriorityAudioSource = highPrioritySFXAudioSources[0];
                highPrioritySFXAudioSources.RemoveAt(0);
                
                var lowVolume = lowPriorityAudioSource.source.volume * lowVolumePercentage;
                lowPriorityAudioSource.fadeTween = DOTween.To(() => lowPriorityAudioSource.source.volume, x => lowPriorityAudioSource.source.volume = x, lowVolume, lowVolumeFadeDuration);
            }
            highPrioritySFXAudioSources.Add(audioSource);

            yield return new WaitForSeconds(clip.length);

            SFXAudioSources.Push(audioSource);
        }

        private AudioSourceData GetAudioSourceData()
        {
            var isAvailable = SFXAudioSources.Count > 0;
            if (!isAvailable)
                SpawnAudioSource();

            return SFXAudioSources.Pop();
        }
    }

    [Serializable]
    public class AudioSourceData
    {
        public AudioSource source;
        public Tween fadeTween;
    }

    [Serializable]
    public class AudioData
    {
        public string id;
        public AudioClip[] clips;
        public float delay;
        
        [Header("Volume")]
        public float volume = 1f;
        public bool isRandomVolume;
        public float randomMinVolume = 1f; 
        public float randomMaxVolume = 1f;
        
        [Header("Pitch")]
        public float pitch = 1f;
        public bool isRandomPitch;
        public float randomMinPitch = 1f;
        public float randomMaxPitch = 1f;

        public AudioClip GetRandomizedClip()
        {
            return clips.Length > 0 ? clips[Random.Range(0, clips.Length)] : null;
        }
        
        public float GetProcessedVolume()
        {
            return isRandomVolume ? Random.Range(randomMinVolume, randomMaxVolume) : volume;
        }
        
        public float GetProcessedPitch()
        {
            return isRandomPitch ? Random.Range(randomMinPitch, randomMaxPitch) : pitch;
        }
    }   
}