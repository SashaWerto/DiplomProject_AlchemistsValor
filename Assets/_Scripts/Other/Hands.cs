using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Hands : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _leftFullHand;
    [SerializeField] private Transform _rightFullHand;
    [Header("References/IK")]
    [SerializeField] private TwoBoneIKConstraint _leftTwoBoneIk;
    [SerializeField] private TwoBoneIKConstraint _rightTwoBoneIk;
    [SerializeField] private RigBuilder _rigBuilder;
    [Header("References/IK")]
    [SerializeField] private Transform _leftTarget;
    [SerializeField] private Transform _rightTarget;
    public Transform LeftFullHand { get => _leftFullHand; set => _leftFullHand = value; }
    public Transform RightFullHand { get => _rightFullHand; set => _rightFullHand = value; }
    public TwoBoneIKConstraint LeftIK { get => _leftTwoBoneIk; set => _leftTwoBoneIk = value; }
    public TwoBoneIKConstraint RightIK { get => _rightTwoBoneIk; set => _rightTwoBoneIk = value; }
    public void ResetHands()
    {
        _leftTwoBoneIk.data.target = _leftTarget;
        _rightTwoBoneIk.data.target = _rightTarget;
        _leftFullHand.localPosition = Vector3.zero;
        _rightFullHand.localPosition = Vector3.zero;
        UpdateRig();
    }
    public void UpdateRig()
    {
        _rigBuilder.Build();
    }
}
