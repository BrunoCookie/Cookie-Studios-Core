using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using Utility;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        private static Coroutine levelMusicPitchFade;
        private static IDictionary<string, Coroutine> volumeFadeDict = new Dictionary<string, Coroutine>();

        public Sound[] sounds;
        public AudioMixerGroup mixer;
        public AudioMixer mainMixer;

        private static readonly HashSet<Sound> ExistingSounds = new HashSet<Sound>();
        
        private static readonly HashSet<string> AudioAdditions = new HashSet<string>();
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            foreach (Sound s in sounds)
            {
                CreateSoundAudioSource(s);
            }
        }

        private void CreateSoundAudioSource(Sound s)
        {
            Sound exists = ExistingSounds.FirstOrDefault(sound => sound.name == s.name);
            if(exists != null)
            {
                return;
            }
            
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixer;

            ExistingSounds.Add(s);
        }

        private void DestroySoundAudioSource(Sound s)
        {
            ExistingSounds.Remove(s);
            Destroy(s.source);
        }

        public void CreateAudioAddition(Sound s)
        {
            AudioAdditions.Add(s.name);
            CreateSoundAudioSource(s);
        }

        public void ClearAudioAdditions()
        {
            foreach (var addition in AudioAdditions)
            {
                DestroySoundAudioSource(ExistingSounds.FirstOrDefault(sound => sound.name == addition));
            }
            AudioAdditions.Clear();
        }

        private void Start()
        {
            mainMixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("MusicVolume"));
            mainMixer.SetFloat("SFXVolume", PlayerPrefs.GetFloat("SFXVolume"));
        }

        public AudioSource Play(string name)
        {
            Sound s = ExistingSounds.FirstOrDefault(sound => sound.name == name);
            s.source.Play();
            return s.source;
        }

        public void Stop(string name)
        {
            Sound s = ExistingSounds.FirstOrDefault(sound => sound.name == name);
            s.source.Stop();
        }

        public bool isPlaying(string name)
        {
            Sound s = ExistingSounds.FirstOrDefault(sound => sound.name == name);
            return s.source.isPlaying;
        }

        public void SetVolume(string name, float _volume)
        {
            Sound s = ExistingSounds.FirstOrDefault(sound => sound.name == name);
            s.source.volume = _volume;
        }

        public AudioSource CreateOnetimeSound(string name)
        {
            Sound s = ExistingSounds.FirstOrDefault(sound => sound.name == name);
            if (s == null) return null;
            AudioSource sfx = gameObject.AddComponent<AudioSource>();
            sfx.clip = s.clip;
            sfx.volume = s.volume;
            sfx.pitch = s.pitch;
            sfx.outputAudioMixerGroup = mixer;

            sfx.Play();
            Destroy(sfx, s.clip.length);
            return sfx;
        }

        public AudioSource CreateOnetimeSound(string name, float delay)
        {
            Sound s = ExistingSounds.FirstOrDefault(sound => sound.name == name);
            if (s == null) return null;
            AudioSource sfx = gameObject.AddComponent<AudioSource>();
            sfx.clip = s.clip;
            sfx.volume = s.volume;
            sfx.pitch = s.pitch;
            sfx.outputAudioMixerGroup = mixer;

            this.Invoke(() => sfx.Play(), delay);
            Destroy(sfx, s.clip.length + delay);
            return sfx;
        }

        public static void FadeVolume(AudioSource source, float seconds, float desiredVolume)
        {
            if (volumeFadeDict.TryGetValue(source.clip.name, out var coroutine))
            {
                if (coroutine != null)
                    instance.StopCoroutine(coroutine);

                volumeFadeDict.Remove(source.clip.name);
            }

            volumeFadeDict.Add(source.clip.name,
                instance.StartCoroutine(VolumeInterpolation(source, seconds, desiredVolume)));
        }

        private static IEnumerator VolumeInterpolation(AudioSource source, float seconds, float desiredVolume)
        {
            desiredVolume = Mathf.Clamp(desiredVolume, 0, 1);
            float deltaVolume = desiredVolume - source.volume;
            float startVolume = source.volume;
            while (source.volume != desiredVolume)
            {
                if (Time.timeScale != 0) source.volume += (deltaVolume / seconds) * Time.deltaTime;
                if (desiredVolume < startVolume)
                    source.volume = Mathf.Clamp(source.volume, desiredVolume, source.volume);
                else source.volume = Mathf.Clamp(source.volume, source.volume, desiredVolume);
                yield return null;
            }

            volumeFadeDict.Remove(source.clip.name);
        }

        public static void FadeLevelMusicPitch(float seconds, float desiredPitch)
        {
            if (levelMusicPitchFade != null)
            {
                instance.StopCoroutine(levelMusicPitchFade);
            }

            levelMusicPitchFade = instance.StartCoroutine(LevelMusicPitchInterpolation(seconds, desiredPitch));
        }

        private static IEnumerator LevelMusicPitchInterpolation(float seconds, float desiredPitch)
        {
            instance.mainMixer.GetFloat("MusicPitch", out var pitch);
            desiredPitch = Mathf.Clamp(desiredPitch, -3, 3);
            if (desiredPitch < pitch) seconds *= -1;
            float startPitch = pitch;
            while (pitch != desiredPitch)
            {
                if (Time.timeScale != 0) pitch += (Time.deltaTime * (1f / Time.timeScale)) / seconds;
                if (desiredPitch < startPitch) pitch = Mathf.Clamp(pitch, desiredPitch, pitch);
                else pitch = Mathf.Clamp(pitch, pitch, desiredPitch);
                instance.mainMixer.SetFloat("MusicPitch", pitch);
                yield return null;
            }

            levelMusicPitchFade = null;
        }
    }
}