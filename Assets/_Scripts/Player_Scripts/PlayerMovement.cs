using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform orientation;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private HeadBobController _headBob;
    [SerializeField] private HeadBobController _weaponBob;
    [Header("Settings")]
    [SerializeField] private float _speed;
    [SerializeField] private float _groundDrag;
    [Header("JumpSettings")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _airMultiplyer;
    [SerializeField] private float _jumpCooldown;
    [Header("GroundCheck")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _height;
    [Header("Keybinds")]
    [SerializeField] private KeyCode _jumpkey = KeyCode.Space;
    private bool _grounded;
    private bool _canJump = true;
    private float _horizontal;
    private float _vertical;
    private Vector3 _moveDirection;
    public bool Freeze { get; set; }
    public Rigidbody PlayerRigidbody { get => _rigidbody; set => _rigidbody = value; }
    private void Update()
    {
        HeadBob();
        if (Freeze)
        {            
            return;
        }
        GetInput();
        SpeedControl();
        _grounded = Physics.Raycast(_groundCheck.position, Vector3.down, _height * 0.5f + 0.2f);
        if (_grounded)
            _rigidbody.drag = _groundDrag;
        else
            _rigidbody.drag = 0;
    }
    private void FixedUpdate()
    {
        if (Freeze)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        Movement();
    }
    private void HeadBob()
    {
        if (_grounded && _rigidbody.velocity.magnitude > 1f)
        {
            _headBob.Enable = true;
            _weaponBob.Enable = true;
        }
        else
        {
            _headBob.Enable = false;
            _weaponBob.Enable = false;
            _headBob.ResetPosition();
            _weaponBob.ResetPosition();
        }
    }
    private void GetInput()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        if(Input.GetKey(_jumpkey) && _canJump && _grounded)
        {
            _canJump = false;
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }
    private void Movement()
    {
        _moveDirection = orientation.forward * _vertical + orientation.right * _horizontal;
        if(_grounded)
        {
           _rigidbody.AddForce(_moveDirection.normalized * _speed * 10f, ForceMode.Force);
        }
        else if(!_grounded)
        {
            _rigidbody.AddForce(_moveDirection.normalized * _speed * 10f * _airMultiplyer, ForceMode.Force);
        }        
    }
    private void SpeedControl()
    {
        Vector3 flatVelocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        if (flatVelocity.magnitude > _speed)
        {
            Vector3 limitVelocity = flatVelocity.normalized * _speed;
            _rigidbody.velocity = new Vector3(limitVelocity.x, _rigidbody.velocity.y, limitVelocity.z);
        }
    }
    private void Jump()
    {
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0f, _rigidbody.velocity.z);
        _rigidbody.AddForce(orientation.up * _jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        _canJump = true;
    }
}
