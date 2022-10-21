using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EagleSpawner : MonoBehaviour
{
    [SerializeField] GameObject eaglePrefab;
    [SerializeField] int spawnZPos = 7;
    [SerializeField] Player player;
    [SerializeField] float timeOut = 10;

    [SerializeField] AudioSource eagleAudio;

    [SerializeField] Image timerBar;
    [SerializeField] float timer = 0;
    int playerLastMaxTravel = 0;

    [SerializeField] TMP_Text timerText;

    private void SpawnEagle()
    {
        player.enabled = false;

        eagleAudio.Play(0);

        var position = new Vector3(player.transform.position.x, 1, player.CurrentTravel + spawnZPos);
        var rotation = Quaternion.Euler(0, 180, 0);
        var eagleObject = Instantiate(eaglePrefab, position, rotation);
        var eagle = eagleObject.GetComponent<Eagle>();
        eagle.SetUpTarget(player);
    }

    private void Update()
    {
        if(player.MaxTravel != playerLastMaxTravel)
        {
            timer = 0;
            playerLastMaxTravel = player.MaxTravel;
            return;
        }

        if(timer < timeOut)
        {
            timer += Time.deltaTime;
            if(player.IsDie == false)
            {
                timerBar.fillAmount = 1 - (timer / timeOut);
                int countdown = (int)(11 - timer);
                timerText.text = countdown.ToString();
            }
            return;
        }

        if (player.IsJumping() == false && player.IsDie == false)
            SpawnEagle();
    }
}
