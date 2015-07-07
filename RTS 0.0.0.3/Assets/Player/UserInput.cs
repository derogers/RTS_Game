using UnityEngine;
using System.Collections;
using RTS;

public class UserInput : MonoBehaviour 
{
	private Player player;
	public Squad holdSquad;
	public Vector3 hitPoint;

	// Use this for initialization
	void Start () 
	{
		/*
		 * tells unity to go to the root of the object that this script
		 * belongs to and then find the player component that we added.
		 * Now it is possible to interact with the player directly
		 */
		player = transform.root.GetComponent<Player>();
	    
		// NEED TO INITIALIZE HOLDSQUAD
	}

	// Update is called once per frame
	void Update () 
	{
		if(player && player.human) //only handle input for a human
		{
			MoveCamera();
			if(Input.GetKeyDown(KeyCode.Escape)) OpenPauseMenu();
			RotateCamera();
			MouseActivity();
		}
	}
	/*
	 * function that handles user camra movement.
	 */
	private void MoveCamera() 
	{
		// define the current mouse position
		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		// new vector for our movement
		Vector3 movement = new Vector3(0,0,0);
		bool mouseScroll = false;
		
		/* horizontal camera movement
		 * adds movement in the appropriate axis when the mouse is inside
		 * the region that we defined for movement
		 */
		if((xpos >= 0 && xpos < ResourceManager.ScrollWidth)||Input.GetKey (KeyCode.A)) 
		{
			movement.x -= ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanLeft);
			mouseScroll = true;
		} 
		else if((xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth)|| Input.GetKey (KeyCode.D)) 
		{
			movement.x += ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanRight);
			mouseScroll = true;
		}
		
		//vertical camera movement
		if((ypos >= 0 && ypos < ResourceManager.ScrollWidth)|| Input.GetKey (KeyCode.S)) 
		{
			movement.z -= ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanDown);
			mouseScroll = true;
		} 
		else if((ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth) || Input.GetKey (KeyCode.W)) 
		{
			movement.z += ResourceManager.ScrollSpeed;
			player.hud.SetCursorState(CursorState.PanUp);
			mouseScroll = true;
		}
		
		//make sure movement is in the direction the camera is pointing
		//but ignore the vertical tilt of the camera to get sensible scrolling
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;
		
		//away from ground movement
		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
		
		//calculate desired camera position based on received input
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;
		
		//limit away from ground movement to be between a minimum and maximum distance
		if(destination.y > ResourceManager.MaxCameraHeight) 
		{
			destination.y = ResourceManager.MaxCameraHeight;
		} 
		else if(destination.y < ResourceManager.MinCameraHeight) 
		{
			destination.y = ResourceManager.MinCameraHeight;
		}
		
		//if a change in position is detected perform the necessary update
		if(destination != origin) 
		{
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
		}

		if(!mouseScroll) 
		{
			player.hud.SetCursorState(CursorState.Select);
		}
	}
	//used to totate the camera
	private void RotateCamera() 
	{
		Vector3 origin = Camera.main.transform.eulerAngles;
		Vector3 destination = origin;
		
		//detect rotation amount if ALT is being held and the Right mouse button is down
		if(((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))||(Input.GetKey(KeyCode.Mouse2))) 
		{
			/*destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;*/
			destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
		}
		
		//if a change in position is detected perform the necessary update
		if(destination != origin) 
		{
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
		}
	}

	//check to see if the left or right mouse is clicked
	private void MouseActivity() 
	{
		if(Input.GetMouseButtonDown(0)) LeftMouseClick();
		else if(Input.GetMouseButtonDown(1)) RightMouseClick();
		MouseHover ();
	}
	// if left mouse click
	// the function needs work done
	private void LeftMouseClick() 
	{
		//If the Mouse is in bounds, actually do something
		if (player.hud.MouseInBounds ()) 
		{
			// Save whatever you clicked on to hitObject using the FindHitObject function
			GameObject hitObject = FindHitObject ();

			// Find where the mouse click was using FindHitPoint function
			hitPoint = FindHitPoint ();

			Debug.Log( hitObject.name );

			//If you hit an object in a valid position
			if(hitObject && hitPoint != ResourceManager.InvalidPosition) 
			{
				//If you already have something selected
				if(player.SelectedObject) 
				{
					Debug.Log( player.SelectedObject.name );

					//If the player has a unit selected
					if(player.SelectedObject.tag == "Unit")
					{
						Debug.Log( "0" );
						holdSquad = ResourceManager.GetUnit (player.SelectedObject.name).squad; // fixed it bby sort of
						Debug.Log( "before" );
						player.SelectedObject.MouseClickSquad(holdSquad, hitPoint, player);//CURRENT PROBLEM, doesnt run the function
						Debug.Log( "after" );
					}
					//If the player doesnt have a unit selected, proceed as normal
					//Each class can write an override function for MouseClick
					else
					{
						Debug.Log( "Other Way" );
						player.SelectedObject.MouseClick(hitObject, hitPoint, player);
					}
				}
				//If you dont have anything selected and you didnt click on the ground
				else if(hitObject.name != "Ground") 
				{
					//Set worldObject to whatever you clicked on
					WorldObject worldObject = hitObject.transform.parent.GetComponent<WorldObject>();
					//If you actually clicked on something
					if(worldObject) 
					{
						//we already know the player has no selected object
						//So we set whatever they clicked on to thier selected object
						player.SelectedObject = worldObject;
						//Set it so the game knows they have something selected
						worldObject.SetSelection(true, player.hud.GetPlayingArea());
					}
				}
			}
		}


		//Keeping around Just in Case, although this was a complete mess that didnt come close to working
		/*
		Debug.Log ("test 1");
		if (player.hud.MouseInBounds ()) 
		{
			//if the mouse click is in bounds
			GameObject hitObject = FindHitObject ();  //assigns hit object by calling the FindHitObject funcion
			Debug.Log (hitObject.name);
			hitPoint = FindHitPoint (); //set vector3 hitpoint by calling the FindHitPoint function
			Debug.Log ("test 2");

			// This causes an error if you click the ground b/c ground doesnt have a parent
			if(hitObject.tag != "Ground")
			{
				if (hitObject.transform.parent.tag == "Unit") // if the hit object is a unit, need to make sure it is calling the unit and not a child of the unit
				{
					Debug.Log ("test 3");
					holdSquad = ResourceManager.GetUnit (hitObject.transform.parent.name).squad; // fixed it bby

					if (hitObject && hitPoint != ResourceManager.InvalidPosition) 
					{
						Debug.Log ("test 4");
						if (player.SelectedObject)
						{
							//not exactly sure what this does, if you figure out change this comment to the answer
							// MAKE A FUNCTION OVVERIDE FOR MOUSE CLICK
							// made another function called MouseClickSquad that can be found in Squad.cs
							Debug.Log ("test 5");
							player.SelectedObject.MouseClick (hitObject, hitPoint, player);
						}
						else
						{
							Debug.Log ("test 6");
							if (hitObject.tag != "Ground")
							{
								Debug.Log ("test 7");
								WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject > ();
								Debug.Log (worldObject.name);

								if (worldObject) 
								{
									Debug.Log ("test 8");
									//we already know the player has no selected object
									player.SelectedObject = worldObject;
									worldObject.SetSelection (true, player.hud.GetPlayingArea ());
								}
							}
						}
					}
				}
			}
			else
			{
				Debug.Log ("test 9"); 
				if (hitObject && hitPoint != ResourceManager.InvalidPosition) 
				{
					Debug.Log ("test 10");
					if (player.SelectedObject) 
					{
						Debug.Log ("test 11");
						player.SelectedObject.MouseClick (hitObject, hitPoint, player);
					}
					else 
					{
						Debug.Log ("test 12");
						if (hitObject.tag != "Ground")
						{
							Debug.Log ("test 13");
							WorldObject worldObject = hitObject.transform.parent.GetComponent< WorldObject > ();
							if (worldObject) 
							{
								Debug.Log ("test 14");
							//we already know the player has no selected object
							player.SelectedObject = worldObject;
							worldObject.SetSelection (true, player.hud.GetPlayingArea ());
							}
						}
					}
				}
			}
		}*/
	}

	//if no object is hit, return null
	//if object is hit, return that object
	// need this to not return the capsule, but return the unit, currently doesnt work properly
	private GameObject FindHitObject() 
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) return hit.collider.gameObject; // might need to change the return value
		return null;
	}
	//if the raycast doesnt hit anything, return invalidPosition
	//if the raycast hits something, return the point it hit
	private Vector3 FindHitPoint() 
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) return hit.point;
		return ResourceManager.InvalidPosition;
	}
	//decides what to do when there is a right mouse click
	private void RightMouseClick() 
	{
		if(player.hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject) 
		{
			player.SelectedObject.SetSelection(false, player.hud.GetPlayingArea());
			player.SelectedObject = null;
		}
	}
	//decides what to do
	private void MouseHover() 
	{
		if(player.hud.MouseInBounds()) 
		{
			GameObject hoverObject = FindHitObject();
			if(hoverObject) 
			{
				if(player.SelectedObject) player.SelectedObject.SetHoverState(hoverObject);
				else if(hoverObject.tag != "Ground") 
				{
					Player owner = hoverObject.transform.root.GetComponent< Player >();
					if(owner) 
					{
						Unit unit = hoverObject.transform.parent.GetComponent< Unit >();
						Building building = hoverObject.transform.parent.GetComponent< Building >();
						if(owner.username == player.username && (unit || building)) player.hud.SetCursorState(CursorState.Select);
					}
				}
			}
		}
	}
	//pause menu function
	private void OpenPauseMenu() 
	{
		Time.timeScale = 0.0f;
		GetComponentInChildren< PauseMenu >().enabled = true;
		GetComponent< UserInput >().enabled = false;
		Cursor.visible = true;
		ResourceManager.MenuOpen = true;
	}
}