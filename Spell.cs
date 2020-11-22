using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public Vector3 targetPosition;

    
    public string spellName;
    public int power;
    public int manaCost;

    public bool projectile;
    public float spellSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, spellSpeed);
    }
}
