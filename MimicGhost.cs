using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicGhost : MonoBehaviour
{

    public Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        Bite();        
    }


    public void Bite()
    {
        IEnumerator BiteLoop()
        {
            int x = Random.Range(0, 3);
            if (x == 0)
            {
                anim.SetTrigger("idleBreak");
            }
            if (x == 1)
            {
                anim.SetTrigger("attack1");
            }
            if (x == 2)
            {
                anim.SetTrigger("attack2");
            }
            yield return new WaitForSeconds(3);
            Bite();
        } StartCoroutine(BiteLoop());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
