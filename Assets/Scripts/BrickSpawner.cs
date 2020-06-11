using UnityEngine;
public class BrickSpawner : MonoBehaviour
{
    public GameObject brick;
    public GameObject brickGroup;
    public int brickLimit;
    private RectTransform _rectTransform;
    private float _timer = 0.0f;
    public float timerTime = 30.0f;
    // TODO: Implement Static Int to know how many instances of this class
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    
    private void Update()
    {
        var position = _rectTransform.position;
        var randomVector3 = new Vector2(Random.Range(position.x,position.x + _rectTransform.rect.width),position.y);
        _timer += Time.fixedDeltaTime;
        if (!(_timer >= timerTime) || brickGroup.transform.childCount >= GlobalData.NumberOfBricksToWin || GlobalData.NumberOfBricksOnFloor >= 0.25f * GlobalData.NumberOfBricksToWin) return;
        Instantiate(brick, randomVector3, Quaternion.identity, brickGroup.transform);
        GlobalData.NumberOfBricksOnFloor++;
        _timer = 0.0f;
    }
}
