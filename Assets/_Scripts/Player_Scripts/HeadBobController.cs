using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HeadBobController : MonoBehaviour
{
    [SerializeField] private float _amplitude = 0.015f;
    [SerializeField] private float _frequency = 10.0f;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private float _multiplier = 1f;
    public bool Enable { get; set; }
    private Vector3 _startPos;
    private void Awake()
    {
        _startPos = _camera.localPosition;
    }
    private void Update()
    {
        if (!Enable) return;
        PlayMotion(FootStepMotion());
    }
    private void PlayMotion(Vector3 motion)
    {
        _camera.localPosition += motion;
    }
    private Vector3 FootStepMotion()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * _frequency) * _amplitude;
        pos.x += Mathf.Cos(Time.time * _frequency / 2) * _amplitude * 2;
        return pos;
    }
    public void ResetPosition()
    {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, _multiplier * Time.deltaTime);
    }
}
