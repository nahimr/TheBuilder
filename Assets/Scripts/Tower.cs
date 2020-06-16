using UnityEngine;
public class Tower : MonoBehaviour
{
    [Header("Tower")]
    public float xSpace, ySpace;
    public Transform startPointTower;
    public Transform startPointBrick;
    public uint laderVelocity;
    private int _numberOfRowsTower;
    
    public int NumberOfBricksPlaced { get; private set; }
    
    private int _numberItem;
    public static Tower Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        NumberOfBricksPlaced = 0;
    }

    private void Start()
    {
        _numberOfRowsTower = GameLogic.Instance.numberOfRowsTower;
        GlobalData.ScaleAround(transform, startPointTower, new Vector3(_numberOfRowsTower, (GlobalData.NumberOfBricksToWin * 0.5f) / _numberOfRowsTower, 1.0f) );
    }

    private void AddGrid(GameObject prefab)
    {
        var position1 = startPointBrick.position;
        var position = new Vector3(position1.x + (xSpace * (_numberItem % _numberOfRowsTower)),
            position1.y + (ySpace * (_numberItem / _numberOfRowsTower)));
        prefab.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        prefab.transform.position = position;
        prefab.transform.rotation = Quaternion.identity;
        _numberItem++;
        NumberOfBricksPlaced++;
        GameLogic.Instance.player.stamina += 50.0f / GlobalData.NumberOfBricksToWin;
    } 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag($"Brick")) return;
        AddGrid(other.gameObject);
        GlobalData.NumberOfBricksOnFloor--;
    }
}
