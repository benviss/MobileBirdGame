using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{

    public Camera mainCamera;
    public float flapTime = .15f;
    public float flapPower = 100;
    public float restMass = 3;
    public float poopMass = 0;

    private Rigidbody2D rb;
    private BoxCollider col;
    private Material mat;
    private bool isFlapping = false;
    public float endFlapTime = 0;
    private bool isDead = false;
    private Vector3 flightVector = Vector3.zero;
    private float flipTime = .25f;
    private bool isMovingForward = true;
    private bool isFlipping = false;

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
            flightVector = CalculateAngleAxis();

            if (Input.GetKey("mouse 0"))
            {
                if (!isFlapping)
                {
                    StartFlap();
                }
                else if (Time.time < endFlapTime)
                {
                    Flap();
                }
            }

            else
            {
                isFlapping = false;
            }

            IsFlip();
        }
    }

    private void IsFlip()
    {
        if (!isFlipping)
        {
            if ((isMovingForward && rb.velocity.x < 0) || (!isMovingForward && rb.velocity.x > 0))
            {
                isFlipping = true;
                isMovingForward = !isMovingForward;
                StartCoroutine("iFlipSprite");
            }
        }
    }

    IEnumerator iFlipSprite()
    {
        float timer = 0;
        float startAngle = 0;
        if (isMovingForward)
        {
            startAngle += 180;
        }

        while (timer < flipTime)
        {
            timer += Time.deltaTime;
            float t = timer / flipTime;
            float angle = Mathf.Lerp(startAngle, startAngle + 180, t);

            transform.rotation = Quaternion.Euler(0, angle, 0);
            yield return null;
        }

        isFlipping = false;
        Vector3 v = transform.rotation.eulerAngles;
        v.y = startAngle + 180;
        transform.rotation = Quaternion.Euler(v);
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
        isFlapping = true;
        Flap();
    }

    private void Flap()
    {
        float downVelocity = Mathf.Clamp(rb.velocity.y, -25, 25) * -1;

        //Debug.Log("Speed: " + downVelocity);
        //Debug.Log("FlapPower: " + (downVelocity * 15 + flapPower));

        rb.AddForce(flightVector * ((flapPower + downVelocity * 12) / flapTime) * Time.deltaTime * 5);
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
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        float xDiff = screenPos.x - Input.mousePosition.x;
        float yDiff = screenPos.y - Input.mousePosition.y;
        float angle = 0;

        if (yDiff > 0)
        {
            yDiff *= -1;
            xDiff *= -1;
            angle += 180;
        }

        angle += Mathf.Atan(xDiff / yDiff) * (180 / 3.1415f);

        return Quaternion.AngleAxis(angle, Vector3.back) * Vector3.up;
    }
}
