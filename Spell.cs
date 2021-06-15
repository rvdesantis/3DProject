using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    public Vector3 targetPosition;

    
    public string spellName;
    public string spellInfo;
    public int power;
    public int manaCost;

    
    public enum DamageType {Fire, Ice, Nature, Dark, Normal }
    public DamageType damage;

    public bool projectile;
    public bool targetALL;
    public bool impactRange;
    public float spellSpeed;
    public float castingTime;
    public float damageTimer;

    public Sprite panelImage;
    public AudioSource audioSource;
    public AudioClip castSound;
    public AudioClip impactSound;

    // Start is called before the first frame update
    void Start()
    {
        if (projectile)
        {
            transform.LookAt(targetPosition);
        }        
        if (castSound != null)
        {
            audioSource.clip = castSound;
            audioSource.Play();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (projectile)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, spellSpeed);
            if (Vector3.Distance(transform.position, targetPosition) < .25f)
            {
                if (impactRange == false)
                {
                    impactRange = true;
                    if (impactSound != null)
                    {
                        audioSource.clip = impactSound;
                        audioSource.PlayOneShot(impactSound, 1); // PlayOneShot used so impactSound does not stop playing castSound
                        IEnumerator SoundTimer()
                        {
                            yield return new WaitForSeconds(impactSound.length);
                            gameObject.SetActive(false);
                        }
                        StartCoroutine(SoundTimer());
                        return;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                }             
            }
        }

    }
}
