using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public Vector3 targetPosition;

    
    public string spellName;
    public int power;
    public int manaCost;

    
    public enum DamageType {Fire, Ice, Nature, Dark, Normal }
    public DamageType damage;

    public bool projectile;
    public bool targetALL;

    public float spellSpeed;
    public float castingTime;
    public float damageTimer;

    public Sprite panelImage;

    // Start is called before the first frame update
    void Start()
    {
        transform.LookAt(targetPosition);

    }

    // Update is called once per frame
    void Update()
    {
        if (projectile)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, spellSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < .25f)
            {
                gameObject.SetActive(false);                
            }
        }

    }
}
