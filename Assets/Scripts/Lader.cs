using UnityEngine;

public class Lader : MonoBehaviour
{
    public Transform startPoint;
    public Vector3 offset;
    private void Awake()
    {
        if (GlobalData.NumberOfBricksToWin <= 32)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        var tempTransform = startPoint;
        tempTransform.position -= offset;
        GlobalData.ScaleAround(transform, tempTransform, new Vector3(transform.localScale.x, (GlobalData.NumberOfBricksToWin * 0.5f) / GlobalData.NumberOfRowsTower, 1.0f));
    }
    
}
