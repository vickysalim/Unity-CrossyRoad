using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudio : MonoBehaviour
{
    [SerializeField] AudioSource bgm;
    [SerializeField] AudioSource collision;
    [SerializeField] AudioSource walk;
    [SerializeField] AudioSource eagle;

    [SerializeField] GameObject muteIcon;
    [SerializeField] GameObject unmuteIcon;

    public void Awake()
    {
        if (!PlayerPrefs.HasKey("audio"))
            PlayerPrefs.SetInt("audio", 1);

        if(PlayerPrefs.GetInt("audio") == 1)
        {
            bgm.mute = false;
            collision.mute = false;
            walk.mute = false;
            eagle.mute = false;

            muteIcon.SetActive(true);
        } else
        {
            bgm.mute = true;
            collision.mute = true;
            walk.mute = true;
            eagle.mute = true;

            unmuteIcon.SetActive(true);
        }
    }

    public void ToggleAudio()
    {
        if (PlayerPrefs.GetInt("audio") == 1)
        {
            //bgm.mute = true;
            GameObject.FindWithTag("BGM").GetComponent<AudioSource>().mute = true;
            collision.mute = true;
            walk.mute = true;
            eagle.mute = true;

            muteIcon.SetActive(false);
            unmuteIcon.SetActive(true);

            PlayerPrefs.SetInt("audio", 0);
        } else
        {
            //bgm.mute = false;
            GameObject.FindWithTag("BGM").GetComponent<AudioSource>().mute = false;
            collision.mute = false;
            walk.mute = false;
            eagle.mute = false;

            muteIcon.SetActive(true);
            unmuteIcon.SetActive(false);
            
            PlayerPrefs.SetInt("audio", 1);
        }
    }
}
