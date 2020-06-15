using UnityEngine;
using Random = UnityEngine.Random;

namespace Items
{
    public class Heart : MonoBehaviour
    {
        public ushort health;
        public bool random;
        public ushort minHealth;
        public ushort maxHealth;
        
        private void Awake()
        {
            if (random)
            {
                health = (ushort) Random.Range(minHealth, maxHealth);
            }
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var ply = other.GetComponent<Player>();
            if (ply.health + health >= 100.0f) return;
            ply.health += health;
            Destroy(gameObject);
        }
    }
}
