using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    private GameObject _gameObject;
    private float _price;
    private string _nameObject;
    private Texture2D _imageTexture;
    public Text nameObjectText;
    public Image imageObject;
    
    public void Init(GameObject gameObject, Texture2D image, string nameObject, float price)
    {
        _gameObject = gameObject;
        _price = price;
        _nameObject = nameObject;
        _imageTexture = image;
    }
    
    private void Start()
    {
        nameObjectText.text = _nameObject; 
        imageObject.sprite = Sprite.Create(_imageTexture, new Rect(0.0f, 0.0f, 128.0f, 128.0f), _gameObject.transform.position,128.0f);
    }


}
