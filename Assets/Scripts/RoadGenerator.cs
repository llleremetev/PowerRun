using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class RoadGenerator : MonoBehaviour
{
    public GameObject RoadPrefab;
    private List<GameObject> roads = new List<GameObject>();
    public float maxSpeed = 10;
    private float speed = 0;
    public int maxRoadCount = 5;
    public GameObject player;
    void Start()
    {
        ResetLevel();
    }

    void Update()
    {
        if (speed == 0) return;

        foreach (GameObject road in roads)
        {
            road.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }

        if (roads[0].transform.position.z < -65)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);

            CreateNextRoad();
        }
    }

    private void CreateNextRoad()
    {
        Vector3 pos = Vector3.zero;
        if(roads.Count>0) { pos = roads[roads.Count - 1].transform.position + new Vector3(0, 0, 65); }
       GameObject go = Instantiate(RoadPrefab, pos, Quaternion.identity);
        go.transform.SetParent(transform);
        roads.Add(go);
    }

    public void StartLevel()
    {
        speed = maxSpeed;
        SwipeManager.instance.enabled = true;
    }

    public void PauseLevel()
    {
        speed = 0;
        SwipeManager.instance.enabled = false;
    }
    public void ResetLevel()
    {
        speed = 0;
        while(roads.Count > 0)
        {
            Destroy(roads[0]);
            roads.RemoveAt(0);

            player.transform.position = new Vector3(0, 0.5f, 0);
        }
        for(int i=0;i<maxRoadCount;i++)
        {
            CreateNextRoad();
        }
        SwipeManager.instance.enabled = false;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
