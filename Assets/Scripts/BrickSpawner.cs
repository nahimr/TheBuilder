using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickSpawner : MonoBehaviour
{
    public GameObject brick;
    private RectTransform _rectTransform;
    public GameObject characterFollowingPrefab;
    private Transform _characterFollowing;
    private float _timer;
    public float timerTime = 30.0f;
    private Vector2 _randomVector3;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _characterFollowing = Instantiate(characterFollowingPrefab, transform.position, Quaternion.identity).transform;
        _randomVector3 = RandomVector2();
    }
    private Vector2 RandomVector2()
    {
        var position = _rectTransform.position;
        return new Vector2(Random.Range((int)position.x,(int)position.x + _rectTransform.rect.width),(int)position.y);
    }
    
    private void FixedUpdate()
    {
        var lerpPos = Vector2.Lerp(_characterFollowing.position, _randomVector3, Time.fixedDeltaTime);
        _characterFollowing.position = lerpPos;
        _timer += Time.fixedDeltaTime;
        if (!(Math.Abs(_characterFollowing.position.x - _randomVector3.x) < 1.0f)) return;
        if (!(_timer >= timerTime) || GlobalData.NumberOfBricksOnFloor >= GlobalData.NumberOfBricksToWin || GlobalData.NumberOfBricksOnFloor >= 0.25f * GlobalData.NumberOfBricksToWin ) return;
        Instantiate(brick, _randomVector3, Quaternion.identity, transform);
        GlobalData.NumberOfBricksOnFloor++;
        _timer = 0.0f;
        _randomVector3 = RandomVector2();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "brickspawner.png", false);
    }
}
