using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;


[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, ITakeDamage
{
    [Header("Player Settings")]
    [SerializeField] int _totalHealth = 100;
    public int CurrentHealth { get; private set; }

    [Header("Basic Movement")]
    [SerializeField] float _walkSpeed = 6f;
    [SerializeField] float _jumpForce = 7f;
    [SerializeField] float _gravityDefault = 10f;

    [Header("Flying Movement")]
    [SerializeField] float _dashSpeed = 12f;
    [SerializeField] float _dashTime = 0.6f;
    [SerializeField] float _flyingTime = 3f;
    [SerializeField] TrailRenderer _leftBoot;
    [SerializeField] TrailRenderer _rightBoot;

    [Header("Camera")]
    [SerializeField] float _lookSpeed = 2f;
    [SerializeField] float _lookXLimit = 45f;
    [SerializeField] CinemachineVirtualCamera _defaultCamera;
    [SerializeField] CinemachineVirtualCamera _aimCamera;
    [SerializeField] GameObject _cameraTarget;
    [SerializeField] float _targetHeight = 1f;

    [Header("Animation")]
    [SerializeField] Animator _anim;
    [SerializeField] Rig _aimRigAnim;


    bool _canMove = true;
    Vector3 _moveDirection = Vector3.zero;
    bool _isDashing;
    float _dashStartTime;
    float _startFlyingTime;
    float _rotationX = 0;
    float _aimRigWeightTarget;
    string _currentAnimBool;

    CharacterController _characterController;
    Weapon _weapon;


    private void Awake()
    {
        _weapon = GetComponent<Weapon>();

        _characterController = GetComponent<CharacterController>();

        CurrentHealth = _totalHealth;
        _currentAnimBool = "idle";
    }

    void Update()
    {
        float movementDirectionY = _moveDirection.y;

        Dash();
        Move();
        Jump(movementDirectionY);

        _characterController.Move(_moveDirection * Time.deltaTime);

        LookAround();
        Aim();
        Shoot();
    }

    void Move()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);


        float curSpeedX = _canMove ? (_isDashing ? _dashSpeed : _walkSpeed) * Input.GetAxis("Vertical") : 0; // up/down arrows
        float curSpeedZ = _canMove ? (_isDashing ? _dashSpeed : _walkSpeed) * Input.GetAxis("Horizontal") : 0; // left/right arrows

        _moveDirection = (forward * curSpeedX) + (right * curSpeedZ);


        if (_characterController.isGrounded && _moveDirection.y == 0)
        {
            UpdateAnim(_moveDirection != Vector3.zero ? "walking" : "idle");
            if (curSpeedX < 0)
                UpdateAnim("walkingBackwards");
        }

        _aimRigWeightTarget = 1;
    }

    void Dash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !_isDashing)
        {
            _dashStartTime = Time.time;
            _isDashing = true;
        }

        if (Time.time > _dashStartTime + _dashTime)
            _isDashing = false;

        if (_isDashing || !_characterController.isGrounded)
        {
            _leftBoot.enabled = true;
            _rightBoot.enabled = true;
        }
        else
        {
            _leftBoot.enabled = false;
            _rightBoot.enabled = false;
        }
    }

    void Jump(float movementDirectionY)
    {
        if (Input.GetButtonDown("Jump") && _canMove && _characterController.isGrounded)
        {
            _startFlyingTime = Time.time + 0.6f;
            _moveDirection.y = _jumpForce;
            UpdateAnim("jump");
        }
        else
            _moveDirection.y = movementDirectionY;

        if (!_characterController.isGrounded)
            _moveDirection.y -= _gravityDefault * Time.deltaTime;

        if (Input.GetButton("Jump") && Time.time < _startFlyingTime + _flyingTime && Time.time > _startFlyingTime)
        {
            _moveDirection.y = 0;
        }
    }

    void LookAround()
    {
        if (!_canMove) return;
        
        _rotationX += -Input.GetAxis("Mouse Y") * _lookSpeed;
        _rotationX = Mathf.Clamp(_rotationX, -_lookXLimit, _lookXLimit);
        _cameraTarget.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
        _cameraTarget.transform.position = transform.position + Vector3.up * _targetHeight;
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * _lookSpeed, 0);
    }

    void Aim()
    {
        if (Input.GetButton("Fire2"))
        {
            _aimCamera.enabled = true;
            _defaultCamera.enabled = false;
            _aimRigWeightTarget = 1;
        }
        if (!Input.GetButton("Fire2"))
        {
            _aimCamera.enabled = false;
            _defaultCamera.enabled = true;

            if (!Input.GetButton("Fire1") && _moveDirection == Vector3.zero)
                _aimRigWeightTarget = 0;
        }

        _aimRigAnim.weight = Mathf.Lerp(_aimRigAnim.weight, _aimRigWeightTarget, Time.deltaTime * 20);
    }

    void Shoot()
    {
        if (Input.GetButton("Fire1"))
        {
            _weapon.Fire();
            _aimRigWeightTarget = 1;
            if(_moveDirection.x == 0 && _moveDirection.z == 0)
                UpdateAnim("firing");
        }
    }

    void UpdateAnim(string animBool)
    {
        if (animBool == _currentAnimBool) return;

        _anim.SetBool(_currentAnimBool, false);
        _currentAnimBool = animBool;
        _anim.SetBool(_currentAnimBool, true);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        UIManager.Instance.UpdateGameplayPanel();

        if (CurrentHealth <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.GameOver();
        }
    }

    private void OnDisable()
    {
        UpdateAnim("idle");
    }
}