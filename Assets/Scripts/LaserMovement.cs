using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    public Transform[] Waypoints;
    public Movement MovementType;
    public byte Speed;

    private bool isReturning;
    private int currentWaypointIndex;

    public enum Movement
    {
        stop,
        restart,
        pingpong
    }

    // Start is called before the first frame update
    void Start()
    {
        currentWaypointIndex = 0;
        isReturning = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        //No waypoints
        if (Waypoints.Length == 0)
        {
            return;
        }

        //Max reached
        if (currentWaypointIndex == Waypoints.Length)
        {
            switch (MovementType)
            {
                case Movement.stop:
                    return;
                case Movement.restart:
                    currentWaypointIndex = 0;
                    break;
                case Movement.pingpong:
                    currentWaypointIndex--;
                    isReturning = true;
                    break;
            }
        }

        //Min reached (only when MovementType == Movement.pingpong)
        else if (currentWaypointIndex == -1)
        {
            currentWaypointIndex++;
            isReturning = false;
        }

        Transform waypoint = Waypoints[currentWaypointIndex];

        //If position is (nearly) on waypoint position, select next position (and put wall on position)
        if (Vector3.Distance(transform.position, waypoint.position) == 0)
        {
            transform.position = waypoint.position;

            if (isReturning)
            {
                currentWaypointIndex--;
            }
            else
            {
                currentWaypointIndex++;
            }

            Move();
        }

        //Move closer to waypoint position
        Vector3 newPosition = Vector3.MoveTowards(transform.position, waypoint.position, (float) Speed / 500);
        transform.position = newPosition;
    }
}
