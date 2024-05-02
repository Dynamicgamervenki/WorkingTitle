using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		public StarterAssetsCustom customStarterAssestInput;
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool interact;
		public bool attack;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		public StarterAssetsCustom inputActions;
        private void Awake()
        {
            inputActions = new StarterAssetsCustom();
			inputActions.Player.Interact.started += transform.GetComponent<ThirdPersonController>().PerformRopeClimb;
			inputActions.Player.Roll.performed += transform.GetComponent<ThirdPersonController>().PlayerRollStart;
			inputActions.Player.Fire1.canceled += attackEnd;
        }
        private void OnEnable()
        {
            inputActions.Enable();
        }
       

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		public void OnInteract(InputValue value)
		{
			if(value.isPressed)
				InteractInput();
		}
		public void OnFire1(InputValue value)
		{
			if (value.isPressed)
				attack = true;
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void InteractInput()
		{
			interact = !interact;
		}
		void attackEnd(InputAction.CallbackContext value)
		{
                attack = false;
        }
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
        private void OnDisable()
        {
            inputActions.Disable();
            inputActions.Player.Interact.started += transform.GetComponent<ThirdPersonController>().PerformRopeClimb;
            inputActions.Player.Roll.performed += transform.GetComponent<ThirdPersonController>().PlayerRollStart;

        }
    }
	
}