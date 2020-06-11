using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject[] elements;
    public GameObject referenceObject;
    private GridLayoutGroup _gridLayoutGroup;
    public Object[][] objections;
    private void Awake()
    {
        _gridLayoutGroup = GetComponent<GridLayoutGroup>();
        foreach (var t in elements)
        {
            var instantiate = Instantiate(referenceObject, _gridLayoutGroup.transform, false);
           // instantiate.GetComponent<ShopElement>().Init(t, AssetPreview.GetAssetPreview(t), t.name, 15.0f);
        }
    }
}
