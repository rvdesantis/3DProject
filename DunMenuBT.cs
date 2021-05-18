using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DunMenuBT : MonoBehaviour
{
    public AreaUIController uiController;
    public Button selfBT;
    public Image arrow;
    public Animator arrowAnim;
    public int btIndex;
    public bool mainMenu;

    public bool selected;



    private void Start()
    {        
        if (mainMenu == false)
        {
            foreach (DunMenuBT button in uiController.homeButtons)
            {
                button.btIndex = uiController.homeButtons.IndexOf(button);
            }
        }
        if (mainMenu)
        {
            foreach (DunMenuBT button in uiController.menuButtons)
            {
                button.btIndex = uiController.menuButtons.IndexOf(button);
            }
        }
    }
    
    public void MenuBTIndex()
    {
        uiController.homeBTIndex = btIndex;
    }



    private void Update()
    {
        if (btIndex == uiController.homeBTIndex)
        {
            arrowAnim.SetBool("blink", true);
        }
        if (btIndex != uiController.homeBTIndex)
        {
            arrowAnim.SetBool("blink", false);
        }     
        
    }
}
