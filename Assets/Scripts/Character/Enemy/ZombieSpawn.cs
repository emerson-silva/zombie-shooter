using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawn : MonoBehaviour
{
    public GameObject zombie;
    public float spawnInterval;
    public float spawnRange;
    public float nextSpawnTime;
    public int maxZombiesAlive;
    public int initialZombiesAlive;
    private int zombiesAlive;
    private float zombieRadius;
    public LayerMask collisionLayer;
    private Player player;
    private bool isOutOfVision;

    public float DifficultyIncrementInterval = 15;
    private float nextDifficultyIncrementTime;

    void Start()
    {
        zombieRadius = zombie.GetComponent<CapsuleCollider>().radius;
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<Player>() ;

        if (Time.timeSinceLevelLoad >= nextSpawnTime)
        {
            for (int i = 0; i < initialZombiesAlive && CanInstantiateZumbie(); i++)
            {
                StartCoroutine(SpawnZumbie());
            }
            nextSpawnTime = spawnInterval;
        }
        nextDifficultyIncrementTime = DifficultyIncrementInterval;
    }

    void Update()
    {
        isOutOfVision = player.GetFieldOfVision() < Vector3.Distance(player.gameObject.transform.position, transform.position);

        if (Time.timeSinceLevelLoad >= nextSpawnTime && isOutOfVision && CanInstantiateZumbie()) 
        {
            nextSpawnTime = Time.timeSinceLevelLoad + spawnInterval;
            StartCoroutine(SpawnZumbie());
        }

        if (Time.timeSinceLevelLoad > nextDifficultyIncrementTime)
        {
            nextDifficultyIncrementTime += DifficultyIncrementInterval;
            maxZombiesAlive++;
        }
    }

    private Vector3 RandomSpawnPosition()
    {
        Vector3 position = transform.position + (Random.insideUnitSphere * spawnRange);
        position.y = transform.position.y;
        return position;
    }

    void OnDrawGizmos()
    {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, spawnRange);
    }

    IEnumerator SpawnZumbie()
    {
        Vector3 createInPosition = RandomSpawnPosition();
        Collider[] collidersInPosition = Physics.OverlapSphere(createInPosition, zombieRadius, collisionLayer);
        while (collidersInPosition.Length > 0 && CanInstantiateZumbie())
        {
            createInPosition = RandomSpawnPosition();
            Physics.OverlapSphere(createInPosition, zombieRadius, collisionLayer);
            yield return null;
        }
        SpawnZumbie(transform.position);
    }

    private void SpawnZumbie(Vector3 position)
    {
        if (CanInstantiateZumbie())
        {
            Zombie spawnedZombie = GameObject.Instantiate(zombie, position, this.transform.rotation).GetComponent<Zombie>();
            spawnedZombie.SetZombieSpawner(this);
            IncrementZumbiesAlive();
        }
    }

    private bool CanInstantiateZumbie()
    {
        return zombiesAlive < maxZombiesAlive;
    }

    private void IncrementZumbiesAlive()
    {
        zombiesAlive++;
    }

    private void DecrementZumbiesAlive()
    {
        if (zombiesAlive>0)
        {
            zombiesAlive--;
        }
    }

    public void NotifyZombieDeath()
    {
        DecrementZumbiesAlive();
    }
}
