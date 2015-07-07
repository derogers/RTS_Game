using UnityEngine;
using System.Collections.Generic;
using RTS;

public class Squad : WorldObject 
{
	// what direction the squad is facing
	float squadRotation;

	// how far apart the units in a squad should stand
	float seperation = 8f;

	// hold the destinations of were everyone in the squad
	Vector3[] destinations;

	// holds the units in the squad
	Unit[] units;

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
		//base.MouseClick(holdSquad, hitPoint, player);
		// not exactly sure what the above line of code does, if you find out comment what it does

		//only handle input if owned by a human player and currently selected
		if(player && player.human && currentlySelected) 
		{
			if(holdSquad.tag == "Ground" && hitPoint != ResourceManager.InvalidPosition) 
			{
				float x = hitPoint.x;
				//makes sure that the unit stays on top of the surface it is on
				float y = hitPoint.y + player.SelectedObject.transform.position.y;
				float z = hitPoint.z;
				//calculates what direction the squad should face
				squadRotation = SquadFormationManager.calculateSquadRotation(transform.position, new Vector3(x,y,z));
				//makes a vector3[] to assign all the units to
				destinations = SquadFormationManager.calculateSquadPositions(seperation,squadRotation, new Vector3(x,y,z));
				Debug.Log( "units[0] ", units[0] );
				for(int i = 0; i < units.Length; i++)
				{
					holdSquad.units[i].target.position = destinations[i];
					Debug.Log( "units[i] ", units[i] );
				}
			}
		}
	}

}