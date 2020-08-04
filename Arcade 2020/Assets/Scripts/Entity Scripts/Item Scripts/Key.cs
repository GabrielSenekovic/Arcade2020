using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : PickUp
{
    public Transform[] sprites;
    [SerializeField] float rotationspeed;
    float currentRotation;
    public override void OnPickUp(GameObject otherObject)
    {
        otherObject.gameObject.GetComponentInParent<Team>().AddKey();
        Destroy(gameObject);
    }
    private void FixedUpdate()
    {
        currentRotation+=rotationspeed;
        sprites[1].rotation = Quaternion.Euler(sprites[1].rotation.x, sprites[1].rotation.y, currentRotation);
        sprites[0].rotation = Quaternion.Euler(sprites[0].rotation.x, currentRotation, sprites[0].rotation.z);
        currentRotation %= 361;
    }
}
