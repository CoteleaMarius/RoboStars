using UnityEngine;
using UnityEngine.InputSystem;

namespace _RoboStars.Scripts.Movement
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int Walking = Animator.StringToHash("isWalking");
        private static readonly int IsRunning = Animator.StringToHash("isRunning");
        [SerializeField] private float rotateSpeed;
        private PlayerInput _inputActions;
        private CharacterController _controller;
        private Animator _animator;
        private Vector2 _movementInput;
        private Vector3 _currentMovement;
        private Quaternion _rotateDirection;
        private bool _isRunning, _isWalking;

        private void OnMovementActions(InputAction.CallbackContext context)
        {
            _movementInput = context.ReadValue<Vector2>();
            _currentMovement.x = _movementInput.x;
            _currentMovement.z = _movementInput.y;
            _isWalking = _movementInput.x != 0 || _movementInput.y != 0;
        }

        private void OnRun(InputAction.CallbackContext context)
        {
            _isRunning = context.ReadValueAsButton();
        }

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
            _inputActions = new PlayerInput();
            _inputActions.CharacterControls.Movement.started += OnMovementActions;
            _inputActions.CharacterControls.Movement.performed += OnMovementActions;
            _inputActions.CharacterControls.Movement.canceled += OnMovementActions;
            _inputActions.CharacterControls.Run.started += OnRun;
            _inputActions.CharacterControls.Run.canceled += OnRun;
        }

        private void OnEnable()
        {
            _inputActions.CharacterControls.Enable();
        }

        private void OnDisable()
        {
            _inputActions.CharacterControls.Disable();
        }

        private void PlayerRotate()
        {
            if (_isWalking)
            {
                _rotateDirection = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_currentMovement),
                    Time.deltaTime * rotateSpeed);
                transform.rotation = _rotateDirection;
            }
        }
        
        private void AnimateControl()
        {
            _animator.SetBool(Walking, _isWalking);
            _animator.SetBool(IsRunning, _isRunning);
        }

        private void Update()
        {
            AnimateControl();
            PlayerRotate();
        }

        private void FixedUpdate()
        {
            _controller.Move(_currentMovement * Time.fixedDeltaTime);
        }
    }
}

