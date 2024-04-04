using UnityEngine;
#if UNITY_EDITOR // makes intent clear.
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "Player Data")] //Create a new playerData object by right clicking in the Project Menu then Create/Player/Player Data and drag onto the player
public class PlayerData : ScriptableObject
{
	[Header("Gravity")]
	[HideInInspector] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
	[HideInInspector] public float gravityScale; //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D).
																							 //Also the value the player's rigidbody2D.gravityScale is set to.
	[Space(5)]
	public float fallGravityMult; //Multiplier to the player's gravityScale when falling.
	public float maxFallSpeed; //Maximum fall speed (terminal velocity) of the player when falling.
	[Space(5)]
	public float maxFastFallSpeed; //Maximum fall speed(terminal velocity) of the player when performing a faster fall.

	[Header("Run")]
	public float runMaxSpeed; //Target speed we want the player to reach.
	public float runAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
	[HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
	public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
	[HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
	[Space(5)]
	[Range(0f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
	[Range(0f, 1)] public float deccelInAir;
	[Space(5)]
	public bool doConserveMomentum = true;

	[Header("Jump")]
	public float jumpHeight; //Height of the player's jump
	public float jumpTimeToApex; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
	[HideInInspector] public float jumpForce; //The actual force applied (upwards) to the player when they jump.
	[Space(5)]
		
	public float jumpCutGravityMult; //Multiplier to the player's gravityScale when the jump button is released early.
	[Range(0f, 1)] public float jumpHangGravityMult; //Multiplier to the reduce gravity close to the apex of the jump
	public float jumpHangTimeThreshold; //Speeds where the player is at the apex of the jump
	public float jumpHangAccelerationMult; //Multiplier to the player's acceleration when at the apex of the jump
	public float jumpHangMaxSpeedMult; //Multiplier to the player's max speed when at the apex of the jump

	[Header("Climb")]
	public float verticalClimbSpeed; //Speed at which the player climbs up and down the tree
	public float horizontalClimbSpeed; //Speed at which the player moves left and right while climbing

	[Header("Glide")]
	[Range(0.01f, 1)] public float glideGravityScale; //Strength of the player's gravity as a multiplier of gravity (set in ProjectSettings/Physics2D) when gliding.
	public float glideMaxFallSpeed; //Maximum fall speed (terminal velocity) of the player when gliding.
	public float glideMaxSpeed; //Maximum speed of the player when gliding.
	public float glideAcceleration; //The speed at which our player accelerates to max speed when gliding.
	[HideInInspector] public float glideAccelAmount; //The actual force (multiplied with speedDiff) applied to the player when gliding.
	public float glideDecceleration; //The speed at which our player decelerates from their current speed when gliding.
	[HideInInspector] public float glideDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player when gliding.


	[Header("Assists")]
	[Range(0.01f, 0.5f)] public float coyoteTime; //Grace period after falling off a platform, where you can still jump
	[Range(0.01f, 0.5f)] public float jumpInputBufferTime; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.



	//Unity Callback, called when the inspector updates
	private void OnValidate()
	{
		gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);
		gravityScale = gravityStrength / Physics2D.gravity.y;

		runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
		runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

		glideAccelAmount = (50 * glideAcceleration) / glideMaxSpeed;
		glideDeccelAmount = (50 * glideDecceleration) / glideMaxSpeed;

		jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

		runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
		runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
		glideAcceleration = Mathf.Clamp(glideAcceleration, 0.01f, glideMaxSpeed);
		glideDecceleration = Mathf.Clamp(glideDecceleration, 0.01f, glideMaxSpeed);
	}

	#if UNITY_EDITOR
    [SerializeField] private bool _revert;
    private string _initialJson = string.Empty;
	#endif

  private void OnEnable ( )
	{
	#if UNITY_EDITOR
					EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
					EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
	#endif
	}

	#if UNITY_EDITOR
		private void OnPlayModeStateChanged ( PlayModeStateChange obj )
		{
		switch ( obj )
		{
			case PlayModeStateChange.EnteredPlayMode:
					_initialJson = EditorJsonUtility.ToJson ( this );
					break;

			case PlayModeStateChange.ExitingPlayMode:
					EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
					if ( _revert )
							EditorJsonUtility.FromJsonOverwrite ( _initialJson, this );
					break;
		}
		}
	#endif
}