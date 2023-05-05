using UnityEditor;
using UnityEngine;

public class HandsManipulator : MonoBehaviour
{
    [Header("References")]
    private Hands _hands;
    [SerializeField] private Transform _rightTarget;
    [SerializeField] private Transform _leftTarget;
    [Header("Preferences")]
    [SerializeField] private Vector3 _rightHandOffset;
    [SerializeField] private Vector3 _leftHandOffset;
    public Hands HandsScript { get => _hands; set => _hands = value; }
    public void SetHands()
    {
        if (_rightTarget)
        {
            _hands.RightFullHand.localPosition = _rightHandOffset;
            _hands.RightIK.data.target = _rightTarget;
        }
        if(_leftTarget)
        {
            _hands.LeftFullHand.localPosition = _leftHandOffset;                     
            _hands.LeftIK.data.target = _leftTarget;
        }
        _hands.UpdateRig();
    }
}
