using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossSpawn : MonoBehaviour
{
    public GameObject boss;
    public float spawnInterval;
    public float spawnRange;
    public float nextSpawnTime;
    public LayerMask collisionLayer;
    private Player player;
    public Text alertBossSpawnText;
    public float alertFadeoutTimer;
    public Transform[] spawnLocations;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<Player>();
        if (nextSpawnTime <= 0)
        {
            nextSpawnTime = spawnInterval;
        }
        if (alertFadeoutTimer <= 0)
        {
            alertFadeoutTimer = 2.0f;
        }
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad >= nextSpawnTime)
        {
            nextSpawnTime = Time.timeSinceLevelLoad + spawnInterval;
            SpawnZumbie(GetMostDistantSpawnPosition());
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        foreach(Transform spawnLocation in spawnLocations)
        {
            Gizmos.DrawWireSphere(spawnLocation.position, spawnRange);
        }
    }

    private void SpawnZumbie(Vector3 position)
    {
        SpawnAlert();
        GameObject.Instantiate(boss, position, this.transform.rotation).GetComponent<Zombie>();
    }

    private void SpawnAlert()
    {
        StartCoroutine(AlertMessageFadeout(alertFadeoutTimer, alertBossSpawnText));
    }

    IEnumerator AlertMessageFadeout(float fadeoutTime, Text alertMessage)
    {
        alertMessage.gameObject.SetActive(true);
        Color alertColor = alertMessage.color;
        alertColor.a = 1;
        alertMessage.color = alertColor;
        yield return new WaitForSeconds(1);
        float fadeTimer = 0;
        while (alertMessage.color.a > 0)
        {
            fadeTimer += (Time.deltaTime / fadeoutTime);
            alertColor.a = Mathf.Lerp(1, 0, fadeTimer);
            Debug.Log(alertColor.a);
            alertMessage.color = alertColor;
            if (alertMessage.color.a <=0)
            {
                alertMessage.gameObject.SetActive(false);
            }
            yield return null;
        }
    }

    private Vector3 GetMostDistantSpawnPosition ()
    {
        Vector3 mostDistantPosition = transform.position;
        float greatestDistanceFound = 0;
        foreach (Transform spawnLocation in spawnLocations)
        {
            float distance = Vector3.Distance(spawnLocation.position, player.transform.position);
            if (distance > greatestDistanceFound)
            {
                greatestDistanceFound = distance;
                mostDistantPosition = spawnLocation.position;
            }
        }
        return mostDistantPosition;
    }
}
