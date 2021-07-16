using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UnlockedUI : MonoBehaviour
{
    public Text unlockType;
    public Text unlockInfo;
    public Button nextBT;
    public RawImage rawImage;
    public VideoPlayer videoPlayer;
    public List<VideoClip> vidClips;
    public int clipNumber;

    public bool darkElfUnlock;
    public bool guideUnlocked;
    public bool medusaUnlocked;

    public bool cycle;


    public void UnlockBT()
    {
        nextBT.Select();
        Cursor.lockState = CursorLockMode.None;
        if (cycle == true)
        {
            if (darkElfUnlock)
            {
                darkElfUnlock = false;
                cycle = true;                         
                unlockType.text = unlockType + " Character";
                return;
            }

            if (guideUnlocked)
            {
                unlockType.text = unlockType + " Agent";
                guideUnlocked = false;
                cycle = true;
                videoPlayer.clip = vidClips[0];
                videoPlayer.Play();                
                return;
            }

            if (medusaUnlocked)
            {
                darkElfUnlock = false;
                cycle = true;
                // set video clip                
                unlockType.text = unlockType + " Agent";
                return;
            }
            else
            {
                cycle = false;
                Debug.Log("Unlock Cycle Finished");
            }
            

        }
        if (cycle == false)
        {
            if (FindObjectOfType<AreaUIController>().dunClearedUI.gameObject.activeSelf == false)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }            
            this.gameObject.SetActive(false);
        }        
    }

    private void Update()
    {
        if (this.gameObject.activeSelf)
        {
            FindObjectOfType<AreaUIController>().uiNavigation = true;
        }
    }



}
