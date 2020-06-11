using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform ply;
    public float moveSpeed = 5f;
    private Rigidbody2D _rigidbody;
    private Vector2 _movement;
    public float health = 3.0f;
    public float infligateDamage = 1.0f;
    public uint pullbackMagnitude = 3;
    private bool _isPullingBack;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void Update()
    {
        var direction = ply.position - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _rigidbody.rotation = angle;
        direction.Normalize();
        _movement = direction;
        if (health <= 0.0f)
        {
            Destroy(gameObject);
        }
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
        if (other.gameObject.CompareTag("Player"))
        {
            _isPullingBack = true;
            var direction = transform.position - other.gameObject.transform.position;
            _rigidbody.AddForce(direction * pullbackMagnitude, ForceMode2D.Impulse);
        }
        if (!other.gameObject.CompareTag("Brick")) return;
        health -= infligateDamage;
    }
}
