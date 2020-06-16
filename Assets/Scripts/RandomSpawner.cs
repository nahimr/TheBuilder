using Items;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class ItemsType
{
    public enum ItemsOptions
    {
        Heart,
        Clip
    }
    public GameObject item;
    public ItemsOptions options;
}


public class RandomSpawner : MonoBehaviour
{
    public ItemsType[] objects;
    private RectTransform _rectTransform;
    private float _timer;
    public float timerTime = 30.0f;
    public int numberMaxObjOnFloor;
    public static int NumberObjectsOnFloor;
    
    
    private void Awake()
    {
        NumberObjectsOnFloor = 0;
    }

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var position = _rectTransform.position;
        var randomVector3 = new Vector2(Random.Range(position.x,position.x + _rectTransform.rect.width),position.y);
        if (NumberObjectsOnFloor != numberMaxObjOnFloor)
        {
            _timer += Time.fixedDeltaTime;
        }
        else
        {
            _timer = 0.0f;
        }
        if (!(_timer >= timerTime) || (NumberObjectsOnFloor >= numberMaxObjOnFloor && numberMaxObjOnFloor > 0)) return;
        var randNumber = Random.Range(0, objects.Length - 1);
        
        var randItem = Instantiate(objects[randNumber].item, randomVector3, Quaternion.identity);
        switch (objects[randNumber].options)
        {
            case ItemsType.ItemsOptions.Heart:
            {
                var randItemHeart = randItem.GetComponent<Heart>();
                randItemHeart.random = true;
                randItemHeart.minHealth = (ushort)Random.Range(1, 25);
                randItemHeart.maxHealth = (ushort) Random.Range(randItemHeart.minHealth, 50);
                break;
            }
            case ItemsType.ItemsOptions.Clip when !Player.Instance.HaveWeapon:
                Destroy(randItem);
                return;
            case ItemsType.ItemsOptions.Clip:
            {
                var randItemClip = randItem.GetComponent<Clip>();
                randItemClip.clip = (uint) Random.Range(1, Player.Instance.weapon.GetComponent<Weapon>().maxClip);
                break;
            }
        }
        
        NumberObjectsOnFloor++;
        _timer = 0.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "enemyicon.png", false);
    }
}