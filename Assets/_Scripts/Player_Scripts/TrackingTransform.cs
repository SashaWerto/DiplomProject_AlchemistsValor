using UnityEngine;

public class TrackingTransform : MonoBehaviour
{
    [SerializeField] private Transform _target;
    void Update()
    {
        transform.position = _target.position;
    }
}