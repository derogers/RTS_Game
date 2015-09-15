using UnityEngine;
using System.Collections.Generic;

// This is used to do the Calculations for the squad formation movement

namespace RTS
{
	// Used to calculate where units should stand/face when given an order
	public static class SquadFormationManager 
	{
		/*
		 * Calcuate where everyone should stand in a squad
		 * Takes in the variables:
		 * seperation : how far apart the units should stand from eachother
		 * rotationY : what direction the squad is facing (The squad lines up perpindicular to the direction the squad is facing)
		 * destination : where the squad wants to move (where the player selected them to move usually)
		 * sMovingUp : If the squad is moving up or down ( True is moving up, False is moving down)
		*/ 
		public static Vector3[] calculateSquadPositions(float seperation, float rotationY, Vector3 destination, bool sMovingUp)
		{
			Vector3[] positionArray = new Vector3[6];
			// First person goes to where the player clicked
			positionArray[0] = new Vector3 (destination.x,destination.y,destination.z);
			// If left of known position (perpindicular to the direction they are facing)
			positionArray[1] = new Vector3 (destination.x - seperation * Mathf.Cos (rotationY), destination.y, destination.z + seperation * Mathf.Sin (rotationY));
			// If right of known position (perpindicular to the direction they are facing)
			positionArray[2] = new Vector3 (destination.x + seperation * Mathf.Cos (rotationY), destination.y, destination.z - seperation * Mathf.Sin (rotationY));

			if (sMovingUp == true) // If moving up (new z coor > old z coor), line up behind (negative z direction)
			{
				positionArray [3] = new Vector3 (destination.x - seperation * Mathf.Sin (rotationY), destination.y, destination.z - seperation * Mathf.Cos (rotationY));
				// Need to keep adding here when a new row is nessary
			} 
			else // If moving down (new z coor < old z coor)), line up in front (possitive z direction)
			{
				positionArray[3] = new Vector3 (destination.x + seperation * Mathf.Sin (rotationY), destination.y, destination.z + seperation * Mathf.Cos (rotationY));
				// Need to keep adding here when a new row is nessary
			}
			// Next 2 positions are to the left and right of positionArray[3]
			positionArray[4] = new Vector3 (positionArray [3].x - seperation * Mathf.Cos (rotationY), destination.y, positionArray [3].z + seperation * Mathf.Sin (rotationY));
			positionArray[5] = new Vector3 (positionArray [3].x + seperation * Mathf.Cos (rotationY), destination.y, positionArray [3].z - seperation * Mathf.Sin (rotationY));
			// Returns the Vector3 that was made
			return positionArray;
		}
		// Calcuate what direction the squad should face
		public static float calculateSquadRotation(Vector3 currentPos, Vector3 destinationPos)
		{
			float rotation;
			// theta = tan^-1( (finalX-currentX) / (finalZ-currentZ) )
			rotation = Mathf.Atan ((destinationPos.x - currentPos.x) / (destinationPos.z - currentPos.z));
			return rotation;
		}

		// Calcuate if the squad is moving up or down
		// This is nessary to calculate the positions vector
		public static bool squadMovingUp(Vector3 currentPos, Vector3 destinationPos)
		{
			// If the z coordinate of the destination position is greater than the current position, then the squad is moving up
			if (destinationPos.z > currentPos.z) 
			{
				return true;
			} 
			else
			{
				return false;
			}
		}
	}
}