using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Player player;
    
    [SerializeField] GameObject gameOverPanel;
    
    [SerializeField] GameObject grass;
    [SerializeField] GameObject road;

    [SerializeField] int extent = 7;
    [SerializeField] int frontDistance = 21;
    [SerializeField] int backDistance = -5;
    [SerializeField] int maxSameTerrainRepeat = 3;

    Dictionary<int, TerrainBlock> map = new Dictionary<int, TerrainBlock>(50);

    [SerializeField] TMP_Text gameOverText;

    //[SerializeField] GameObject muteIcon;
    //[SerializeField] GameObject unmuteIcon;

    private void Start()
    {
        gameOverPanel.SetActive(false);
        //gameOverText = gameOverPanel.GetComponentInChildren<TMP_Text>();

        //if (audioSource.mute)
        //{
        //    unmuteIcon.SetActive(true);
        //}
        //else
        //{
        //    muteIcon.SetActive(true);
        //}

        for (int z = backDistance; z <= 0; z++)
        {
            CreateTerrain(grass, z);
        }

        for(int z = 1; z < frontDistance; z++)
        {
            var prefab = GetNextRandomTerrainPrefab(z);

            CreateTerrain(prefab, z);
        }

        player.SetUp(backDistance, extent);
    }

    private int playerLastMaxTravel;
    private void Update()
    {
        if (player.IsDie && gameOverPanel.activeInHierarchy == false)
            StartCoroutine(ShowGameOverPanel());

        if (player.MaxTravel == playerLastMaxTravel)
            return;

        playerLastMaxTravel = player.MaxTravel;

        var randTbPrefab = GetNextRandomTerrainPrefab(player.MaxTravel + frontDistance - 1);
        CreateTerrain(randTbPrefab, player.MaxTravel + frontDistance - 1);

        var lastTB = map[player.MaxTravel - 1 + backDistance];

        map.Remove(player.MaxTravel - 1 + backDistance);
        Destroy(lastTB.gameObject);

        player.SetUp(player.MaxTravel + backDistance, extent);
    }

    IEnumerator ShowGameOverPanel()
    {
        yield return new WaitForSeconds(1);

        //player.enabled = false;
        gameOverText.text = "SCORE: " + player.MaxTravel;
        gameOverPanel.SetActive(true);
    }

    private void CreateTerrain(GameObject prefab, int zPos)
    {
        var go = Instantiate(prefab, new Vector3(0, 0, zPos), Quaternion.identity);
        var tb = go.GetComponent<TerrainBlock>();
        tb.Build(extent);

        map.Add(zPos, tb);
    }

    private GameObject GetNextRandomTerrainPrefab(int pos)
    {
        bool isUniform = true;
        var tbRef = map[pos - 1];

        for(int distance = 2; distance <= maxSameTerrainRepeat; distance++)
        {
            if (map[pos - distance].GetType() != tbRef.GetType())
            {
                isUniform = false;
                break;
            }
        }

        if(isUniform)
        {
            if (tbRef is Grass)
                return road;
            else
                return grass;
        }

        return Random.value > 0.5f ? road : grass;
    }

}
