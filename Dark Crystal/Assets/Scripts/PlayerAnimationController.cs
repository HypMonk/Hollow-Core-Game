using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField]
    List<RuntimeAnimatorController> _runtimeAnimatorControllers = new List<RuntimeAnimatorController>();

    [SerializeField]
    Animator _animator;

    [SerializeField]
    SpriteRenderer _spriteRenderer;

    [HideInInspector]
    public PlayerStates state;

    [HideInInspector]
    public PlayerDirection direction;

    PlayerStates _lastState;
    PlayerDirection _lastDirection;

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerStates.Idle;
        direction = PlayerDirection.LDown;

        foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
        {
            if (rac.name == "idle_down")
            {
                _animator.runtimeAnimatorController = rac;
            }
        }

        _lastState = state;
        _lastDirection = direction;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != _lastState)
        {
            UpdateAnimationState();
        }

        if (direction != _lastDirection)
        {
            UpdateAnimationDirection();
        }
    }

    void UpdateAnimationState()
    {

    }

    void UpdateAnimationDirection()
    {
        if (direction == PlayerDirection.Right)
        {
            if (_spriteRenderer.flipX == false) { _spriteRenderer.flipX = true; }
        } else
        {
            if (_spriteRenderer.flipX == true) { _spriteRenderer.flipX = false; }
        }

        if (state == PlayerStates.Idle)
        {
            if (direction == PlayerDirection.LDown || direction == PlayerDirection.RDown)
            {

                foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
                {
                    if (rac.name == "idle_down")
                    {
                        _animator.runtimeAnimatorController = rac;
                    }
                }
            } 
            else if (direction == PlayerDirection.LUp || direction == PlayerDirection.RUp)
            {

                foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
                {
                    if (rac.name == "idle_up")
                    {
                        _animator.runtimeAnimatorController = rac;
                    }
                }
            } 
            else if (direction == PlayerDirection.Left || direction == PlayerDirection.Right)
            {

                foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
                {
                    if (rac.name == "idle_side")
                    {
                        _animator.runtimeAnimatorController = rac;
                    }
                }
            }
        } else if (state == PlayerStates.Walking)
        {
            if (direction == PlayerDirection.LDown || direction == PlayerDirection.RDown)
            {

                foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
                {
                    if (rac.name == "walk_down")
                    {
                        _animator.runtimeAnimatorController = rac;
                    }
                }
            }
            else if (direction == PlayerDirection.LUp || direction == PlayerDirection.RUp)
            {

                foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
                {
                    if (rac.name == "walk_up")
                    {
                        _animator.runtimeAnimatorController = rac;
                    }
                }
            }
            else if (direction == PlayerDirection.Left || direction == PlayerDirection.Right)
            {

                foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
                {
                    if (rac.name == "walk_side")
                    {
                        _animator.runtimeAnimatorController = rac;
                    }
                }
            }
        }
        else if (state == PlayerStates.Sliding)
        {
            if (direction == PlayerDirection.LDown || direction == PlayerDirection.RDown)
            {

                foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
                {
                    if (rac.name == "slide_down")
                    {
                        _animator.runtimeAnimatorController = rac;
                    }
                }
            }
            else if (direction == PlayerDirection.LUp || direction == PlayerDirection.RUp)
            {

                foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
                {
                    if (rac.name == "slide_up")
                    {
                        _animator.runtimeAnimatorController = rac;
                    }
                }
            }
            else if (direction == PlayerDirection.Left || direction == PlayerDirection.Right)
            {

                foreach (RuntimeAnimatorController rac in _runtimeAnimatorControllers)
                {
                    if (rac.name == "slide_side")
                    {
                        _animator.runtimeAnimatorController = rac;
                    }
                }
            }
        }

    }
}
