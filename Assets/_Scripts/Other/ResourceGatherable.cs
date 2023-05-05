using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ResourceGatherable : MonoBehaviour, IGatherable
{
    [Header("References")]
    [SerializeField] protected Transform _gfx;
    [SerializeField] protected Health _health;
    [SerializeField] protected Transform _dropPoint;
    [Header("Preferences")]
    [SerializeField] protected ItemType _typeCanDamage;
    [SerializeField] protected int _maxCount = 1;
    [SerializeField] protected int _minCount = 1;
    [Header("Resources")]
    [SerializeField] protected List<Item> _resourcesList = new List<Item>();
    private float _itemPerDamage;
    private float _takenDamage;
    public ItemType TypeOfItemWhatCanDamage {get => _typeCanDamage; set => _typeCanDamage = value; }
    public virtual void Start()
    {
        _itemPerDamage = _health.CurrentHealth / _resourcesList.Count;
    }
    public virtual void GetGatherDamage(float damage)
    {
        _health.CurrentHealth -= damage;
        _takenDamage += damage;
        while(_takenDamage >= _itemPerDamage)
        {
            _takenDamage -= _itemPerDamage;
            LootManager.Instance.DropItem(_dropPoint.position, _resourcesList[Random.Range(0, _resourcesList.Count)], Random.Range(_minCount, _maxCount), true, false, 0, true);
        }
        if (_health.CurrentHealth <= 0f)
        {
            Destroy(gameObject);
            return;
        }
        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.SetAutoKill();
        scaleSequence.Append(_gfx.DOScaleX(0.9f, 0.05f));
        scaleSequence.Append(_gfx.DOScaleY(1.1f, 0.05f));
        scaleSequence.PrependInterval(0.1f);
        scaleSequence.Append(_gfx.DOScale(1f, 0.1f));
    }
}
