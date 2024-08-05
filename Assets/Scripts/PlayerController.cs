using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Transform firePoint;
    [SerializeField]
    private GameObject bullet;

    [Header("Bullet Configuration")]
    [SerializeField]
    private float defaultBulletSpeed = 1.0f;
    [SerializeField]
    private float defaultBulletRadius = 0.75f;

    private bool isGrounded { get; set; }
    private bool isFacingRight { get; set; }
    private Camera cam;
    private bool firing;


    // Start is called before the first frame update
    void Start()
    {
        if (rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
        if (groundCheck == null)
        {
            groundCheck = transform.GetChild(1);
        }
        cam = Camera.main;
        firing = false;
    }

    private void Update()
    {
        isGrounded = groundCheckMethod();
        directionCheck();

    }

    private bool groundCheckMethod()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
    }

    //Check if mousePos matches current facing direction, if not change it
    private void directionCheck()
    {
        Vector2 mousePos = Input.mousePosition;
        Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        if ((point - transform.position).x >= 0 && isFacingRight)
        {
            changeDirection();
            isFacingRight = false;
        }
        else if ((point - transform.position).x < 0 && !isFacingRight)
        {
            changeDirection();
            isFacingRight = true;
        }
    }

    private void changeDirection()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    public void shootSpell(InputAction.CallbackContext context)
    {
        //if mouse press JUST occured
        if (!context.canceled && !firing)
        {
            Debug.Log("HIT");
            Vector3 direction;
            Vector2 mousePos = Input.mousePosition;
            Vector3 point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));

            direction = point - transform.position;
            direction = direction.normalized;

            GameObject spell = GameObject.Instantiate(bullet);
            spell.transform.position = firePoint.position;
            //set bullet params
            spell.GetComponent<PlayerSpell>().setDirection(direction);
            spell.GetComponent<PlayerSpell>().setTravelSpeed(defaultBulletSpeed);
            spell.GetComponent<PlayerSpell>().setBlastRadius(defaultBulletRadius);
            StartCoroutine(fireCooldown());
        }
    }

    private IEnumerator fireCooldown()
    {
        firing = true;
        yield return new WaitForFixedUpdate();
        firing = false;
    }
}
