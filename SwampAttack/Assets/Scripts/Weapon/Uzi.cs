using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Uzi : Weapon
{
    public override void Shoot(Transform shootPoint)
    {
        float scatter = 0.4f;
        Vector3 angle = shootPoint.eulerAngles;
        angle.z += Random.Range(-scatter, scatter);
        var moveCoordinateX = -1;
        var movementVector = new Vector2(moveCoordinateX, angle.z);
        Bullet.SetBulletVector(movementVector);
        Instantiate(Bullet, shootPoint.position, Quaternion.Euler(angle));
    }
}
