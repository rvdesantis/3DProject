using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class UnlockedUI : MonoBehaviour
{
    public Text unlockType;
    public Text unlockInfo;
    public Button confirmBT;
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
        if (cycle == true)
        {
            if (darkElfUnlock)
            {
                darkElfUnlock = false;
                cycle = true;
                // set video clip
                // play video clip
                // set text string

                return;
            }

            if (guideUnlocked)
            {
                guideUnlocked = false;
                cycle = true;
                // set video clip
                // play video clip
                // set text string
                return;
            }

            if (medusaUnlocked)
            {
                darkElfUnlock = false;
                cycle = true;
                // set video clip
                // play video clip
                // set text string
                return;
            }
            else
            cycle = false;

        }
        if (cycle == false)
        {
            FindObjectOfType<AreaUIController>().ToggleUINav();
            this.gameObject.SetActive(false);
        }
        
    }


}
