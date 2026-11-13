using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class NewPlayerMovement : MonoBehaviour
    {
        [field: SerializeField] public float MoveSpeed { get; private set; } = 5000f;
        [field: SerializeField] public CharacterState Idle { get; private set; }
        [field: SerializeField] public CharacterState Walk { get; private set; } 
        [field: SerializeField] public CharacterState Use { get; private set; }

        [field: SerializeField] public StateAnimationSetDictionary StateAnimations { get; private set; }
        [field: SerializeField] public float WalkVelocityThreshold { get; private set; } = 0.05f;
        public CharacterState CurrentState { get { return _currentState; }
            private set
            {
                if (_currentState != value){
                    _currentState = value;
                    ChangeClip();
                    _timeToEndAnimation = _currentClip.length;
                }
            } 
        }
    
        private Vector2 _axisInput = Vector2.zero;
        private Rigidbody2D _rb;
        private Animator _animator;
        private CharacterState _currentState;
        private AnimationClip  _currentClip;
        private Vector2 _facingDirection;
        private float _timeToEndAnimation = 0f;
    
        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            CurrentState = Idle;
        }

        private void Update()
        {
            _timeToEndAnimation = Mathf.Max(_timeToEndAnimation - Time.deltaTime,  0f);
            if (_currentState.CanExitWhilePlaying || _timeToEndAnimation <= 0)
            {
            
                if (_axisInput != Vector2.zero && _rb.linearVelocity.magnitude > WalkVelocityThreshold) 
                {
                    CurrentState = Walk;
                }
                else
                {
                    CurrentState = Idle;
                }
                ChangeClip();
            }
        }

        private void ChangeClip()
        {
            // If we have no facing direction yet, default to (0, -1) [Down]
            if (_facingDirection == Vector2.zero)
            {
                _facingDirection = Vector2.down;
            }

            AnimationClip expectedClip = StateAnimations.GetFacingClipFromState(_currentState, _facingDirection);

            // Safety check: Does the clip exist?
            if (expectedClip == null) return;

            if (_currentClip == null || _currentClip != expectedClip)
            {
                _animator.Play(expectedClip.name);
                _currentClip = expectedClip;
            }
        }


        private void FixedUpdate()
        {
            if (_currentState.CanMove)
            {
                Vector2 moveForce = _axisInput * (MoveSpeed * Time.fixedDeltaTime);
                _rb.AddForce(moveForce);
            }
        }
        private void OnMove(InputValue value)
        {
            _axisInput = value.Get<Vector2>();

            if (_axisInput != Vector2.zero)
            {
                _facingDirection = _axisInput;
            }
    
        }
        private void OnUse(InputValue value)
        {
            CurrentState = Use;
            _rb.WakeUp(); 
        }

    }
}
