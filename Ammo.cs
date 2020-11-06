using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{

    public Vector3 targetPosition;
    public float spellSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {          
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, spellSpeed);

        if (Vector3.Distance(transform.position, targetPosition) < .25f)
        {
            Destroy(this.gameObject, 1);            
        }
    }
}
