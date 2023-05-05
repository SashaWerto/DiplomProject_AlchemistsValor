using UnityEngine;
using UnityEngine.AI;
[System.Serializable]
public class EnemyAI : BaseAI
{
    [Header("Preferences/Combat")]
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _rotationToTargetOnAttack = 0.03f;
    protected override void Start()
    {
        base.Start();
        if (!_target)
            _target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    protected override void Update()
    {
        base.Update();
        if(_distanceToTarget <= _attackDistance && _health && !_health.OnStun && !_health.Dead)
        {
            _animator.SetBool("isAttacking", true);
            Quaternion targetRotation = Quaternion.LookRotation(_target.position - transform.position);
            var angle = targetRotation.eulerAngles;
            var pizda = Quaternion.Euler(new Vector3(0, angle.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, pizda, _rotationToTargetOnAttack);
        }
        else _animator.SetBool("isAttacking", false);
    }
}
