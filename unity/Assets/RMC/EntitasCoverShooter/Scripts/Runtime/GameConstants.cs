//Uncomment to remove debugging functionality
//#define ENTITAS_DISABLE_VISUAL_DEBUGGING
using RMC.Common.Entitas.Utilities;
using RMC.Common.UnityEngineReplacement;
using RMC.Common.Utilities;

namespace RMC.EntitasCoverShooter.Entitas
{
	/// <summary>
	/// Easy access to commonly changeable configuration
	/// </summary>
	public class GameConstants
	{
		// ------------------ Constants and statics
        public static readonly Vector3 BallFriction = new Vector3 (0, 0.03f, 0);
        public static readonly float PaddleFrictionY = 0.5f; //moving up during collision makes the ball go 'up'
        public static readonly float PaddleBounceAmountX = -1.1f; //ball bounces with higher speed each collision
        public static readonly float BallInitialVelocityMinX = 30;
        public static readonly float BallInitialVelocityMaxX = 50;
        public static readonly float BallInitialVelocityMinY = 0;
        public static readonly float BallInitialVelocityMaxY = 20;

        //
        public const float AudioVolume = 0.5f;//default
        //
        public const string Audio_ButtonClickSuccess = "Audio/SoundEffects/ButtonClickSuccess";
        public const string Audio_Collision = "Audio/SoundEffects/Collision";
        public const string Audio_GoalFailure = "Audio/SoundEffects/GoalFailure";
        public const string Audio_GoalSuccess = "Audio/SoundEffects/GoalSuccess";

		// ------------------ Events

		// ------------------ Serialized fields and properties

		// ------------------ Non-serialized fields

		// ------------------ Methods

        /// <summary>
        /// Return random starting velocity. Called for each StartNextRound
        /// </summary>
        public static Vector3 GetBallInitialVelocity ()
        {
            return new Vector3
            (
                    GameUtility.GetRandomRangePosNeg (BallInitialVelocityMinX, BallInitialVelocityMaxX),
                    GameUtility.GetRandomRangePosNeg (BallInitialVelocityMinY, BallInitialVelocityMaxY),
                0
            );

        }



	}
}