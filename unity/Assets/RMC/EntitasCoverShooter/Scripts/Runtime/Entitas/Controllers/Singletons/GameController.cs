using UnityEngine;
using Entitas;
using Entitas.Unity.VisualDebugging;
using RMC.Common.Entitas.Systems.Render;
using RMC.Common.Entitas.Systems.Transform;
using RMC.EntitasCoverShooter.Entitas.Systems.Collision;
using RMC.Common.Entitas.Systems;

// This is required because the entitas class path is similar to my namespaces. This prevents collision - srivello
using Feature = Entitas.Systems;
//
using RMC.Common.Entitas.Systems.Destroy;
using RMC.Common.Singleton;
using UnityEngine.SceneManagement;
using RMC.Common.Entitas.Systems.GameState;
using RMC.EntitasCoverShooter.Entitas.Components;
using RMC.Common.Entitas.Controllers.Singleton;
using System.Collections;
using RMC.Common.Entitas.Utilities;
using EntitasSystems = Entitas.Systems;
using RMC.EntitasCoverShooter.Entitas.Systems.GameState;
using RMC.EntitasCoverShooter.Entitas.Systems;
using RMC.Common.Utilities;
using RMC.Common.Entitas.Components.Render;
using RMC.Common.Entitas.Components.Input;

namespace RMC.EntitasCoverShooter.Entitas.Controllers.Singleton
{
	/// <summary>
	/// The main entry point for the game. Creates the Entitas setup
	/// </summary>
    public class GameController : SingletonMonobehavior<GameController> 
	{
		// ------------------ Constants and statics
        private const float PaddleOffsetToEdgeX = 3;

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields
        private EntitasSystems _pausableUpdateSystems;
        private EntitasSystems _unpausableUpdateSystems;
        private EntitasSystems _pausableFixedUpdateSystems;
		private Pool _pool;
		private PoolObserver _poolObserver;
		private Entity _gameEntity;
        private RMC.Common.UnityEngineReplacement.Bounds _screenBounds;


		// ------------------ Methods

		override protected void Awake () 
		{
			base.Awake();
            Debug.Log ("GameController.Awake()");


            GameController.OnDestroying += GameController_OnDestroying;
            TickController.Instantiate();
            AudioController.Instantiate();
            InputController.Instantiate();
            ViewController.Instantiate();
            ResourceController.Instantiate();

			Application.targetFrameRate = 30;
            _screenBounds = UnityEngineReplacementUtility.Convert(GameUtility.GetOrthographicBounds(Camera.main));


			SetupPools ();
			SetupPoolObserver();

			//order matters
			//1 - Systems that depend on entities will internally listen for entity creation before reacting - nice!
			SetupSystems ();

			//2
			SetupEntities ();

			//place a ball in the middle of the screen w/ velocity
			//_pool.CreateEntity().willStartNextRound = true;

		}

		protected void Update () 
		{
			if (!_gameEntity.time.isPaused)
			{
                _pausableUpdateSystems.Execute ();
			}
            _unpausableUpdateSystems.Execute();
		}

        protected void LateUpdate () 
        {
            if (!_gameEntity.time.isPaused)
            {
                _pausableFixedUpdateSystems.Execute ();
            }
        }

