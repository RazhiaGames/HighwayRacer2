//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2024 BoneCracker Games
// http://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;

/// <summary>
/// Limits the linear right and left velocity of the player vehicle when it hits the trigger (barrier).
/// </summary>
public class HR_BarrierCollisionProtector : MonoBehaviour {

    /// <summary>
    /// Enum for the side of the collision.
    /// </summary>
    public CollisionSide collisionSide = CollisionSide.Left;
    public enum CollisionSide { Left, Right }

    /// <summary>
    /// Called when another collider stays within the trigger.
    /// </summary>
    /// <param name="col">The collider that stays within the trigger.</param>
    private void OnTriggerStay(Collider col) {

        RCCP_CarController carController = col.transform.GetComponentInParent<RCCP_CarController>();

        if (!carController)
            return;

        Rigidbody playerRigid = carController.Rigid;

        // Limit the linear and angular velocity of the player vehicle
        playerRigid.linearVelocity = new Vector3(0f, playerRigid.linearVelocity.y, playerRigid.linearVelocity.z);
        playerRigid.angularVelocity = new Vector3(playerRigid.angularVelocity.x, 0f, 0f);

        // Apply force to the player vehicle based on the collision side
        if (collisionSide == CollisionSide.Right)
            playerRigid.AddForce(-Vector3.right * 1f, ForceMode.VelocityChange);
        else
            playerRigid.AddForce(Vector3.right * 1f, ForceMode.VelocityChange);

    }

    /// <summary>
    /// Draws gizmos in the editor.
    /// </summary>
    private void OnDrawGizmos() {

        Gizmos.color = new Color(1f, .5f, 0f, .75f);
        Gizmos.DrawCube(transform.position, GetComponent<BoxCollider>().size);

    }

}
