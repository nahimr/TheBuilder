using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private Player _ply;
    public float moveSpeed = 5f;
    private Rigidbody2D _rigidbody;
    private Vector2 _movement;
    public float health = 3.0f;
    private float _maxHealth;
    public float infligateDamage = 1.0f;
    public uint pullbackMagnitude = 3;
    private bool _isPullingBack;

    [Header("HUD")] public Slider healthBar;
    public Canvas canvas;
    public float offset;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _maxHealth = health;
        _ply = GameLogic.Instance.player;
    }
    
    private void Update()
    {
        var position = transform.position;
        var direction = _ply.transform.position - position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rigidbody.rotation = angle;
        direction.Normalize();
        _movement = direction;
        var transform1 = canvas.transform;
        transform1.position = position + Vector3.up * offset;
        transform1.rotation = Quaternion.identity;
        healthBar.value = health / _maxHealth;
        if (!(health <= 0.0f)) return;
        _ply.stamina--;
        EnemySpawner.NumberEnemiesOnFloor--;
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (_isPullingBack) return;
        MoveEnemy(_movement);
    }

    private void MoveEnemy(Vector2 direction)
    {
        _rigidbody.MovePosition((Vector2) transform.position + (direction * (moveSpeed * Time.fixedDeltaTime)));
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        _isPullingBack = false;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {     
        if (other.gameObject.CompareTag("Brick") && other.rigidbody.bodyType == RigidbodyType2D.Static)
        {
            Physics2D.IgnoreCollision(other.collider, GetComponent<Collider2D>());
        }
        
        if (other.gameObject.CompareTag("Player"))
        {
            _isPullingBack = true;
            var direction = transform.position - other.gameObject.transform.position;
            _rigidbody.AddForce(direction * pullbackMagnitude, ForceMode2D.Impulse);
        }
        if (!other.gameObject.CompareTag("Brick")) return;
        if (other.gameObject.GetComponent<Rigidbody2D>().bodyType != RigidbodyType2D.Dynamic) return;
        health -= infligateDamage;
    }
}
