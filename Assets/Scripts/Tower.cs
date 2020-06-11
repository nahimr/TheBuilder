using UnityEngine;
public class Tower : MonoBehaviour
{
    public float xSpace, ySpace;
    public Transform startPointTower;
    public Transform startPointBrick;
    private int _numberItem;

    private void Start()
    {
        GlobalData.ScaleAround(transform, startPointTower, new Vector3(GlobalData.NumberOfRowsTower, (GlobalData.NumberOfBricksToWin * 0.5f) / GlobalData.NumberOfRowsTower, 1.0f) );
    }

    private void AddGrid(GameObject prefab)
    {
        var position1 = startPointBrick.position;
        var position = new Vector3(position1.x + (xSpace * (_numberItem % GlobalData.NumberOfRowsTower)),
            position1.y + (ySpace * (_numberItem / GlobalData.NumberOfRowsTower)));
        prefab.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        prefab.transform.position = position;
        prefab.transform.rotation = Quaternion.identity;
        _numberItem++;
        GlobalData.NumberOfBricksPlaced++;
        GlobalData.Stamina += 50.0f / GlobalData.NumberOfBricksToWin;
    } 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag($"Brick")) return;
        AddGrid(other.gameObject);
        GlobalData.NumberOfBricksOnFloor--;
    }



}
