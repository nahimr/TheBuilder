using UnityEngine;

namespace Items
{
    public class Clip : MonoBehaviour
    {
        public uint clip;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            if(other.gameObject.GetComponent<Player>().weapon.GetComponent<Weapon>().AddClip(clip)) Destroy(gameObject);
           
        }
    }
}
