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
        Instantiate(objects[randNumber].item, randomVector3, Quaternion.identity);
        NumberObjectsOnFloor++;
        _timer = 0.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "enemyicon.png", false);
    }
}