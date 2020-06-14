using UnityEngine;
public class MenusLogic : MonoBehaviour
{

    public GameObject shopMenu;

    public GameObject mainMenu;
    
    public GameObject levelsMenu;

    public void MainMenu(bool pEnabled)
    {
        mainMenu.SetActive(pEnabled);
    }

    public void LevelsMenu(bool pEnabled)
    {
        if (pEnabled)
        {
            shopMenu.SetActive(false);
            mainMenu.SetActive(false);
            levelsMenu.SetActive(true);
        }
        else
        {
            shopMenu.SetActive(false);
            mainMenu.SetActive(true);
            levelsMenu.SetActive(false);
        }
    }

    public void ShopMenu(bool pEnabled)
    {
        shopMenu.SetActive(pEnabled);
        mainMenu.SetActive(!pEnabled);
    }


    
}
