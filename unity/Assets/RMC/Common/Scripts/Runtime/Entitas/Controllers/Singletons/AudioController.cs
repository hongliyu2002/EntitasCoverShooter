using RMC.Common.Singleton;
using Entitas;
using UnityEngine;
using System.Collections.Generic;


namespace RMC.Common.Entitas.Controllers.Singleton
{
	/// <summary>
	/// Converts Entity's with related Component to Unity Audio
	/// </summary>
    public class AudioController : SingletonMonobehavior<AudioController> 
	{
        // ------------------ Constants and statics

        // ------------------ Events

        // ------------------ Serialized fields and properties
        private Group _soundGroup;
        private Entity _gameEntity;
        private AudioSource _audioSource;
        private Dictionary<string, AudioClip> _audioClipDictionary;

        // ------------------ Methods

        // ------------------ Non-serialized fields

        // ------------------ Methods

        protected void Start()
        {
            //NOTE: One AudioSource = Limitation of one sound playing concurrently. Ok for demo
            _audioSource = gameObject.AddComponent<AudioSource>();
            AudioController.OnDestroying += AudioController_OnDestroying;
            _audioClipDictionary = new Dictionary<string, AudioClip>();

            //
            _soundGroup = Pools.pool.GetGroup(Matcher.AllOf(Matcher.PlayAudio));
            _soundGroup.OnEntityAdded += SoundGroup_OnEntityAdded;

            //By design: Start() happens after core entities are created, so no need to wait to access
            _gameEntity = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.AudioSettings)).GetSingleEntity();
        }

        private void AudioController_OnDestroying (AudioController instance) 
        {
            AudioController.OnDestroying -= AudioController_OnDestroying;
            _soundGroup.OnEntityAdded -= SoundGroup_OnEntityAdded;
        }

        private void SoundGroup_OnEntityAdded (Group group, Entity entity, int index, IComponent component) 
        {
            if (!_gameEntity.audioSettings.isMuted)
            {
                PlaySound(entity.playAudio.audioClipName, entity.playAudio.volume);
            }

            //  The sound has been processed, so destroy the related Entity
            entity.WillDestroy(true);
        }

        private void PlaySound (string audioClipName, float volume)
        {
            AudioClip audioClip = FetchAudioClip(audioClipName);
            _audioSource.PlayOneShot(audioClip, volume);

            //Keep
            //Debug.Log ("PlaySound() " + audioClip.name);
        }


        //cache in RAM, audio clips after first use.
        private AudioClip FetchAudioClip (string audioClipName)
        {
            if (!_audioClipDictionary.ContainsKey(audioClipName))
            {
                AudioClip audioClip = Resources.Load<AudioClip>(audioClipName);
                _audioClipDictionary.Add(audioClipName, audioClip);
            }

            return _audioClipDictionary[audioClipName]; 
        }

    }


}
