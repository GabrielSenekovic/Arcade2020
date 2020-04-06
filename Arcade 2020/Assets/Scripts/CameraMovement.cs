using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] Team team;

    void Update()
    {
        if(!team.GetIfBothTouchingDoor())
        {
            return;
        }
    }

    void Move()
    {

    }
}