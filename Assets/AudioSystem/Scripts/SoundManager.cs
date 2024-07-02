using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Mert.AudioSystem
{
    public class SoundManager : PersistentSingleton<SoundManager>
    {
        IObjectPool<SoundEmitter> soundEmitterPool;
        readonly List<SoundEmitter> activeSoundEmitters = new List<SoundEmitter>();
        public readonly LinkedList<SoundEmitter> FrequentSoundEmitters = new LinkedList<SoundEmitter>();

        [SerializeField] SoundEmitter soundEmitterPrefab;
        [SerializeField] bool collectionCheck = true;
        [SerializeField] int defaultCapacity = 10;
        [SerializeField] int maxPoolSize = 100;
        [SerializeField] int maxSoundInstances = 30;

        private void Start()
        {
            InitializePool();
        }

        private void InitializePool()
        {
            soundEmitterPool = new ObjectPool<SoundEmitter>(CreateSoundEmitter, OnTakeFromPool, OnReturnedPool, OnDestroyPoolObject, collectionCheck, defaultCapacity, maxPoolSize);
        }

        public SoundBuilder CreateSound() => new SoundBuilder(this);

        public bool CanPlaySound(SoundData soundData)
        {
            if (!soundData.frequentSound) return true;

            if (FrequentSoundEmitters.Count >= maxSoundInstances)
            {
                try
                {
                    FrequentSoundEmitters.First.Value.Stop();
                    return true;
                }
                catch
                {
                    Debug.Log("SoundEmitter is already released");
                }
                return false;
            }
            return true;
        }

        public SoundEmitter Get() => soundEmitterPool.Get();

        public void ReturnToPool(SoundEmitter soundEmitter) => soundEmitterPool.Release(soundEmitter);

        SoundEmitter CreateSoundEmitter()
        {
            var soundEmitter = Instantiate(soundEmitterPrefab);
            soundEmitter.gameObject.SetActive(false);
            return soundEmitter;
        }

        void OnTakeFromPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(true);
            activeSoundEmitters.Add(soundEmitter);
        }

        void OnReturnedPool(SoundEmitter soundEmitter)
        {
            soundEmitter.gameObject.SetActive(false);
            activeSoundEmitters.Remove(soundEmitter);
        }

        void OnDestroyPoolObject(SoundEmitter soundEmitter)
        {
            Destroy(soundEmitter.gameObject);
        }
    }
}
