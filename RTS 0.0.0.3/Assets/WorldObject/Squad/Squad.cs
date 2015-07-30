using UnityEngine;
using System.Collections.Generic;
using RTS;

public class Squad : WorldObject 
{
	// what direction the squad is facing
	public float squadRotation;

	// how far apart the units in a squad should stand
	float seperation = 8f;

	// hold the destinations of were everyone in the squad
	public Vector3[] destinations;
	public Transform[] dests; 

	// holds the units in the squad
	public Unit[] units;
	public Transform[] Tforms;

	/*** Game Engine methods, all can be overridden by subclass ***/

	protected override void Awake() 
	{
		base.Awake();
	}
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
	}
	
	protected override void Update () 
	{
		base.Update();
	}
	
	protected override void OnGUI() 
	{
		base.OnGUI();
	}

	public override void MouseClickSquad(Squad holdSquad, Vector3 hitPoint, Player player) 
	{
		base.MouseClickSquad(holdSquad, hitPoint, player);
		// not exactly sure what the above line of code does, if you find out comment what it does

		//only handle input if owned by a human player and currently selected
		if(player && player.human) // && currently Selected
								   // ^ took that out because I think its redundant
		{
			//Debug.Log( "2" );
			//Debug.Log ( holdSquad);
			//this if statement probably wont work because holdSquad.tag != Ground if it is suppose to. Not sure how fix this yet
			if(holdSquad.tag != "Ground" && hitPoint != ResourceManager.InvalidPosition) 
			{
				//Debug.Log( "3" );
				//units = need to call getunit somehow
				float x = hitPoint.x;
				//makes sure that the unit stays on top of the surface it is on
				//might be the cause of a bug where The destination points are too high up
				//might be able to just git rid of player.SelectedObject.transform.position.y and fix it
				//as of now i think its fine...
				float y = hitPoint.y + player.SelectedObject.transform.position.y;
				float z = hitPoint.z;

				//calculates what direction the squad should face
				squadRotation = SquadFormationManager.calculateSquadRotation(Tforms[0].position, new Vector3(x,y,z));
				//Debug.Log (squadRotation);

				//makes a vector3[] to assign all the units to
				destinations = SquadFormationManager.calculateSquadPositions(seperation,squadRotation, new Vector3(x,y,z));


				//Debug.Log( "units[0] ", units[0] );
				for(int i = 0; i < units.Length; i++)
				{
					//holdSquad.units[i].target.position = destinations[i];
					Tforms[i].position = destinations[i];
					Debug.Log( units[i] );
					Debug.Log( destinations[i] );
				}
			}
		}
	}
}