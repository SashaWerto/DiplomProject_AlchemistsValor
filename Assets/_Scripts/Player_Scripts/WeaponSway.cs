using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    [Header("Sway settings")]
    [SerializeField] private float _smooth;
    [SerializeField] private float _swayMultiplier;
    private float _startMiltiplier;
    public float Multiplier { get => _swayMultiplier; set => _swayMultiplier = value; }
    private void Awake()
    {
        _startMiltiplier = _swayMultiplier;
    }
    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _swayMultiplier;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, _smooth * Time.deltaTime);
    }
    public void ResetMultiplier()
    {
        _swayMultiplier = _startMiltiplier;
    }
}
