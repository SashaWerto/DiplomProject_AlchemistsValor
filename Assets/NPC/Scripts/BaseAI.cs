using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
[System.Serializable]
public class BaseAI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected private Animator _animator;
    [SerializeField] protected private Health _health;
    [SerializeField] protected private Rigidbody _rigidbody;
    [Header("Preferences/Movement")]
    [SerializeField] private float _speed;
    [Header("Preferences/Patrolling")]
    [SerializeField] private float _patrollRange;
    [SerializeField] protected private bool _patrolling;
    private int _currentIndexOfPath;
    private Vector3 _startPos;
    protected private AnimatorStateInfo _animatorInfo;
    protected private Transform _target;
    protected private NavMeshPath _navmeshPath;
    protected private float _distanceToTarget;
    protected private Vector3 _directionToPoint;
    public Animator AnimatorAI { get => _animator; set => _animator = value; }
    private void Awake()
    {
        _navmeshPath = new NavMeshPath();
    }
    protected virtual void Start()
    {
        _startPos = transform.position;
        StartCoroutine(UpdatePath());
    }
    protected virtual void Update()
    {
        _animatorInfo = _animator.GetCurrentAnimatorStateInfo(0);
        PlayAnimations();
        if (_health && _health.Dead) return;
        if (_navmeshPath.status != NavMeshPathStatus.PathPartial && !_animatorInfo.IsTag("block"))
        {
            if (_currentIndexOfPath >= _navmeshPath.corners.Length) return;
            GetDirection();
            MoveToTarget();
        }
        SpeedControl();
        if (_target) _distanceToTarget = Vector3.Distance(transform.position, _target.position);     
    }
    private void GetDirection()
    {
        var distanceToPoint = Vector3.Distance(transform.position, _navmeshPath.corners[_currentIndexOfPath]);
        if (distanceToPoint <= 2f && _currentIndexOfPath < _navmeshPath.corners.Length - 1)
        {
            _currentIndexOfPath++;
        }
        _directionToPoint = _navmeshPath.corners[_currentIndexOfPath] - transform.position;
    }
    protected void MoveToTarget()
    {
        _rigidbody.AddForce(_directionToPoint.normalized * _speed, ForceMode.Force);
        RotateToTarget();
    }
    protected void RotateToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_navmeshPath.corners[_currentIndexOfPath] - transform.position);
        var angle = targetRotation.eulerAngles;
        var rotationToTarget = Quaternion.Euler(new Vector3(0, angle.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, 0.015f);
    }
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        if (flatVelocity.magnitude > _speed)
        {
            Vector3 limitVelocity = flatVelocity.normalized * _speed;
            _rigidbody.velocity = new Vector3(limitVelocity.x, _rigidbody.velocity.y, limitVelocity.z);
        }
    }
    private void PlayAnimations()
    {
        if (_rigidbody.velocity.magnitude > 1)
        {
            _animator.SetBool("isMoving", true);
        }
        else _animator.SetBool("isMoving", false);
        if(_health && _health.OnStun && !_health.Dead)
        {
            _animator.SetBool("onStun", true);
        }
        else _animator.SetBool("onStun", false);
    }
    IEnumerator UpdatePath()
    {
        while(true)
        {
            if(_target && !_patrolling)
            {
                NavMesh.CalculatePath(transform.position, _target.position, NavMesh.AllAreas, _navmeshPath);
                _currentIndexOfPath = 0;
                yield return new WaitForSeconds(0.1f);
            }
            else if (_patrolling)
            {
                Vector3 point = new Vector3(_startPos.x + Random.Range(-_patrollRange, _patrollRange), _startPos.y, _startPos.z + Random.Range(-_patrollRange, _patrollRange));
                NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, _navmeshPath);
                _currentIndexOfPath = 0;
                yield return new WaitForSeconds(Random.Range(3f,9f));
            }
            else yield return new WaitForSeconds(1f);
        }              
    }
    public virtual void GetDamage()
    {
        if (!_health) return;
        if (_health.CheckState())
        {
            Dead();
            return;
        }
        else _animator.SetTrigger("Damaged");
    }
    public void Dead()
    {
        _animator.SetBool("Dead", true);
        if (TryGetComponent<Collider>(out var collider))
        {
            collider.enabled = false;
        }
        if (_rigidbody)
        {
            _rigidbody.isKinematic = true;
        }
    }
    protected virtual void Attack()
    {

    }
}
