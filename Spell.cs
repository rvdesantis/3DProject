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
    public float damageTimer;

    public Sprite panelImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, spellSpeed);
        if (Vector3.Distance(transform.position, targetPosition) < .35f)
        {
            gameObject.SetActive(false);
            Destroy(this); // not destroying spell for some reason.
        }
    }
}
