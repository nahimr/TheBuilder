using System;
using UnityEngine;
public class Bullet : MonoBehaviour
{
    [HideInInspector]
    public uint speed;
    [HideInInspector]
    public uint damage;
    [HideInInspector]
    public Vector2 direction;
    private Rigidbody2D _rigidbody;


    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _rigidbody.AddForce(direction * speed, ForceMode2D.Impulse);
    }

    private void Update()
    {
        Destroy(gameObject, 15);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().health -= damage;
        }
        Destroy(gameObject);
    }
    
}
