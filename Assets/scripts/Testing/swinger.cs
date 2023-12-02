using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class swinger : MonoBehaviour
{
    public Transform swingAnchor;

    public float hSpeed;

    public float pos;

    public float accelleration;

    public float followEasing;

    public float gravityForce;

    public float minAngle, maxAngle, swingAngle;

    public Vector3 axis, lookDirection;

    public Quaternion desiredRotation;

    void Start()
    {
        
    }

    void Update()
    {
        #region calculate progression from input
        float h = Input.GetAxis("Horizontal");

        if(h != 0)
        {
            hSpeed += accelleration * h;

            hSpeed = Mathf.Clamp(hSpeed, -1f, 1f);

            pos += hSpeed * Time.deltaTime;

            //pos -= gravityForce;
        }
        else
        {
            pos = Mathf.Lerp(pos, 0.5f, gravityForce);
        }


        pos = Mathf.Clamp(pos, 0, 1);
        #endregion

        #region Handle the swinging rotation
        // Calculate the normalized time parameter for the lerp
        float t = (h + 1.0f) / 2.0f;// (Mathf.Sin(Time.time * hSpeed) + 1.0f) / 2.0f;

        //use a lerp to determine the current angle of the swing
        swingAngle = Mathf.Lerp(minAngle, maxAngle, pos);

        //use the swing angle to create a rotation in euler angles
        lookDirection = axis * swingAngle;
        desiredRotation = Quaternion.Euler(-lookDirection);

        //rotate the the swing object.
        transform.rotation = desiredRotation;

        #endregion

        #region Move the anchor position

        //project a position bellow the facing direction of the swinger
        Vector3 followPos = transform.position + (-transform.up * 2);

        //lerp the position of the swinging object to follow the anchor
        swingAnchor.position = Vector3.Lerp(swingAnchor.position, followPos, followEasing);

        #endregion
    }

    //visually process the angles of the swingpoint
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Quaternion lineRot = Quaternion.Euler(axis * minAngle);

        Gizmos.DrawRay(transform.position, lineRot * -Vector3.up);
        Gizmos.color = Color.blue;
        Quaternion otherLineRot = Quaternion.Euler(axis * maxAngle);
        Gizmos.DrawRay(transform.position, otherLineRot * -Vector3.up);
    }
}
