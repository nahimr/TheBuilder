using System;
using UnityEngine;
public class Player : MonoBehaviour
{
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private float _horizontal;
    private float _vertical;
    private bool _isGrounded;
    public LayerMask solidLayer;
    public LayerMask brickLayer;
    public Camera mainCamera;
    private bool _objectTook;
    private RaycastHit2D _objectTaken;
    public JoystickButton jumpButton;
    public JoystickButton takeButton;
    public FixedJoystick moveStick;
    public Vector3 cameraOffset;
    
    
    private void Awake()
    {
        _objectTook = false;
        jumpButton.take = false;
        takeButton.take = true;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        GameEvents.Current.OnTake += Take;
    }

    private void MoveEvent()
    {
        _horizontal = !GlobalData.IsSmartphone ? Input.GetAxisRaw("Horizontal") : moveStick.Horizontal;

        if (!_objectTook)
        {
            if(Mathf.Abs(_rigidbody.velocity.x) > 20.0f) return;
            if(Mathf.Abs(_horizontal) > 0.1f)
                Move(new Vector2(_horizontal / 2.0f,0.0f));
        }
        else
        {
            if(Mathf.Abs(_rigidbody.velocity.x) > (100.0f + GlobalData.Stamina) / 10.0f) return;
            if (Mathf.Abs(_horizontal) > 0.1f)
                Move(new Vector2(_horizontal / 4.0f, 0.0f));

            /*
            if(Mathf.Abs(_rigidbody.velocity.x) > 10.0f) return;
            if(Mathf.Abs(_horizontal) > 0.1f)
                Move(new Vector2(_horizontal / 4.0f,0.0f));*/
        }
    }
    private void Move(Vector2 value)
    {
        if (value.x < 0.0f)
        {
            _spriteRenderer.flipX = true;
        }
        else if (value.x > 0.0f)
        {
            _spriteRenderer.flipX = false;
        }
        _rigidbody.AddForce(value , ForceMode2D.Impulse);
    }
    
    private void Jump()
    {
        if ((!Input.GetButton("Jump") && !jumpButton.pressed) || !IsGrounded()) return;
        Move(new Vector2(0.0f, 3.0f));

    }

    private void TakeUpdate()
    {
        if (_objectTook)
        {
            _objectTaken.transform.position = _rigidbody.position;
            _objectTaken.transform.rotation = Quaternion.identity;
        }
       
        if (!Input.GetButtonDown("Take") || GlobalData.IsSmartphone) return;
        GameEvents.Current.Take();
    }
    
    
    private void Take()
    {
        Vector3 direction;
        if (_spriteRenderer.flipX)
        {
            direction = -transform.right;
        }
        else
        {
            direction = transform.right;
        }
        if (!_objectTook)
        {
            const float dist = 1.0f;
            const float radius = 2.50f;
            var hit = Physics2D.CircleCast(transform.position, radius, direction, dist, brickLayer);
            if (!hit.collider) return;
            if (!hit.rigidbody.CompareTag("Brick") || hit.rigidbody.bodyType != RigidbodyType2D.Dynamic ) return;
            if (!ObjectIsGrounded(hit.transform)) return;
            _objectTaken = hit;
            _objectTook = true;
            _objectTaken.collider.enabled = false;
            _objectTaken.rigidbody.bodyType = RigidbodyType2D.Static;
            _objectTaken.rigidbody.gravityScale = 0.0f;
        }
        else if(_objectTook && Mathf.Abs(moveStick.Horizontal) > 0.0f || Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0.0f )
        {
            var tempHorizontal = !GlobalData.IsSmartphone ? Input.GetAxisRaw("Horizontal") : moveStick.Horizontal;
            _objectTaken.rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _objectTaken.collider.enabled = true;
            _objectTaken.rigidbody.gravityScale = 1.0f;
            _objectTaken.rigidbody.AddForce(Vector2.right * (1200.0f * tempHorizontal),ForceMode2D.Impulse);
            _objectTook = false;
            
        }
    }
    
    
    private bool IsGrounded() {
        Vector2 position = transform.position;
        var direction = Vector2.down;
        const float distance = 1.40f;
        var hit = Physics2D.Raycast(position, direction, distance, solidLayer | brickLayer);
        return hit.collider != null;
    }

    private bool ObjectIsGrounded(Transform obj)
    {
        var pos = obj.position;
        var dir = Vector2.down;
        const float distance = 0.50f;
        var hit = Physics2D.Raycast(pos, dir, distance, solidLayer);
        return hit.collider != null;
    }
    
    private void Update()
    {
        var transform1 = mainCamera.transform;
        Vector3 position =  _rigidbody.position;
        position += Vector3.forward * -10.0f;
        position += Vector3.up * 2.0f;
        position += cameraOffset;
        transform1.position = position;
        var transform2 = transform;
        if (_spriteRenderer.flipX)
        {
            
            Debug.DrawRay(transform2.position, -transform2.right, Color.magenta);
        }
        else
        {
            Debug.DrawRay(transform2.position, transform2.right, Color.magenta);
        }
        TakeUpdate();
    }

    private void FixedUpdate()
    {
        MoveEvent();
        Jump();
        if (GlobalData.Stamina >= 50.0f)
            GlobalData.Stamina -= 0.01f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var direction = transform.position - other.gameObject.transform.position;
        if (other.gameObject.CompareTag("Enemy") && _rigidbody.velocity.magnitude < 6.0f)
        {
            
            _rigidbody.AddForce(direction * other.gameObject.GetComponent<Enemy>().pullbackMagnitude, ForceMode2D.Impulse);

            if (GlobalData.Health > 0.0f) GlobalData.Health--;
            if (GlobalData.Stamina < 100.0f) GlobalData.Stamina += 2.0f;
        }
        if (!(Mathf.Abs(direction.x) < Mathf.Abs(direction.y)) || !(direction.y < 0) ||
            !other.gameObject.CompareTag($"Brick")) return;
        if (GlobalData.Health > 0.0f) GlobalData.Health--;
        if (GlobalData.Stamina < 100.0f) GlobalData.Stamina += 2.0f;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Lader"))
        {
            _rigidbody.gravityScale = 0.0f;
            
        }
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Lader")) return;
        var tempVertical = !GlobalData.IsSmartphone ? Input.GetAxisRaw("Vertical") : moveStick.Vertical;
        Move(new Vector2(0.0f, tempVertical));
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Lader")) return;
        _rigidbody.gravityScale = 2.0f;
    }
}
