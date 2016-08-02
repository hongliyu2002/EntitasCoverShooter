using UnityEngine;
using Entitas;
using UnityEngine.UI;
using System;
using RMC.EntitasCoverShooter.Entitas.Controllers.Singleton;

namespace RMC.EntitasCoverShooter.Entitas.Controllers
{
	/// <summary>
	/// Bridges the Unity GUI and the Entitas
	/// </summary>
	public class CanvasController : MonoBehaviour
	{
		// ------------------ Constants and statics

		// ------------------ Events

		// ------------------ Serialized fields and properties
        public Text _scoreText;
        public Button _restartButton;
        public Button _pauseButton;
        public Button _muteButton;
		

		// ------------------ Non-serialized fields
        private Entity _gameEntity;
        private Group _gameScore;

		// ------------------ Methods

        protected void Start()
        {

            Group group = Pools.pool.GetGroup(Matcher.AllOf(Matcher.Game, Matcher.Bounds, Matcher.Score));

            SetGameGroup(group);

            _restartButton.onClick.AddListener (OnRestartButtonClicked);
            _pauseButton.onClick.AddListener (OnPauseButtonClicked);
            _muteButton.onClick.AddListener (OnMuteButtonClicked);

        }


        protected void OnDestroy()
        {
            _restartButton.onClick.RemoveListener (OnRestartButtonClicked);
            _pauseButton.onClick.RemoveListener (OnPauseButtonClicked);
            _muteButton.onClick.RemoveListener (OnMuteButtonClicked);

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
