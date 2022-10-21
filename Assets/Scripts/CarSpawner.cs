using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] carPrefab;
    //[SerializeField] GameObject motorcyclePrefab;
    [SerializeField] TerrainBlock terrain;
    
    bool isRight;
    float timer;

    private void Start()
    {
        isRight = Random.value > 0.5f ? true : false;
        timer = Random.Range(0, 1f);
    }
    private void Update()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        timer = Random.Range(1f, 3f);

        var spawnPos = this.transform.position + Vector3.right * (isRight ? -(terrain.Extent * 1) : terrain.Extent + 1);

        //GameObject randomVehicle = Random.value > 0.5f ? carPrefab : motorcyclePrefab;

        int randomNum = Random.Range(0, carPrefab.Length);

        var go = Instantiate(
            original: carPrefab[randomNum],
            position: spawnPos,
            rotation: Quaternion.Euler(0, isRight ? 180 : 0, 0),
            parent: this.transform);

        var car = go.GetComponent<Car>();
        car.SetUp(terrain.Extent);
    }
}
