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
			_pool.CreateEntity().willStartNextRound = true;

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
            Entity whitePaddleEntity            = _pool.CreateEntity ();
            whitePaddleEntity.AddPaddle         (PaddleComponent.PaddleType.White);
            whitePaddleEntity.AddResource       ("Prefabs/PaddleWhite");
            whitePaddleEntity.AddVelocity       (RMC.Common.UnityEngineReplacement.Vector3.zero);
            whitePaddleEntity.AddFriction       (RMC.Common.UnityEngineReplacement.Vector3.zero);
            whitePaddleEntity.WillAcceptInput   (true);
            whitePaddleEntity.AddTick           (0);
            whitePaddleEntity.OnComponentAdded += OnWhitePaddleComponentAdded;

            //  Create computer player on the left
            Entity blackPaddleEntity        = _pool.CreateEntity ();
            blackPaddleEntity.AddPaddle     (PaddleComponent.PaddleType.Black);
            blackPaddleEntity.AddResource   ("Prefabs/PaddleBlack");
            blackPaddleEntity.AddVelocity   (RMC.Common.UnityEngineReplacement.Vector3.zero);
            blackPaddleEntity.AddFriction   (RMC.Common.UnityEngineReplacement.Vector3.zero);
            blackPaddleEntity.AddAI         (whitePaddleEntity, 1, 25f);
            blackPaddleEntity.AddTick       (0);
            blackPaddleEntity.OnComponentAdded += OnBlackPaddleComponentAdded;


		}

        /// <summary>
        /// Position depends on size, which depends on View. Wait for view.
        /// </summary>
        private void OnWhitePaddleComponentAdded (Entity entity, int index, IComponent component)
        {
            if (component.GetType() == typeof(ViewComponent))
            {
                entity.AddPosition(new RMC.Common.UnityEngineReplacement.Vector3(_screenBounds.max.x - entity.view.bounds.size.x / 2 - PaddleOffsetToEdgeX, 0, 0));
                entity.OnComponentAdded -= OnWhitePaddleComponentAdded;
            }
        }

        /// <summary>
        /// Position depends on size, which depends on View. Wait for view.
        /// </summary>
        private void OnBlackPaddleComponentAdded (Entity entity, int index, IComponent component) 
        {
            if (component.GetType() == typeof(ViewComponent))
            {
                entity.AddPosition (new RMC.Common.UnityEngineReplacement.Vector3 (_screenBounds.min.x + entity.view.bounds.size.x/2 + PaddleOffsetToEdgeX, 0, 0));
                entity.OnComponentAdded -= OnBlackPaddleComponentAdded;
            }
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

			//	'Collision' as NOT physics based - as an example
			_pausableUpdateSystems.Add (_pool.CreateSystem<BoundsBounceSystem> ());
            _pausableUpdateSystems.Add (_pool.CreateSystem<BoundsConstrainSystem> ());
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

		//ADVICE ON RESTARTING: https://github.com/sschmid/Entitas-CSharp/issues/82
		public void Restart ()
		{
            
            SetPause(true);
            _pool.CreateEntity().AddPlayAudio(GameConstants.Audio_ButtonClickSuccess, GameConstants.AudioVolume);
            CoroutineUtility.Instance.StartCoroutineAfterDelay(Restart_Coroutine(), 0.25f);
			
		}

        //Add small pause so we hear the click sound
        private IEnumerator Restart_Coroutine ()
        {
            GameController.Destroy();
            return null;
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
