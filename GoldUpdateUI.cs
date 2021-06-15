using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldUpdateUI : MonoBehaviour
{
    public Text goldText;
       
    void Start()
    {
        
    }

    
    void Update()
    {
        goldText.text = StaticMenuItems.goldCount.ToString();
    }
}
