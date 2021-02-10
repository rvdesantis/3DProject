using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArrow : MonoBehaviour
{    

    public void ArrowTrigger()
    {
        GetComponent<Animator>().SetTrigger("trigger");
    }


}
