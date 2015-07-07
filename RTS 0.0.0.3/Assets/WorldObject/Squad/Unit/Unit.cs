using UnityEngine;
using System.Collections.Generic;
using RTS;

public class Unit : WorldObject 
{
	// allows for minipulation of the units nav mesh which is attacted to every unit
	NavMeshAgent agent;

	// holds where the unit is suppose to be
	public Transform target;

	// how far apart and which way the squad is facing
	// should be handled in squad.cs
	public float seperation;
	public float squadRotation;

	// this should all be held here. basic stats of a unit.
	// will probably need to add a lot more stuff.
	public int cost = 20;
	public int minSquadSize = 5, maxSquadSize = 10;
	public int speed = 10;
	public int attackSpeed = 1, weaponsSkill = 4,evasiveness = 3;
	public int ballisticSkill = 4;
	public int strength = 4, toughness = 4, armour = 4;
	public int currentWounds = 2, maxWounds = 2;
	public int courage = 6, discipline = 6, terror = 3;
	public int spellCasting = 0, spellDefense = 4;

	public Squad squad;

	/*** Game Engine methods, all can be overridden by subclass ***/
	
	protected override void Awake() 
	{
		base.Awake();
	}
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		squad = GameObject.Find("Squad").GetComponent<Squad>(); //Will probably need to change later, but works for now
		agent = GetComponent<NavMeshAgent>();
	}
	
	protected override void Update () 
	{
		base.Update();
		//tells unit to go to target.position
		agent.SetDestination (target.position);
		//can be found in WorldObject
		CalculateBounds ();

	}

	protected override void OnGUI() 
	{
		base.OnGUI();
	}

	public override void SetHoverState(GameObject hoverObject) 
	{
		base.SetHoverState(hoverObject);
		//only handle input if owned by a human player and currently selected
		if(player && player.human && currentlySelected) 
		{
			// if the unit is selected, and the mouse is over the ground, then set the hover state to move
			if(hoverObject.tag == "Ground") player.hud.SetCursorState(CursorState.Move);
		}
	}

	// tells unit want to do on mouseClick
	// need to move this function over to squad.cs
	// not needed here at all
	// keeping it right now for reference and debugging proposes

	/*
	 * public override void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller) 
	{
		base.MouseClick(hitObject, hitPoint, controller);
		//only handle input if owned by a human player and currently selected
		if(player && player.human && currentlySelected) 
		{
			if(hitObject.tag == "Ground" && hitPoint != ResourceManager.InvalidPosition) 
			{
				float x = hitPoint.x;
				//makes sure that the unit stays on top of the surface it is on
				float y = hitPoint.y + player.SelectedObject.transform.position.y;
				float z = hitPoint.z;
				//calculates what direction the squad should face
				squadRotation = SquadFormationManager.calculateSquadRotation(transform.position, new Vector3(x,y,z));
				//makes a vector3[] to assign all the units to
				SquadFormationManager.calculateSquadPositions(seperation,squadRotation, new Vector3(x,y,z));
				//sets target.position to the Vector3 of the mouse click
				target.position = new Vector3(x,y,z);
				//need to change this so that each unit in the squad goes
				//to a specific position calculated by the squadFormationManager
			}
		}
	}*/
}