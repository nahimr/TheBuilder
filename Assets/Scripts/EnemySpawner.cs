using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public GameObject enemyGroup;
    public Transform ply;
    private RectTransform _rectTransform;
    private float _timer = 0.0f;
    public float timerTime = 30.0f;
    public int numberMaxEnemyOnFloor;
    // TODO: Implement Static Int to know how many instances of this class and number of enemies
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        enemy.GetComponent<Enemy>().ply = ply;
    }

    
    private void Update()
    {
        var position = _rectTransform.position;
        var randomVector3 = new Vector2(Random.Range(position.x,position.x + _rectTransform.rect.width),position.y);
        _timer += Time.fixedDeltaTime;
        if (!(_timer >= timerTime) || (enemyGroup.transform.childCount >= numberMaxEnemyOnFloor && numberMaxEnemyOnFloor > 0)) return;
        Instantiate(enemy, randomVector3, Quaternion.identity, enemyGroup.transform);
        _timer = 0.0f;
    }
}