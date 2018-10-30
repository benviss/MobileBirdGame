using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour {

    public Transform player;
    public float baseUpdateSpeed = 10;

	void FixedUpdate ()
    {
        float offsetX = (player.position.x - transform.position.x) / 2;
        float offsetY = (player.position.y - transform.position.y) / 2;
        float updateSpeed = baseUpdateSpeed;
        float diff = Mathf.Abs(offsetX) + Mathf.Abs(offsetY);

        if (diff > 1)
        {
            updateSpeed /= Mathf.Pow(diff, .1f);
        }

        transform.position = new Vector3(offsetX / updateSpeed + transform.position.x, offsetY / updateSpeed + transform.position.y, transform.position.z);
    }
}
