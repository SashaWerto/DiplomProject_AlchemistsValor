using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class EnemyAI : BaseAI
{
    [Header("References/Detector")]
    [SerializeField] private Detector _detector;
    [Header("Preferences/Combat")]
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _rotationToTargetOnAttack = 0.03f;
    [Header("Preferences/Defence")]
    [SerializeField] private float _blockChance;
    [SerializeField] private float _blockMaxTime;
    private bool _battleState;
    private bool _blockingState;
    private bool _defenceState;
    private float _currentBlockTime;
    protected override void Start()
    {
        base.Start();
        if (!_target)
            _target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected override void Update()
    {
        base.Update();
        if(_patrolling)
        {
            PatrollState();           
        }
        else if(_battleState)
        {
            BattleState();
        }
    }
    protected virtual void PatrollState()
    {
        _detector.Detection();
        if(_detector.Detected)
        {
            _battleState = true;
            _patrolling = false;
            RestartPathUpdate();
        }        
        _animator.SetBool("isAttacking", false);
    }
    protected virtual void BattleState()
    {
        if (_distanceToTarget <= _attackDistance && _health && !_health.OnStun && !_health.Dead)
        {
            _animator.SetBool("isAttacking", true);
            RotateToTarget();
        }
        else _animator.SetBool("isAttacking", false);
        if(!_detector.Detected)
        {
            _battleState = false;
            _patrolling = true;
        }
    }
    protected virtual void BlockState()
    {
        _currentBlockTime -= Time.deltaTime;
        if (_currentBlockTime <= 0f)
        {
            RotateToTarget();
            ResetBlockTime();
        }
    }
    protected virtual void SetBattleState()
    {
        _detector.SetDetected();
        _battleState = true;
        _patrolling = false;
    }
    public virtual void RotateToTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
        var angle = targetRotation.eulerAngles;
        var rotationToTarget = Quaternion.Euler(new Vector3(0, angle.y));
        transform.rotation = Quaternion.Slerp(transform.rotation, rotationToTarget, _rotationToTargetOnAttack);
    }
    private void ResetBlockTime()
    {
        _currentBlockTime = _blockMaxTime;
    }
    protected virtual void SetPatrollState()
    {
        _battleState = false;
        _patrolling = true;
    }
    public override void GetDamage()
    {
        base.GetDamage();
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 20f, transform.right, 1f);
        foreach(RaycastHit hit in hits)
        {
            if(hit.collider.TryGetComponent<EnemyAI>(out var enemy))
            {
                enemy.SetBattleState();
            }
        }       
    }
}
