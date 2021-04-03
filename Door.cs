using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject player;
    public bool locked;
    public bool inArea;
    public AreaUIController areaUI;
    public Items matchingKey;
    public bool opened;

    public AudioSource audioSource;
    public AudioClip lockedSound;
    public AudioClip openSound;

    // Start is called before the first frame update
    void Start()
    {
        areaUI = FindObjectOfType<AreaUIController>();
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<CharacterController>().gameObject;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < 5)
        {
            inArea = true;
            if (matchingKey == null)
            {
                areaUI.messageText.text = "Door Locked";
                areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
            }
            if (matchingKey != null && locked)
            {
                areaUI.messageText.text = "Door Locked (" + matchingKey.itemName + " missing)";
                areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
            }
            if (locked == false && opened == false)
            {
                areaUI.messageText.text = "Open Door?";
                areaUI.messageUI.GetComponent<Animator>().SetBool("solid", true);
            }
            if (locked == false && opened)
            {
                if (areaUI.messageUI.GetComponent<Animator>().GetBool("solid") == true)
                {
                    areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
                }
            }
        }
        if (Vector3.Distance(player.transform.position, transform.position) >= 5 && inArea == true)
        {
            areaUI.messageUI.GetComponent<Animator>().SetBool("solid", false);
            inArea = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            if (inArea)
            {                
                if (locked == false)
                {                    
                    GetComponent<Animator>().SetTrigger("open");
                    opened = true;
                    audioSource.clip = openSound;
                    audioSource.Play();
                    return;
                }       
                if (locked == true)
                {
                    audioSource.clip = lockedSound;
                    audioSource.Play();
                }
            }
        }
    }
}
