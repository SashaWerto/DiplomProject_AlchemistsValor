using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabbing : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _camera;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Transform _castPoint;
    [Header("Ledge Grabbing")]
    [SerializeField] private float _moveToLedgeSpeed;
    [SerializeField] private float _maxLedgeGrabDistance;
    [SerializeField] private float _minTimeOnLedge;
    private float _timeOnLedge;
    [Header("Ledge Jumping")]
    [SerializeField] KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private float _ledgeJumpForwardForce;
    [SerializeField] private float _ledgeJumpUpwardForce;
    [HideInInspector] public bool Holding {get;set;}
    [Header("Ledge Detection")]
    [SerializeField] private float _canDetectAfterTime;
    [SerializeField] private float _ledgeDetectionLength;
    [SerializeField] private float _ledgeSphereCastRadius;
    [SerializeField] LayerMask _whatIsLedge;

    private Transform _lastLedge;
    private Transform _currentLedge;

    private RaycastHit _ledgeHit;

    [Header("Exiting")]
    [SerializeField] private bool _exitingLedge;
    [SerializeField] private float _exitLedgeTime;
    private float _exitLedgeTimer;
    private float _canDetectAfterTimer;

    private void Update()
    {
        LedgeDetection();
        SubStateMachine();
    }
    private void SubStateMachine()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        bool anyInputKeyPressed = horizontalInput != 0 || verticalInput != 0;
        if(Holding)
        {
            FreezeRigidbodyOnLedge();
            _timeOnLedge += Time.deltaTime;
            if (_timeOnLedge > _minTimeOnLedge && anyInputKeyPressed) ExitLedgeHold();
            if(Input.GetKeyDown(jumpKey))
            {
                LedgeJump();
            }
        }
        else if (_exitingLedge)
        {
            if (_exitLedgeTimer > 0) _exitLedgeTimer -= Time.deltaTime;
            else _exitingLedge = false;
        }
        _canDetectAfterTimer -= Time.deltaTime;
    }
    private void LedgeDetection()
    {
        bool ledgeDetected = Physics.SphereCast(_castPoint.position, _ledgeSphereCastRadius, _camera.forward,
            out _ledgeHit, _ledgeDetectionLength, _whatIsLedge);
        if (!ledgeDetected) return;
        float distanceToLedge = Vector3.Distance(_castPoint.position, _ledgeHit.transform.position);
        if (_ledgeHit.transform == _lastLedge) return;
        if (distanceToLedge < _maxLedgeGrabDistance && !Holding && _canDetectAfterTimer <= 0) EnterLedgeHold();
    }
    private void LedgeJump()
    {
        ExitLedgeHold();
        Vector3 forceToAdd = _camera.forward * _ledgeJumpForwardForce + _orientation.up * _ledgeJumpUpwardForce;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(forceToAdd, ForceMode.Impulse);
    }
    private void EnterLedgeHold()
    {
        Holding = true;
        _currentLedge = _ledgeHit.transform;
        _lastLedge = _ledgeHit.transform;

        _rigidbody.useGravity = false;
        _rigidbody.velocity = Vector3.zero;
        Debug.Log("Enter Ledge");
    }
    private void ExitLedgeHold()
    {
        _exitingLedge = true;
        _exitLedgeTimer = _exitLedgeTime;
        Holding = false;
        _playerMovement.Freeze = false;
        _timeOnLedge = 0f;
        _rigidbody.useGravity = true;
        _canDetectAfterTimer = _canDetectAfterTime;
        Invoke(nameof(ResetLastLedge), 1f);
        Debug.Log("Exit Ledge");
    }
    private void FreezeRigidbodyOnLedge()
    {
        _rigidbody.useGravity = false;
        Vector3 directionToLedge = _currentLedge.position - _castPoint.position;
        float distanceToLedge = Vector3.Distance(_castPoint.position, _currentLedge.position);
        if(distanceToLedge > 1f)
        {
            if (_rigidbody.velocity.magnitude < _moveToLedgeSpeed)
                _rigidbody.AddForce(directionToLedge.normalized * _moveToLedgeSpeed * 1000f * Time.deltaTime);
        }
        else
        {
            _playerMovement.Freeze = true;
        }
        if (distanceToLedge > _maxLedgeGrabDistance) ExitLedgeHold();
    }
    private void ResetLastLedge()
    {
        _playerMovement.Freeze = false;
        _lastLedge = null;
    }
}
