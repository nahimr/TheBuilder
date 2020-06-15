using UnityEngine;

namespace Items
{
    public class FakeRotation : MonoBehaviour
    {
        private float _scaleX;
        private bool _forward;
        private void Start()
        {
            _scaleX = transform.localScale.x;
            _forward = true;
        }

        private void FixedUpdate()
        {
            if(transform.localScale.x >= _scaleX) _forward = true;
            if (transform.localScale.x > 0.0 && _forward)
            {
                var transform1 = transform;
                var localScale = transform1.localScale;
                localScale = localScale -= new Vector3(0.1f, 0f, 0f);
                transform1.localScale = localScale;
            } else
            {
                _forward = false;
                var transform1 = transform;
                var localScale = transform1.localScale;
                localScale = localScale += new Vector3(0.1f, 0f, 0f);
                transform1.localScale = localScale;
            }
            
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Floor")) return;
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        }
    }
}
