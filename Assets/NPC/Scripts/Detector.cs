using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Detector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _detectPoint;
    [SerializeField] private GameObject _warningIcon;
    [SerializeField] private GameObject _detectedIcon;
    [Header("References/UI")]
    [SerializeField] private Image _warningIconImage;
    [Header("Preferences")]
    [SerializeField] private float _maxDetection = 5f;
    [SerializeField] private float _size = 10f;
    [SerializeField] private float _distance = 20f;
    [SerializeField] private float _detectionEndDelay = 5f;
    private bool _onDetectedState;
    private float _detectionDelay;
    private float _detection;
    public bool Detected { get => _onDetectedState; set => _onDetectedState = value; }
    public float DetectionValue { get => _detection; set => _detection = value; }
    public float DetectionMaxValue { get => _maxDetection; set => _maxDetection = value; }
    private void Start()
    {
        _detectPoint.localPosition = new Vector3(-_size, 0, 0);
    }
    private void Update()
    {
        _detectionDelay += Time.deltaTime;
        if (_detectionEndDelay <= _detectionDelay)
        {
            _onDetectedState = false;
        }
    }
    public void Detection()
    {
        RaycastHit hit;
        Physics.SphereCast(_detectPoint.position, _size, _detectPoint.right, out hit, _distance);
        _warningIconImage.fillAmount = _detection / _maxDetection;
        if (_detection > 0 && !_onDetectedState)
        {
            _warningIcon.SetActive(true);
            _detectedIcon.SetActive(false);
        }
        else if(_onDetectedState)
        {
            _warningIcon.SetActive(false);
            _detectedIcon.SetActive(true);
        }
        else _warningIcon.SetActive(false);
        if (hit.collider)
        {
            if(hit.collider.CompareTag("Player"))
            {
                _detectionDelay = 0;
                float multiplierByDistance = Vector3.Distance(transform.position, hit.collider.transform.position);
                _detection += _distance / multiplierByDistance * Time.deltaTime;
                if (_detection >= _maxDetection)
                {
                    DetectedState();
                }
                else if (_detection <= 0f)
                {
                    UndetectedState();
                }
                return;
            }            
        }
        _detection = Mathf.Clamp(_detection -= 0.2f * Time.deltaTime, 0, _maxDetection);
    }
    public void DetectedState()
    {
        _onDetectedState = true;
        _warningIcon.SetActive(false);
        _detectedIcon.SetActive(true);
    }
    public void UndetectedState()
    {
        _onDetectedState = false;
        _warningIcon.SetActive(false);
        _detectedIcon.SetActive(false);
    }
    public void SetDetected()
    {
        _detectionDelay = 0;
        _detection = _maxDetection;
        DetectedState();
    }
}
