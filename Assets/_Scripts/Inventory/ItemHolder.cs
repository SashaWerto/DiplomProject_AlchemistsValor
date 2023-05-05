using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemHolder : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private bool _dontInitOnStart;
    [Header("References")]
    [SerializeField] private Item _item;
    [SerializeField] private int _count;
    [SerializeField] private float _durability;
    [Header("References/Visual")]
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    private bool _equiped;
    public Item ItemInHolder { get => _item; set => _item = value; }
    public int Count { get => _count; set => _count = value; }
    public float Durability { get => _durability; set => _durability = value; }
    public bool Equiped { get => _equiped; set => _equiped = value; }
    public bool isDirty { get; set; }
    private void Start()
    {
        if(!_dontInitOnStart)
            Initiation();
        if (!isDirty)
            _durability = _item.maxDurability;
    }

    public void Initiation()
    {
        if(_item.mesh)
        {
            _meshFilter.mesh = _item.mesh;
            _meshRenderer.material = _item.meshMaterial;
        }
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<BoxCollider>();
    }
    public void AddItemToInventory()
    {
        LootManager.Instance.DropItem(Vector3.zero, _item, _count, false, isDirty, _durability, true);
        Destroy(gameObject);
    }
}