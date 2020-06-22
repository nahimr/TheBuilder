using UI;
using UnityEngine;
public class Mount : MonoBehaviour
{
   public ushort id;
   public enum Type
   {
      Dynamic,
      Static
   }
   public enum Options
   {
      VelocityUpgrader,
      Jetpack,
      BrickSlot
   }

   public ushort velocitySpeed;
   public ushort jetpackForce;
   public ushort jetpackAmount;
   
   
   public Type typeOfMount;
   public Options optionsOfMount;
   public Vector3 scale;
   public Vector3 offset;

   private bool _brickTaken;
   private RaycastHit2D _brickSlot;
   private ushort _jetpackAmount;
   
   private void Awake()
   {
      var transform1 = transform;
      transform1.localScale = scale;
      transform1.position += offset;
   }

   private void Start()
   {
      GameEvents.Current.OnSpecial += Action;
      if (typeOfMount != Type.Dynamic) return;
      switch (optionsOfMount)
      {
         case Options.VelocityUpgrader:
            Player.Instance.speed = velocitySpeed;
            break;
         case Options.Jetpack:
            UI_InGame.Instance.jetpackBar.maxValue = jetpackAmount;
            UI_InGame.Instance.jetpackBar.value = jetpackAmount;
            UI_InGame.Instance.jetpackBar.gameObject.SetActive(true);
            _jetpackAmount = jetpackAmount;
            break;
      }
   }
   

   private void Action()
   {
      if (typeOfMount != Type.Dynamic) return;
      switch (optionsOfMount)
      {
         case Options.Jetpack:
            if (_jetpackAmount == 0) return;
            Player.Instance.rigidbodyPly.AddForce(Vector2.up * jetpackForce, ForceMode2D.Force);
            _jetpackAmount--;
            var division = (float) _jetpackAmount / jetpackAmount;
            UI_InGame.Instance.jetpackBar.value = _jetpackAmount;
            UI_InGame.Instance.jetpackBarImage.color = Color.Lerp(new Color(0.95f, 0.30f, 0.29f),new Color(0.13f, 0.65f, 0.70f), division);
            break;
         case Options.BrickSlot when !_brickTaken:
         {
            const float dist = 1f;
            const float radius = 2f;
            var transform1 = transform;
            var hit = Physics2D.CircleCast(transform1.position, radius, transform1.right, dist, Player.Instance.brickLayer);
            if (!hit.collider) return;
            if (!hit.rigidbody.CompareTag("Brick") || hit.rigidbody.bodyType != RigidbodyType2D.Dynamic) return;
            _brickSlot = hit;
            _brickTaken = true;
            _brickSlot.collider.enabled = false;
            _brickSlot.rigidbody.bodyType = RigidbodyType2D.Static;
            _brickSlot.rigidbody.gravityScale = 0.0f;
            _brickSlot.transform.gameObject.SetActive(false);
            break;
         }
         case Options.BrickSlot:
         {
            if(_brickTaken)
            {
               _brickSlot.transform.gameObject.SetActive(true);
               var transform1 = transform;
               _brickSlot.transform.position = transform1.position;
               _brickSlot.rigidbody.bodyType = RigidbodyType2D.Dynamic;
               _brickSlot.collider.enabled = true;
               _brickSlot.rigidbody.gravityScale = 1.0f;
               _brickSlot.rigidbody.AddForce(transform1.right * 1.0f, ForceMode2D.Impulse);
               _brickTaken = false;
            }
            break;
         }
      }
   }
}
