using UnityEngine;
public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    public LayerMask solidLayer;
    public LayerMask brickLayer;
    public Camera mainCamera;
    public Vector3 cameraOffset;
    public Transform grabTransform;
    public float health;
    public float stamina;
    public uint speed;
    public bool canThrow;
    public bool canThrowOnTop;
    public GameObject mount;
    public GameObject weapon;
    private Collider2D _collider;
    public Rigidbody2D rigidbodyPly;
    private float _horizontal;
    private float _vertical;
    private bool _isGrounded;
    private float _brickVelocityOnThrow = 1200.0f;
    private bool _objectTook;
    private RaycastHit2D _objectTaken;
    private bool _haveWeapon;
    private bool _haveMount;
    public static Player Instance { get; set; }

    public bool HaveWeapon
    {
        get => _haveWeapon;
        set => _haveWeapon = value;
    }

    private void Awake()
    {
        Instance = this;
        _objectTook = false;
        _collider = GetComponent<Collider2D>();
        health = 100.0f;
        stamina = 0.0f;
        _haveMount = mount;
        _haveWeapon = weapon;
        
        var transform1 = transform;
        if (_haveWeapon)
        {
           weapon = Instantiate(weapon, transform1.position, Quaternion.identity, transform1);
        }

        if (_haveMount)
        {
            Instantiate(mount, transform1.position, Quaternion.identity, transform1);
        }
    }
    private void Start()
    {
        SmartphoneJoysticks.Instance.jumpButton.take = false;
        SmartphoneJoysticks.Instance.takeButton.take = true;
        rigidbodyPly = GetComponent<Rigidbody2D>();
        GameEvents.Current.OnTake += Take;
    }
    private static void Fire()
    {
        if (!SmartphoneJoysticks.Instance.fireButton.pressed && !Input.GetButton("Fire")) return;
        GameEvents.Current.Fire();
    }
    private void MoveEvent()
    {
        _horizontal = !GlobalData.IsSmartphone ? Input.GetAxisRaw("Horizontal") : SmartphoneJoysticks.Instance.moveStick.Horizontal;
        if (!_objectTook)
        {
            if(Mathf.Abs(rigidbodyPly.velocity.x) > 20.0f) return;
            if(Mathf.Abs(_horizontal) > 0.1f)
                Move(new Vector2(_horizontal / 2.0f,0.0f));
        }
        else
        {
            if(Mathf.Abs(rigidbodyPly.velocity.x) > (100.0f + stamina) / 10.0f) return;
            if (Mathf.Abs(_horizontal) > 0.1f)
                Move(new Vector2(_horizontal / 4.0f, 0.0f));
        }
    }
    private void Move(Vector2 value)
    {
        if (value.x < 0.0f)
        {
            var transform1 = transform;
            var localScale = transform1.localScale;
            localScale = new Vector3(-1.0f, localScale.y, localScale.z);
            transform1.localScale = localScale;
        }
        else if (value.x > 0.0f)
        {
            var transform1 = transform;
            var localScale = transform1.localScale;
            localScale = new Vector3(1.0f, localScale.y, localScale.z);
            transform1.localScale = localScale;
        }
        rigidbodyPly.AddForce(value * speed, ForceMode2D.Impulse);
    }
    
    private void Jump()
    {
        if ((!Input.GetButton("Jump") && !SmartphoneJoysticks.Instance.jumpButton.pressed) || !IsGrounded()) return;
        Move(new Vector2(0.0f, 3.0f));

    }

    private void TakeFixedUpdate()
    {
        if (!_objectTook) return;
        _objectTaken.transform.position = grabTransform.position;
        _objectTaken.transform.rotation = Quaternion.identity;
    }

    private static void TakeUpdate()
    {
        if (!Input.GetButtonDown("Take") || GlobalData.IsSmartphone || Time.timeScale <= 0.0f) return;
        GameEvents.Current.Take();
    }
    private void Take()
    {
        var direction = transform.localScale.x * Vector2.right;
        if (!_objectTook)
        {
            const float dist = 1f;
            const float radius = 2f;
            var hit = Physics2D.CircleCast(grabTransform.position, radius, direction, dist, brickLayer);
            if (!hit.collider) return;
            if (!hit.rigidbody.CompareTag("Brick") || hit.rigidbody.bodyType != RigidbodyType2D.Dynamic) return;
            if (!ObjectIsGrounded(hit.transform)) return;
            _objectTaken = hit;
            _objectTook = true;
            _objectTaken.collider.enabled = false;
            _objectTaken.rigidbody.bodyType = RigidbodyType2D.Static;
            _objectTaken.rigidbody.gravityScale = 0.0f;
        }
        else if(_objectTook)
        {
            _objectTaken.rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _objectTaken.collider.enabled = true;
            _objectTaken.rigidbody.gravityScale = 1.0f;
            if (canThrow)
            {
                if (canThrowOnTop && SmartphoneJoysticks.Instance.moveStick.Vertical > 0.0f || Input.GetAxisRaw("Vertical") > 0.0f)
                {
                    _objectTaken.rigidbody.AddForce(Vector3.up * _brickVelocityOnThrow / 2.0f, ForceMode2D.Impulse);
                }
                else
                {
                    _objectTaken.rigidbody.AddForce(direction * _brickVelocityOnThrow, ForceMode2D.Impulse);
                }
            }
            else
            {
                _objectTaken.rigidbody.AddForce(direction * 1.0f, ForceMode2D.Impulse);
            }
            _objectTook = false;
        }
    }
    
    
    private bool IsGrounded() {
        var direction = Vector2.down;
        const float distance = 0.75f;
        var hit = Physics2D.CircleCast(_collider.bounds.center,0.50f, direction, distance, solidLayer | brickLayer);
        return hit.collider != null;
    }

    private bool ObjectIsGrounded(Transform obj)
    {
        var pos = obj.position;
        var dir = Vector2.down;
        const float distance = 1.10f;
        var hit = Physics2D.Raycast(pos, dir, distance, solidLayer);
        return hit.collider != null;
    }
    
    private void Update()
    {
        var transform1 = mainCamera.transform;
        Vector3 position =  rigidbodyPly.position;
        position += Vector3.forward * -10.0f;
        position += Vector3.up * 2.0f;
        position += cameraOffset;
        transform1.position = position;
        TakeUpdate();

        if (_haveWeapon)
        {
            Fire();
        }

        if (_haveMount)
        {
            Action();
        }
    }

    private static void Action()
    {
        if (!SmartphoneJoysticks.Instance.specialButton.pressed && !Input.GetButton("SpecialButton")) return;
        GameEvents.Current.Special();
    }
    
    private void FixedUpdate()
    {
        MoveEvent();
        Jump();
        TakeFixedUpdate();
        if (stamina >= 50.0f)
            stamina -= 0.01f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var direction = transform.position - other.gameObject.transform.position;
        if (other.gameObject.CompareTag("Enemy") && rigidbodyPly.velocity.magnitude < 6.0f)
        {
            rigidbodyPly.AddForce(direction * other.gameObject.GetComponent<Enemy>().pullbackMagnitude, ForceMode2D.Impulse);
            if (health > 0.0f) health--;
        }
        if (!(Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) || !(direction.y < 0) ||
            !other.gameObject.CompareTag($"Brick")) return;
        if (health > 0.0f) health--;
        if (stamina < 100.0f) stamina += 2.0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Lader")) return;
        rigidbodyPly.gravityScale = 0.0f;
        Move(new Vector2(0.0f,  Tower.Instance.laderVelocity));
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Lader")) return;
        rigidbodyPly.gravityScale = 2.0f;
    }
}