        //Called during GameController.Destroy();
        private void GameController_OnDestroying (GameController instance) 
        {
            Debug.Log ("GameController.Destroy()");

            GameController.OnDestroying -= GameController_OnDestroying;

            if (AudioController.IsInstantiated())
            {
                AudioController.Destroy();
            }
            if (InputController.IsInstantiated())
            {
                InputController.Destroy();
            }
            if (ViewController.IsInstantiated())
            {
                ViewController.Destroy();
            }
            if (TickController.IsInstantiated())
            {
                TickController.Destroy();
            }
            if (ResourceController.IsInstantiated())
            {
                ResourceController.Destroy();
            }


            _pausableUpdateSystems.DeactivateReactiveSystems();
            _unpausableUpdateSystems.DeactivateReactiveSystems ();

            Pools.pool.Reset ();
            DestroyPoolObserver();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

		private void SetupPools ()
		{
			_pool = new Pool (ComponentIds.TotalComponents, 0, new PoolMetaData ("Pool", ComponentIds.componentNames, ComponentIds.componentTypes));
			
			//	TODO: Not sure why I must do this, but I must or other classes can't do pool lookups - srivello
			_pool = Pools.pool;
			


		}


		private void SetupPoolObserver()
		{

			//	Optional debugging (Its helpful.)
#if (!ENTITAS_DISABLE_VISUAL_DEBUGGING && UNITY_EDITOR)
			//TODO: Sometimes there are two of these in the hierarchy. Prevent that - srivello
			PoolObserverBehaviour poolObserverBehaviour = GameObject.FindObjectOfType<PoolObserverBehaviour>();
			if (poolObserverBehaviour == null)
			{
				_poolObserver = new PoolObserver (_pool);
				//Set as a child to unclutter hierarchy
				_poolObserver.entitiesContainer.transform.SetParent (transform);	
			}
			//Helpful if you want to see the pools in the hierarchy after you STOP the scene. Rarely needed - srivello
			//Object.DontDestroyOnLoad (poolObserver.entitiesContainer);
#endif
			
		}


		private void SetupEntities ()
		{

            //Debug.Log("GameController.SetupEntities()");
          //Debug.Log(bounds.min.y + " and " + bounds.max.y);


            //  Create game with data. This is non-visual.
			_gameEntity = _pool.CreateEntity();
            _gameEntity.IsGame(true);
			_gameEntity.AddBounds(_screenBounds);
			_gameEntity.AddScore(0,0);
			_gameEntity.AddTime (0, 0, false);
            _gameEntity.AddAudioSettings(false);
            _gameEntity.AddTick(0);

            //  Create human player on the right
            Entity playerEntity            = _pool.CreateEntity ();
            playerEntity.IsPlayer(true);
            playerEntity.AddResource       ("Prefabs/Player"); //this later adds ViewComponent
            playerEntity.AddVelocity       (RMC.Common.UnityEngineReplacement.Vector3.zero);
            playerEntity.AddFriction       (RMC.Common.UnityEngineReplacement.Vector3.zero);
            playerEntity.WillAcceptInput   (true);
            playerEntity.AddTick           (0);
            playerEntity.AddPosition(new RMC.Common.UnityEngineReplacement.Vector3(0,-1,4));

            //  Create computer player on the left
            Entity enemyEntity        = _pool.CreateEntity ();
            enemyEntity.IsEnemy(true);
            enemyEntity.AddResource   ("Prefabs/Enemy"); //this later adds ViewComponent
            enemyEntity.AddVelocity   (RMC.Common.UnityEngineReplacement.Vector3.zero);
            enemyEntity.AddFriction   (RMC.Common.UnityEngineReplacement.Vector3.zero);
            //enemyEntity.AddAI         (playerEntity, 1, 25f);
            enemyEntity.AddTick       (0);
            enemyEntity.AddPosition(new RMC.Common.UnityEngineReplacement.Vector3(0,0,-4));


		}



		private void SetupSystems ()
		{



			//a feature is a group of systems
			_pausableUpdateSystems = new Feature ();
			
			_pausableUpdateSystems.Add (_pool.CreateSystem<StartNextRoundSystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<VelocitySystem> ());
            _pausableUpdateSystems.Add (_pool.CreateSystem<AcceptInputSystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<AISystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<GoalSystem> ());
			_pausableUpdateSystems.Add (_pool.CreateSystem<DestroySystem> ());
            _pausableUpdateSystems.Add (_pool.CreateSystem<CreateBulletSystem> ());
			_pausableUpdateSystems.Initialize();
			_pausableUpdateSystems.ActivateReactiveSystems();


            _pausableFixedUpdateSystems = new Feature ();
            //  'Collision as Physics based - as an example.
            _pausableFixedUpdateSystems.Add (_pool.CreateSystem<CollisionSystem> ());
            _pausableFixedUpdateSystems.Initialize();
            _pausableFixedUpdateSystems.ActivateReactiveSystems();


			//for demo only, an example of an unpausable system
			_unpausableUpdateSystems = new Feature ();
			_unpausableUpdateSystems.Add (_pool.CreateSystem<TimeSystem> ());
			_unpausableUpdateSystems.Initialize();
			_unpausableUpdateSystems.ActivateReactiveSystems();

            // This is custom and optional. I use it to store the systems in case I need them again. 
            // This is the only place I put a component directly on a _pool. It is supported.
            // I'm not sure this is useful, but I saw something similar in Entitas presentation slides - srivello
            _pool.SetEntitas
            (
                _pausableUpdateSystems,
                _unpausableUpdateSystems,
                _pausableUpdateSystems
            );
            //Debug.Log("pausableUpdateSystems: " + Pools.pool.entitas.pausableUpdateSystems);


		}


		public void TogglePause ()
		{
            _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_ButtonClickSuccess, GameConstants.AudioVolume);
            SetPause(!_gameEntity.time.isPaused);

			//Keep
			//Debug.Log ("TogglePause() isPaused: " + _gameEntity.time.isPaused);	

		}

        public void ToggleMute ()
        {

            bool isMuted = _gameEntity.audioSettings.isMuted;

            if (isMuted)
            {
                //unmute first
                SetMute(!isMuted);
                _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_ButtonClickSuccess, GameConstants.AudioVolume);

            }
            else
            {
                //play sound first
                _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_ButtonClickSuccess, GameConstants.AudioVolume);
                SetMute(!isMuted);
            }

        }



        private void SetPause (bool isPaused)
        {
            _gameEntity.ReplaceTime
            (
                _gameEntity.time.timeSinceGameStartUnpaused, 
                _gameEntity.time.timeSinceGameStartTotal, 
                isPaused
            );
        }

        private void SetMute (bool isMute)
        {
            _gameEntity.ReplaceAudioSettings
            (
                isMute
            );
        }

        public void OnStandButtonPointerDown()
        {
            //HACK: When the GUI button is down, send a 'button' down event
            _pool.CreateEntity().AddInput (
                InputComponent.InputType.KeyCodeDown, 
                RMC.Common.UnityEngineReplacement.KeyCode.Space, 
                RMC.Common.UnityEngineReplacement.Vector2.zero);
        }

        public void OnStandButtonPointerUp()
        {
            //HACK: When the GUI button is up, send a 'button' up event
            _pool.CreateEntity().AddInput (
                InputComponent.InputType.KeyCodeUp, 
                RMC.Common.UnityEngineReplacement.KeyCode.Space, 
                RMC.Common.UnityEngineReplacement.Vector2.zero);
        }


		//ADVICE ON RESTARTING: https://github.com/sschmid/Entitas-CSharp/issues/82
		public void Restart ()
		{
            
            SetPause(true);
            _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_ButtonClickSuccess, GameConstants.AudioVolume);
            Timer.Register (0.25f, () => RestartNow());
			
		}

        //Add small pause so we hear the click sound
        private void RestartNow ()
        {
            GameController.Destroy();
        }



		private void DestroyPoolObserver()
		{
			if (_poolObserver != null)
			{
				_poolObserver.Deactivate();

				if (_poolObserver.entitiesContainer != null)
				{
					Destroy (_poolObserver.entitiesContainer);
				}

				_poolObserver = null;
			}
		}



	}


}
