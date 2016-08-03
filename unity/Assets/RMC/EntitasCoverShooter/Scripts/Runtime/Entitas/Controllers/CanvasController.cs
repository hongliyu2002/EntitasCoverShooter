using UnityEngine;
using Entitas;
using UnityEngine.UI;
using System;
using RMC.EntitasCoverShooter.Entitas.Controllers.Singleton;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using RMC.Common.Utilities;
using RMC.Common.Singleton;

namespace RMC.EntitasCoverShooter.Entitas.Controllers
{
	/// <summary>
	/// Bridges the Unity GUI and the Entitas
	/// </summary>
    public class CanvasController : SingletonMonobehavior<CanvasController>
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
        public Text _scoreText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private  Button _pauseButton;
        [SerializeField] private  Button _muteButton;
        [SerializeField] private  Button _standButton;
        public float StandButtonTopY { get { return _standButton.GetComponent<RectTransform>().offsetMax.y; } }
            

		

		// ------------------ Non-serialized fields
        private Entity _gameEntity;
        private Group _gameScore;
        private EventTrigger _standButtonEventTrigger;

		// ------------------ Methods

        protected void Start()
        {

            Group group = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Bounds, Matcher.Score));

            SetGameGroup(group);

            _restartButton.onClick.AddListener (OnRestartButtonClicked);
            _pauseButton.onClick.AddListener (OnPauseButtonClicked);
            _muteButton.onClick.AddListener (OnMuteButtonClicked);

            _standButtonEventTrigger = _standButton.GetComponent<EventTrigger>();
            EventSystemUtility.AddEventTrigger(_standButtonEventTrigger, OnStandButtonPointerDown, EventTriggerType.PointerDown);
            EventSystemUtility.AddEventTrigger(_standButtonEventTrigger, OnStandButtonPointerUp, EventTriggerType.PointerUp);

        }



        private void OnStandButtonPointerDown()
        {
            GameController.Instance.OnStandButtonPointerDown();
        }

        private void OnStandButtonPointerUp()
        {
            GameController.Instance.OnStandButtonPointerUp();
        }


        protected void OnDestroy()
        {
            _restartButton.onClick.RemoveListener (OnRestartButtonClicked);
            _pauseButton.onClick.RemoveListener (OnPauseButtonClicked);
            _muteButton.onClick.RemoveListener (OnMuteButtonClicked);
            EventSystemUtility.RemoveAllEventTriggers(_standButtonEventTrigger);

        }

        private void SetGameGroup (Group group)
        {
            //The group should have 1 thing, always, but...
            //FIXME: after multiple restarts (via 'r' button in HUD) this fails - srivello
            if (group.count == 1) 
            {
                _gameEntity = group.GetSingleEntity();
                _gameEntity.OnComponentReplaced += Entity_OnComponentReplaced;

                //set first value
                var scoreComponent = _gameEntity.score;
                SetText (scoreComponent.whiteScore, scoreComponent.blackScore, _gameEntity.time.timeSinceGameStartUnpaused);

            }
            else
            {
                Debug.LogWarning ("CC _scoreGroup failed, should have one entity, has count: " + group.count);    
            }
        }

        private void Entity_OnComponentReplaced(Entity entity, int index, IComponent previousComponent, IComponent newComponent)
        {
            SetText(entity.score.whiteScore, entity.score.blackScore, entity.time.timeSinceGameStartUnpaused);
            SetPauseButtonColor(entity.time.isPaused);
            SetMuteButtonColor(entity.audioSettings.isMuted);
        }

        private void SetText(int whiteScore, int blackScore, float time)
        {
            _scoreText.text = string.Format ("White: {0}\t\tBlack: {1}\t\tTime: {2:000}", whiteScore, blackScore, time);
        }


        /// <summary>
        /// We update the View (GUI) only when the model changes. Best practice!
        /// </summary>
        private void SetPauseButtonColor (bool isDark)
        {
            if (isDark)
            {
                _pauseButton.GetComponent<Image>().color = Color.gray;
            }
            else
            {
                _pauseButton.GetComponent<Image>().color = Color.white;
            }

        }

        /// <summary>
        /// We update the View (GUI) only when the model changes. Best practice!
        /// </summary>
        private void SetMuteButtonColor (bool isDark)
        {
            if (isDark)
            {
                _muteButton.GetComponent<Image>().color = Color.gray;
            }
            else
            {
                _muteButton.GetComponent<Image>().color = Color.white;
            }
        }
            
		private void OnRestartButtonClicked()
        {
           GameController.Instance.Restart();
        }
		private void OnPauseButtonClicked()
        {
           GameController.Instance.TogglePause();
        }
        private void OnMuteButtonClicked()
        {
            GameController.Instance.ToggleMute();
        }



	}
}
