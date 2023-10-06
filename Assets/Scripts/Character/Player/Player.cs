using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKillable
{
    [SerializeField]
    private LayerMask groundMask;
    private Status status;
    [SerializeField]
    private Gun activeGun;
    public ControlUI canvas;
    public AudioClip hurtSound;
    public GameObject PlayerWepons;
    private CharacterMovement characterMovement;
    private CharacterAnimation playerAnimation;
    public GameObject bloodParticle;

    private void Awake()
    {
        characterMovement = gameObject.GetComponent<CharacterMovement>();
        playerAnimation = gameObject.GetComponent<CharacterAnimation>();
        status = gameObject.GetComponent<Status>();
    }

    void Start()
    {
        activeGun = PlayerWepons.GetComponentInChildren<Gun>();
        Time.timeScale = 1;
    }

    void FixedUpdate()
    {
        if (IsAlive())
        {
            float horizontalAxis = Input.GetAxis(InputConst.HORIZONTAL_AXIS);
            float verticalAxis = Input.GetAxis(InputConst.VERTICAL_AXIS);

            MoveTo(new Vector3(horizontalAxis, 0, verticalAxis));

            // if Gun shootType is Continuous keep shooting while button is pressed down, otherwise shoot only once by click
            if ((ShootType.CONTINUOUS.Equals(activeGun.ShootType) && Input.GetButton(InputConst.FIRE)) ||
                    (ShootType.TRIGGER.Equals(activeGun.ShootType) && Input.GetButtonDown(InputConst.FIRE)))
            {
                this.activeGun.Shoot();
            }
        }
    }

    public float GetFieldOfVision()
    {
        return status.FieldOfVision;
    }

    private void MoveTo(Vector3 direction)
    {
        characterMovement.MoveTo(direction, status.RunSpeed);
        playerAnimation.MoveAnimation(direction.x != 0 || direction.z != 0);

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        // Debug.DrawRay(mouseRay.origin, mouseRay.direction * 40, Color.red);
        RaycastHit rayHit;

        if (Physics.Raycast(mouseRay, out rayHit, 40, groundMask))
        {
            Vector3 aim = rayHit.point - transform.position;
            aim.y = 0;
            characterMovement.LookAt(aim);
        }
    }

    public void TakeDamage (float damage)
    {
        status.TakeDamage(damage);
        AudioControl.asInstance.PlayOneShot(hurtSound);
        canvas.UpdateLifeDisplay();
        if (!IsAlive())
        {
            Death();
        }
    }

    public void ShowBlood(Quaternion rotation)
    {
        Instantiate(bloodParticle, transform.position, rotation);
    }

    public void Heal (float amount)
    {
        status.Heal(amount);
        canvas.UpdateLifeDisplay();
    }

    public float GetLife()
    {
        return status.GetLife();
    }

    public bool IsAlive()
    {
        return status.IsAlive();
    }

    public void Death ()
    {
        status.TakeDamage(status.MaxLife);
        GameManager.Instance.PauseGame();
        canvas.ShowPauseBackground();
        canvas.ShowDeathPanel();
    }

    public void PickUpGun (GunType pickedGunType)
    {
        GameObject actualGun = activeGun.gameObject;
        GameObject pickedGun = PlayerWepons.transform.GetChild(pickedGunType.GetHashCode()).gameObject;
        pickedGun.SetActive(true);
        activeGun = pickedGun.GetComponent<Gun>();
        actualGun.SetActive(false);
    }
}
