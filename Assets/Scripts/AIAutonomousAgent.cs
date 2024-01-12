using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AIAutonomousAgent : AIAgent {

	public AIPerception seekPerception = null;
	public AIPerception fleePerception = null;
	public AIPerception flockPerception = null;

	private void Update() {

		if ( seekPerception ) {

			var gameObjects = seekPerception.GetGameObjects();
			if ( gameObjects.Length > 0 ) {

				movement.ApplyForce( Seek( gameObjects[0] ) );

			}

		}

		if ( fleePerception ) {

			var gameObjects = fleePerception.GetGameObjects();
			if ( gameObjects.Length > 0 ) {

				movement.ApplyForce( Flee( gameObjects[0] ) );

			}

		}

		if ( flockPerception ) {

			var gameObjects = flockPerception.GetGameObjects();
			if ( gameObjects.Length > 0 ) {

				movement.ApplyForce( Cohesion( gameObjects ) );
				movement.ApplyForce( Alignment( gameObjects ) );
				movement.ApplyForce( seperation( gameObjects, 3 ) );

			}

		}

		transform.position = Utilities.Wrap( transform.position, new Vector3( -10, -10, -10 ), new Vector3( 10, 10, 10 ) );


	}

	private Vector3 Seek( GameObject target ) {

		Vector3 direction = target.transform.position - transform.position;
		return GetSteeringForce( direction );

	}

	private Vector3 Flee( GameObject target ) {

		Vector3 direction = transform.position - target.transform.position;
		return GetSteeringForce( direction );

	}

	private Vector3 Cohesion( GameObject[] neighbors ) {

		Vector3 positions = Vector3.zero;
		foreach ( var neighbor in neighbors ) {

			positions += neighbor.transform.position;

		}

		Vector3 center = positions / neighbors.Length;

		return GetSteeringForce( center - transform.position );

	}

	private Vector3 seperation( GameObject[] neighbors, float radius ) {

		Vector3 seperation = Vector3.zero;
		foreach ( var neighbor in neighbors ) {

			Vector3 direction = ( transform.position - neighbor.transform.position );
			if ( direction.sqrMagnitude < radius ) {

				seperation += direction / direction.sqrMagnitude;

			}

		}

		return GetSteeringForce( seperation );

	}

	private Vector3 Alignment( GameObject[] neighbors ) {

		Vector3 velocities = Vector3.zero;
		foreach ( var neighbor in neighbors ) {

			velocities += neighbor.GetComponent<AIAgent>().movement.Velocity;

		}

		return GetSteeringForce( velocities / neighbors.Length );

	}

	private Vector3 GetSteeringForce( Vector3 direction ) {

		Vector3 desired = direction.normalized * movement.maxSpeed;
		Vector3 steer = desired - movement.Velocity;
		Vector3 force = Vector3.ClampMagnitude( steer, movement.maxForce );

		return force;

	}

}
