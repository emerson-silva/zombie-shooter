using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour, IKillable
{
    private ControlUI controlUI;
    private CharacterMovement characterMovement;
    private CharacterAnimation zombieAnimation;
    private Status status;
    private Player player;
    public AudioClip zombieDeathSound;
    public GameObject bloodParticle;

    [SerializeField]
    private Vector3 destiny;
    [SerializeField]
    private Vector3 movementDirection;
    public float DelayToChangeDestiny;
    private float changeDirectionTimer;
    public float DelayToWander;
    private float waitToWander;

    // Find a way to use Dictionaries in the Inspector
    // public Dictionary<GameObject, float> itemsDropRate;
    public float DropRate;
    public GameObject DroppedItem;

    private ZombieSpawn zombieSpawner;

    private const float FADE_AWAY_TIME = 1.4f;
    private const bool IS_MOVING = true;

    private void Start()
    {
        controlUI = GameObject.FindObjectOfType<ControlUI>();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).GetComponent<Player>();
        characterMovement = gameObject.GetComponent<CharacterMovement>();
        zombieAnimation = GetComponent<CharacterAnimation>();
        status = gameObject.GetComponent<Status>();
        destiny = this.transform.position;
        RandomizeZombie();
    }

    void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        if (distance > status.FieldOfVision)
        {
            Wander();

        } else if (distance > status.MeleeDistance)
        {
            ChasePlayer();
        } else
        {
            Attack();
        }
    }

    private void MoveTo(Vector3 direction, float speed)
    {
        characterMovement.MoveTo(direction, speed);
        characterMovement.LookAt(direction);
        zombieAnimation.MoveAnimation(direction.x != 0 || direction.z != 0);
    }

    private void RandomizeZombie()
    {
        int randomZombie = Random.Range(1, transform.childCount);
        transform.GetChild(randomZombie).gameObject.SetActive(true);
    }

    private void ChasePlayer()
    {
        movementDirection = (player.GetComponent<Rigidbody>().position - transform.position).normalized;
        MoveTo(movementDirection, status.RunSpeed);
    }

    private void Wander()
    {
        if ( Vector3.Distance(destiny, transform.position)<= 0.1f || changeDirectionTimer <= 0)
        {
            zombieAnimation.MoveAnimation(!IS_MOVING);
            if (waitToWander<=0)
            {
                GenerateRandomDestiny();
                changeDirectionTimer = DelayToChangeDestiny;
                waitToWander = DelayToChangeDestiny;
            } else
            {
                waitToWander -= Time.deltaTime;
            }
        } else
        {
            changeDirectionTimer -= Time.deltaTime;
            movementDirection = (destiny - transform.position).normalized;
            MoveTo(movementDirection, status.WalkSpeed);
            zombieAnimation.MoveAnimation(IS_MOVING);
        }
    }

    private void GenerateRandomDestiny()
    {
        destiny = transform.position + (Random.insideUnitSphere * status.FieldOfVision);
        destiny.y = this.transform.position.y;
    }

    private void Attack()
    {
        if (status.IsAlive())
        {
            movementDirection = (player.GetComponent<Rigidbody>().position - transform.position).normalized;
            characterMovement.LookAt(movementDirection);
            zombieAnimation.MeleeAttack();
        }
    }

    // Referencied by animation event
    public void AttackHit ()
    {
        if (Vector3.Distance(transform.position, player.GetComponent<Rigidbody>().position) < (status.MeleeDistance + 1)) {
            player.TakeDamage(status.BaseDamage);
        }
    }

    public void TakeDamage(float damage)
    {
        status.TakeDamage(damage);
        if (!status.IsAlive())
        {
            Death();
        }
    }

    public void ShowBlood(Quaternion rotation)
    {
        Instantiate(bloodParticle, transform.position, rotation);
    }

    public void Death()
    {
        AudioControl.asInstance.PlayOneShot(zombieDeathSound);
        controlUI.IncrementZombiesKilled();
        DropItems();
        zombieAnimation.Death();
        if (GetZombieSpawner()!=null) {
            GetZombieSpawner().NotifyZombieDeath();
        }
        Destroy(gameObject, FADE_AWAY_TIME);
        DisableScripts();
    }

    void DropItems()
    {
        if (ShouldDropItem(DropRate))
        {
            DropItem(DroppedItem);
        }
        //foreach (KeyValuePair<GameObject, float> itemEntry in itemsDropRate)
        //{
        //    if (ShouldDropItem(itemEntry.Value))
        //    {
        //        DropItem(itemEntry.Key);
        //    }
        //}
    }

    bool ShouldDropItem (float chanceRate)
    {
        return Random.Range(0f, 1f) <= chanceRate;
    }

    void DropItem (GameObject item)
    {
        GameObject.Instantiate(item, transform.position, transform.rotation);
    }

    public ZombieSpawn GetZombieSpawner()
    {
        return this.zombieSpawner;
    }

    public void SetZombieSpawner(ZombieSpawn zombieSpawner) {
        this.zombieSpawner = zombieSpawner;
    }

    public void DisableScripts()
    {
        characterMovement.DisableCollisions();
        this.enabled = false;
    }
}
