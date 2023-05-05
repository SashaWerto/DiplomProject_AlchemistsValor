using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Manager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    private bool _state;
    public Animator AnimatorUi { get => _animator; set => _animator = value; }
    public void ChangeSpeed(float speed)
    {
        _animator.SetFloat("Speed", speed);
    }
    public void ToggleBool(string parameter)
    {
        _state = !_state;
        _animator.SetBool(parameter, _state);
    }
    public void SetBoolTrue(string parameter)
    {
        _animator.SetBool(parameter, true);
    }
    public void SetBoolFalse(string parameter)
    {
        _animator.SetBool(parameter, false);
    }
}
