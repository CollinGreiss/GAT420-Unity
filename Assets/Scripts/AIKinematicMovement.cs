using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AIKinematicMovement : AIMovement {

    public override void ApplyForce( Vector3 force ) {
    
        Acceleration += force;
    
    }

    public override void MoveTowards( Vector3 target ) {

        Vector3 direction = ( target - transform.position ).normalized;
        ApplyForce( direction * maxForce );

    }

    public override void Stop() {

        Velocity = Vector3.zero;

    }

    public override void Resume() {
        //
    }

    private void LateUpdate() {

        Velocity += Acceleration * Time.deltaTime;
        //Velocity = Velocity.ClampMagnitude( minSpeed, maxSpeed );
        Velocity = Vector3.ClampMagnitude( Velocity, maxSpeed );
        transform.position += Velocity * Time.deltaTime;

        if ( Velocity.sqrMagnitude > 0.1f ) {

            transform.rotation = Quaternion.LookRotation( Velocity );

        }

        Acceleration = Vector3.zero;

    }

}
