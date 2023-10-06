using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour, IKillable
{
    private const float FADE_AWAY_TIME = 2f;

    private Status status;
    private CharacterAnimation bossAnimation;
    private CharacterMovement bossMovement;
    private Transform player;
    private NavMeshAgent agent;
    [SerializeField]
    private List<GameObject> itemsDroppedOnDeath;
    [SerializeField]
    private GameObject[] randomGunsToDrop;
    [SerializeField]
    private Slider lifeDisplay;
    public Image lifeSliderImage;
    public Color maxLifeColor;
    public Color minLifeColor;
    public GameObject bloodParticle;

    void Start()
    {
        bossAnimation = GetComponent<CharacterAnimation>();
        bossMovement = GetComponent<CharacterMovement>();
        status = GetComponent<Status>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
        agent.speed = status.RunSpeed;
        RandomizeBoss();
        if (itemsDroppedOnDeath==null)
        {
            itemsDroppedOnDeath = new List<GameObject>();
        }
        AddRandomGunToDrops();
        lifeDisplay.maxValue = status.MaxLife;
        UpdateLifeDisplay();
    }

    private void RandomizeBoss()
    {
        int randomZombie = Random.Range(1, transform.childCount);
        transform.GetChild(randomZombie).gameObject.SetActive(true);
    }

    void Update()
    {
        agent.SetDestination(player.position);
        bossAnimation.MoveAnimation(agent.velocity.magnitude);
        TryToAttack();
    }

    private bool IsNearPlayer()
    {
        if (agent.hasPath) {
            return agent.remainingDistance <= agent.stoppingDistance;
        } else
        {
            return Vector3.Distance(transform.position, player.position) <= status.MeleeDistance;
        }
    }

    private void TryToAttack()
    {
        if (IsNearPlayer())
        {
            if (status.IsAbleToAttack())
            {
                bossMovement.LookAt(player.position - transform.position);
                bossAnimation.MeleeAttack();
            }
        }
    }

    public void AttackHit ()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, transform.forward, out rayHit, status.MeleeDistance))
        {
            if (rayHit.transform.gameObject.CompareTag(Tags.PLAYER))
            {
                player.gameObject.GetComponent<Player>().TakeDamage(status.BaseDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        status.TakeDamage(damage);
        UpdateLifeDisplay();
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
        DropItems();
        bossAnimation.Death();
        bossMovement.DisableCollisions();
        agent.enabled = false;
        Destroy(gameObject, FADE_AWAY_TIME);
        this.enabled = false;
    }

    private void DropItems()
    {
        foreach(GameObject item in itemsDroppedOnDeath)
        {
            Vector3 randomDropPosition = transform.position + Random.insideUnitSphere * 1.2f;
            randomDropPosition.y = transform.position.y;
            GameObject.Instantiate(item, randomDropPosition, transform.rotation);
        }
    }

    private void AddRandomGunToDrops()
    {
        GameObject droppedGun = (GameObject) randomGunsToDrop.GetValue(Random.Range(0, randomGunsToDrop.Length));
        itemsDroppedOnDeath.Add(droppedGun);
    }

    private void UpdateLifeDisplay ()
    {
        lifeDisplay.value = status.GetLife();
        Debug.Log(status.GetLife() * 1.0f / status.MaxLife);
        Color lifeColor = Color.Lerp(minLifeColor, maxLifeColor, status.GetLife() * 1.0f / status.MaxLife);
        lifeSliderImage.color = lifeColor;
    }
}
