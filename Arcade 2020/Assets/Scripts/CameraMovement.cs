using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Vector3 targetPosition;
    public bool movementDone = false;
    public bool moving = false;
    [SerializeField]float movementSpeed;

    Vector2 minPosition;
    Vector2 maxPosition;

    public void Awake()
    {
        targetPosition = transform.position;
    }
    public void LateUpdate()
    {
        if(moving)
        {
            Vector3 nextPosition = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
            if(transform.position != targetPosition)
            {
                nextPosition.x = Mathf.Clamp(nextPosition.x, minPosition.x, maxPosition.x);
                nextPosition.y = Mathf.Clamp(nextPosition.y, minPosition.y, maxPosition.y);
                transform.position = Vector3.Lerp(transform.position,nextPosition, movementSpeed * Time.deltaTime);
            }
            if(Mathf.RoundToInt(transform.position.x * 10) == (int)targetPosition.x * 10 && Mathf.RoundToInt(transform.position.y * 10) == (int)(targetPosition.y * 10))
            {
                transform.position = targetPosition;
                movementDone = true;
                moving = false;
            }
        }
    }
    public void Move(Vector2 directionModifier)
    {
        moving = true;
        targetPosition = new Vector3(transform.position.x + directionModifier.x * 20, transform.position.y + directionModifier.y * 20, transform.position.z);
        Debug.Log(targetPosition);
        minPosition.x = transform.position.x + directionModifier.x * 20;
        minPosition.y = transform.position.y + directionModifier.y * 20;
        maxPosition.x = transform.position.x + 20;
        maxPosition.y = transform.position.y + 20;
    }
}