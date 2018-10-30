using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{

    public float flapTime = .15f;
    public float flapPower = 100;
    public float glidePercentage = .6f;
    public float dragCof = .001f;
    public float restMass = 3;
    public float poopMass = 0;

    private Rigidbody2D rb;
    private BoxCollider col;
    private Material mat;
    private float endFlapTime = 0;
    private bool isDead = false;
    private float energyPerStep = 0f;
    private float energyExpendedStep = 0f;
    private float expendModifier = .002f;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider>();
        mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isDead)
        {
            transform.up = CalculateAngleAxis();

            if (Input.GetKey("mouse 0"))
            {
                if (Input.GetKeyDown("mouse 0"))
                {
                    StartFlap();
                }
                else if (Time.time < endFlapTime)
                {
                    Flap();
                }
            }
        }

    }
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Bug")
        {
            ConsumeBug(collision.gameObject);
        }
    }

    private void ConsumeBug(GameObject bug)
    {
        if (!isDead)
        {
            // bug.eaten();
            // bird.mass += bug.mass();
            Destroy(bug);
            poopMass += .3f;
        }
    }
    
    // doing our own drag model helps control what speed the player can fly at (not linear scaling - fast == no)
    private void StartFlap()
    {
        endFlapTime = Time.time + flapTime;
        Flap();
    }

    private void Flap()
    {
        float downVelocity = Mathf.Clamp(rb.velocity.y, -25, 25) * -1;
        Vector3 angle = CalculateAngleAxis();

        //Debug.Log("Speed: " + downVelocity);
        //Debug.Log("FlapPower: " + (downVelocity * 15 + flapPower));

        rb.AddForce(angle * ((flapPower + downVelocity * 12) / flapTime) * Time.deltaTime * 5);
        // TODO animate flap

    }

    private void Glide()
    {
        // Calculates counteractive force of gravity - glidePercentage of 1 should be zero grav
        float scalar = (float) (Time.deltaTime * 9.81f * 59.85 * rb.mass * glidePercentage);
        //Debug.Log("glidePower: " + scalar);

        rb.AddForce(Vector3.up * scalar);
    }


    private Vector3 CalculateAngleAxis()
    {
        float t = (Input.mousePosition.x / Screen.width);
        t -= .5f;
        t *= 1.75f;
        t += .5f;
        float minAngle = -35;
        float maxAngle = 45;
        float angle = Mathf.Lerp(minAngle, maxAngle, t);

        //Debug.Log("pos: " + xPos);
        //Debug.Log("width: " + Screen.width);
        //Debug.Log("angle: " + angle);

        return Quaternion.AngleAxis(angle, Vector3.back) * Vector3.up;
    }
}
