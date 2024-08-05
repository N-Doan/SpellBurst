using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpell : MonoBehaviour
{
    [SerializeField]
    private float travelSpeed;
    public void setTravelSpeed(float val)
    {
        travelSpeed = val;
    }
    private Vector3 direction;

    public void setDirection(Vector3 val)
    {
        direction = val;
    }

    [SerializeField]
    private float maxBlastRadius;
    public void setBlastRadius(float val)
    {
        maxBlastRadius = val;
    }
    [SerializeField]
    private Rigidbody2D rb;
    // Start is called before the first frame update

    public PlayerSpell(float speed, Vector3 dir)
    {
        travelSpeed = speed;
        direction = dir;
    }
    private void Start()
    {
        if(rb == null)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.MovePosition(transform.position + direction * travelSpeed * Time.fixedDeltaTime);
    }
}
