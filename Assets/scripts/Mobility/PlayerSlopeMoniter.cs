using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlopeMoniter : MonoBehaviour
{
    public enum Direction { up, down, forward }
    public Direction currentDirection;

    Vector3 lastPosition, currentPosition;

    public float dot;

    public PlayerMovement.SpeedMods slopeMod = new PlayerMovement.SpeedMods();
    public PlayerMovement playerscript;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMovement.modifiers.Add(slopeMod);
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = transform.position;
    }
    private void LateUpdate()
    {
        Vector3 dir = currentPosition - lastPosition;

        dir.Normalize();

         dot = Vector3.Dot(Vector3.up, dir);

        if (dot == 0)
        {
            currentDirection = Direction.forward;
            slopeMod.maxAccellerationMod = 0;
            slopeMod.maxDecelleration = 0;
            slopeMod.maxSpeedMod = 0;
        }
        else if (dot > 0)
        {
            currentDirection = Direction.up;
            slopeMod.maxAccellerationMod = -0.25f;
            slopeMod.maxDecelleration = 0.1f;
            slopeMod.maxSpeedMod = -5;
        }
        else
        {
            currentDirection = Direction.down;
            slopeMod.maxAccellerationMod = 0.15f;
            slopeMod.maxDecelleration = -0.5f;
            slopeMod.maxSpeedMod = 3f;
        }

        lastPosition = currentPosition;
    }
}
