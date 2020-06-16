using UnityEngine;

public class ConstraintAxis : MonoBehaviour
{
    public Vector3 scale;
    private void Start()
    {
        transform.localScale = scale;
    }
}
