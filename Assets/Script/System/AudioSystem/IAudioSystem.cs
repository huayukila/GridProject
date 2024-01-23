using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework.BuildProject
{
    internal interface IAudioSystem : ISystem
    {
        void StopAllMusic();

        /// <summary>
        ///     BGM再生
        /// </summary>
        /// <param name="audioName">BGMの名前</param>
        /// <param name="volume">音量</param>
        void PlayBGM(string audioName, float volume);

        void StopBGM();
        void PauseBGM();
        void PauseAllMusic();
        void ContinueBGM();
        void ContinueAllMusic();

        /// <summary>
        ///     普通でFX再生
        /// </summary>
        /// <param name="audioName">名前</param>
        /// <param name="volume">音量</param>
        void PlayFX(string audioName, float volume);

        /// <summary>
        ///     オブジェクトについていくのFX。まだ実装してないため、今は利用できません
        /// </summary>
        /// <param name="audioName">名前</param>
        /// <param name="volume">音量</param>
        /// <param name="obj">オブジェクト</param>
        /// <param name="isLoop">繰り返して再生のか</param>
        void PlayFX(string audioName, float volume, GameObject obj, bool isLoop);

        /// <summary>
        ///     発生場所にそのまま残るために。同じ利用できません
        /// </summary>
        /// <param name="audioName">名前</param>
        /// <param name="volume">音量</param>
        /// <param name="targetTransform">発生位置</param>
        void PlayFX(string audioName, float volume, Transform targetTransform);
    }


    public class AudioSystem : AbstractSystem, IAudioSystem
    {
        private GameObject m_AudioPool;
        private Dictionary<string, AudioClip> m_BgmAudioDic;
        private AudioSource m_BgmAudioSource;
        private Dictionary<string, AudioClip> m_FXAudioDic;
        private SimpleObjectPool<GameObject> m_FXAudioSourcePool;
        private List<AudioSource> m_PlayingMusicList;

        public void StopAllMusic()
        {
            StopBGM();
            foreach (var music in m_PlayingMusicList) music.Pause();
        }

        public void PlayBGM(string audioName, float volume)
        {
            m_BgmAudioSource.clip = m_BgmAudioDic[audioName];
            m_BgmAudioSource.volume = volume;
            m_BgmAudioSource.Play();
        }

        public void StopBGM()
        {
            m_BgmAudioSource?.Stop();
        }

        public void PlayFX(string audioName, float volume)
        {
            var fxObj = m_FXAudioSourcePool.Allocate();
            fxObj.GetComponent<FXAudioPlayer>().PlayMusic(m_FXAudioDic[audioName], volume);
            m_PlayingMusicList.Add(fxObj.GetComponent<AudioSource>());
        }

        public void PlayFX(string audioName, float volume, GameObject obj, bool isLoop)
        {
        }

        public void PlayFX(string audioName, float volume, Transform targetTransform)
        {
        }

        public void PauseBGM()
        {
            m_BgmAudioSource?.Pause();
        }

        public void PauseAllMusic()
        {
            PauseBGM();
            foreach (var music in m_PlayingMusicList) music.Pause();
        }

        public void ContinueBGM()
        {
            m_BgmAudioSource?.UnPause();
        }

        public void ContinueAllMusic()
        {
            ContinueBGM();
            foreach (var music in m_PlayingMusicList) music.UnPause();
        }

        protected override void OnInit()
        {
            m_AudioPool = new GameObject("AudioPool");
            Object.DontDestroyOnLoad(m_AudioPool);
            m_PlayingMusicList = new List<AudioSource>();
            m_FXAudioSourcePool = new SimpleObjectPool<GameObject>(() =>
            {
                var obj = new GameObject();
                var audioSource = obj.AddComponent<AudioSource>();
                obj.AddComponent<FXAudioPlayer>().Init(() =>
                {
                    m_FXAudioSourcePool.Recycle(obj);
                    m_PlayingMusicList.Remove(audioSource);
                });
                obj.transform.SetParent(m_AudioPool.transform);
                return obj;
            }, obj =>
            {
                var audioSource = obj.GetComponent<AudioSource>();
                audioSource.volume = 1.0f;
                audioSource.clip = null;
                audioSource.loop = false;
            }, 10);
            var audioDatabase = (AudioDataBase)Resources.Load("AudioDataBase");
            m_BgmAudioDic = audioDatabase.GetBGMAudioDic();
            m_FXAudioDic = audioDatabase.GetFXAudioDic();
        }
    }

    public class FXAudioPlayer : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Action _callBack;
        private bool _isPlaying;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_audioSource == null || !_isPlaying) return;
            if (_audioSource.isPlaying) return;
            _isPlaying = false;
            _callBack?.Invoke();
        }

        public void PlayMusic(AudioClip clip, float volume)
        {
            _audioSource.clip = clip;
            _audioSource.volume = volume;
            _audioSource.Play();
            _isPlaying = true;
        }

        public void Init(Action callback)
        {
            _callBack = callback;
        }
    }
}