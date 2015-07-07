using UnityEngine;
using System.Collections.Generic;

namespace RTS{
	// used to calculate where units should stand/face when given an order
	public static class SquadFormationManager 
	{
		//calcuate where everyone should stand in a squad
		public static Vector3[] calculateSquadPositions(float seperation, float rotationY, Vector3 destination)
		{
			Vector3[] positionArray = new Vector3[6];
			//first person goes to where the player clicked
			positionArray [0] = new Vector3 (destination.x,destination.y,destination.z);
			//If left of known position
			positionArray [1] = new Vector3 (destination.x - seperation * Mathf.Cos (rotationY), destination.y, destination.z + seperation * Mathf.Sin (rotationY));
			//if right of known position
			positionArray [2] = new Vector3 (destination.x + seperation * Mathf.Cos (rotationY), destination.y, destination.z - seperation * Mathf.Sin (rotationY));
			//if below known position
			positionArray [3] = new Vector3 (destination.x - seperation * Mathf.Sin (rotationY), destination.y, destination.z - seperation * Mathf.Cos (rotationY));
			//next 2 are also examples of "if below known position"
			positionArray [4] = new Vector3 (positionArray [1].x - seperation * Mathf.Sin (rotationY), destination.y, positionArray [1].z - seperation * Mathf.Cos (rotationY));
			positionArray [5] = new Vector3 (positionArray [2].x - seperation * Mathf.Sin (rotationY), destination.y, positionArray [2].z - seperation * Mathf.Cos (rotationY));
			//return the Vector3 we just made im pretty sure
			return new Vector3[6];
		}
		// calcuate what direction the squad should face
		public static float calculateSquadRotation(Vector3 currentPos, Vector3 destinationPos)
		{
			float rotation;
			// theta = tan^-1( (finalX-currentX) / (finalZ-currentZ) )
			rotation = Mathf.Atan ((destinationPos.x - currentPos.x) / (destinationPos.z - currentPos.z));
			return rotation;
		}
	}
}