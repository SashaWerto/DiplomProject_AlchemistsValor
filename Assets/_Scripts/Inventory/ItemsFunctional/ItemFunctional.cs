using UnityEngine;
public class ItemFunctional : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform _hitPoint;
    [SerializeField] protected GameObject _hitParticle;
    [Header("References/GFX")]
    [SerializeField] protected GameObject _fullVariant;
    [SerializeField] protected GameObject _brokenVariant;
    [Header("Preferences")]
    [SerializeField] protected float _radius;
    [SerializeField] protected float _force;
    [Header("Key's")]
    [SerializeField] protected KeyCode _useKey;
    [SerializeField] protected Animator _animator;
    protected CharacterStats _stats;
    protected AnimatorStateInfo _animatorState;
    protected bool _cantUse;
    protected bool Broken => _cell.Durability <= 0;
    protected Collider _hitCollider;
    protected RaycastHit[] _hitsRaycast;
    protected Cell _cell;
    public Cell EquipedCell { get => _cell; set => _cell = value; }
    public Transform HitPoint { get => _hitPoint; set => _hitPoint = value; }
    public GameObject BrokenVariant { get => _brokenVariant; set => _brokenVariant = value; }
    public GameObject FullVariant { get => _fullVariant; set => _fullVariant = value; }
    public CharacterStats Statistics { get => _stats; set => _stats = value; }
    public float Radius { get => _radius; set => _radius = value; }
    public bool CantUse { get => _cantUse; set => _cantUse = value; }
    public virtual void Update()
    {
        _animatorState = _animator.GetCurrentAnimatorStateInfo(0);        
    }
    /// <summary>
    /// Метод Attack() предназначен для вызова из Animator ивентом, чтобы действия синхронизировались с таймингом анимаций.
    /// Предназначен для проверки столкновений с Collider2D и нахождением прицепленных на них интерфейсов для взаимодействия
    /// с объектами (Метод используется наследуемыми классами).
    /// </summary>
    public virtual void Attack()
    {
        _hitsRaycast = null;
        _hitsRaycast = Physics.SphereCastAll(_hitPoint.position, _radius, _hitPoint.forward, 1f);
        foreach (RaycastHit hit in _hitsRaycast)
        {
            if (hit.collider.TryGetComponent<Rigidbody>(out var rigidbody) && !hit.collider.CompareTag("Player"))
            {
                var heading = transform.parent.position - rigidbody.transform.position;
                var distance = heading.magnitude;
                var direction = heading / distance;
                rigidbody.AddForce(-direction * _force, ForceMode.Impulse);
            }
            Invoke(nameof(CheckHit), 0.1f);
        }
    }
    public virtual void SetItem(CharacterStats characterStats, Cell cell)
    {
        Statistics = characterStats;
        EquipedCell = cell;
    }
    public virtual void SetBroken()
    {
        _fullVariant.SetActive(false);
        if(_brokenVariant)
            _brokenVariant.SetActive(true);
    }
    public virtual void SetFull()
    {
        _fullVariant.SetActive(true);
        if (_brokenVariant)
            _brokenVariant.SetActive(false);
    }
    public virtual void CheckHit()
    {
        if(_hitCollider)
        {
            _cell.Durability -= 1f;
            _cell.RefreshDurabilityBar();
            if(_hitParticle)
                Instantiate(_hitParticle, _hitCollider.ClosestPoint(_hitPoint.position), Quaternion.identity);
        }
        _hitCollider = null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(_hitPoint.position, _radius);
    }
}