using UnityEngine;
using RTS;
using System.Collections.Generic;

public class WorldObject : MonoBehaviour 
{

	//might need to add "unmodified" and "modified" most every stat later
	//like how SM bikers have T4, but have a modified T5 on a bike

	//not sure where to add different unit types

	//not sure where to put different units of the same type

	//Probably need to move some of these stats to Unit.cs
	//example- speed, strength, toughness, ballisticSkill

	public string objectName = "WorldObject";
	public Texture2D buildImage;
	//might need to scrap or combine the variable below becides the audio stuff
	public float weaponRange = 10.0f, weaponRechargeTime = 1.0f, weaponAimSpeed = 1.0f, detectionRange = 20.0f;
	public AudioClip attackSound, selectSound, useWeaponSound;
	public float attackVolume = 1.0f, selectVolume = 1.0f, useWeaponVolume = 1.0f;

	protected Player player;
	protected string[] actions = {};
	protected bool currentlySelected = false;
	protected Bounds selectionBounds;
	protected Rect playingArea = new Rect(0.0f, 0.0f, 0.0f, 0.0f);

	protected virtual void Awake() 
	{
		selectionBounds = ResourceManager.InvalidBounds;
		CalculateBounds();
	}
	// Use this for initialization
	protected virtual void Start () 
	{
		player = transform.root.GetComponentInChildren< Player >();
	}
	
	protected virtual void Update () 
	{
		
	}
	
	protected virtual void OnGUI() 
	{
		if(currentlySelected) DrawSelection();
	}

	public void SetSelection(bool selected, Rect playingArea) 
	{
		currentlySelected = selected;
		if(selected) this.playingArea = playingArea;
	}

	public string[] GetActions() 
	{
		return actions;
	}
	
	public virtual void PerformAction(string actionToPerform) 
	{
		//it is up to children with specific actions to determine what to do with each of those actions
	}
	/* Below is something Derek made
	 * It is probably not what he should have made
	 * Derek is going to try anyways
	 * Wish Derek the best of luck
	 * 
	 * 
	 */
	public virtual void MouseClickSquad(Squad holdSquad, Vector3 hitPoint, Player player) 
	{
		//Debug.Log ("Herro 1");
		//only handle input if currently selected
		if(currentlySelected && holdSquad && holdSquad.tag != "Ground") 
		{
			Squad squad = holdSquad;//holdSquad.transform.parent.GetComponent< Squad >(); // currently this value is null when click on the ground
			squad.MouseClickSquad(holdSquad, hitPoint, player);
			//Debug.Log (squad);
			//Debug.Log ("Herro 2");

			//clicked on another selectable object
			if(squad) 
			{
				ChangeSelection(squad, player);
			}
		}
	}

	public virtual void MouseClick(GameObject hitObject, Vector3 hitPoint, Player controller) 
	{
		//only handle input if currently selected
		if(currentlySelected && hitObject && hitObject.tag != "Ground") 
		{
			WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject >();
			//clicked on another selectable object
			if(worldObject)
			{
				ChangeSelection(worldObject, controller);
			}
		}
	}

	private void ChangeSelection(WorldObject worldObject, Player controller) 
	{
		//this should be called by the following line, but there is an outside chance it will not
		SetSelection(false, playingArea);
		if(controller.SelectedObject) controller.SelectedObject.SetSelection(false, playingArea);
		controller.SelectedObject = worldObject;
		worldObject.SetSelection(true, controller.hud.GetPlayingArea());
	}

	private void DrawSelection() 
	{
		GUI.skin = ResourceManager.SelectBoxSkin;
		Rect selectBox = WorkManager.CalculateSelectionBox(selectionBounds, playingArea);
		//Draw the selection box around the currently selected object, within the bounds of the playing area
		GUI.BeginGroup(playingArea);
		DrawSelectionBox(selectBox);
		GUI.EndGroup();
	}
	//called in WorldObject.cs awake
	// called in Unit.cs Update
	public void CalculateBounds() 
	{
		selectionBounds = new Bounds(transform.position, Vector3.zero);
		foreach(Renderer r in GetComponentsInChildren< Renderer >()) 
		{
			selectionBounds.Encapsulate(r.bounds);
		}
	}
	//draws the selection box
	protected virtual void DrawSelectionBox(Rect selectBox) 
	{
		GUI.Box(selectBox, "");
	}

	public virtual void SetHoverState(GameObject hoverObject) 
	{
		//only handle input if owned by a human player and currently selected
		if(player && player.human && currentlySelected) 
		{
			if(hoverObject.tag != "Ground") player.hud.SetCursorState(CursorState.Select);
		}
	}
	//returns bool if something is owned by the player
	public bool IsOwnedBy(Player owner) 
	{
		if(player && player.Equals(owner)) 
		{
			return true;
		} 
		else 
		{
			return false;
		}
	}
}