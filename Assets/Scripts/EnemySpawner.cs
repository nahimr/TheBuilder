using UnityEngine;
using Random = UnityEngine.Random;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private RectTransform _rectTransform;
    private float _timer;
    public float timerTime = 30.0f;
    public int numberMaxEnemyOnFloor;
    public static int NumberEnemiesOnFloor;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        var position = _rectTransform.position;
        var randomVector3 = new Vector2(Random.Range(position.x,position.x + _rectTransform.rect.width),position.y);
        if (NumberEnemiesOnFloor != numberMaxEnemyOnFloor)
        {
            _timer += Time.fixedDeltaTime;
        }
        else
        {
            _timer = 0.0f;
        }
        if (!(_timer >= timerTime) || (NumberEnemiesOnFloor >= numberMaxEnemyOnFloor && numberMaxEnemyOnFloor > 0)) return;
        Instantiate(enemy, randomVector3, Quaternion.identity, transform);
        NumberEnemiesOnFloor++;
        _timer = 0.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "enemyicon.png", false);
    }
}