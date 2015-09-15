using UnityEngine;
using System.Collections;
using RTS;

// Defines how the user interacts with the game

public class UserInput : MonoBehaviour 
{
	private Player player;
	public Squad holdSquad;
	public Vector3 hitPoint;

	// Use this for initialization
	void Start () 
	{
		/*
		 * Tells unity to go to the root of the object that this script
		 * belongs to and then find the player component that we added.
		 * now it is possible to interact with the player directly
		 */
		player = transform.root.GetComponent<Player>();
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
	 * Function that handles user camra movement.
	 * WASD moves the camera Foward, Left, Backward, Right (respectively)
	 * Mouse scroll moves the camera Up and Down
	 */
	private void MoveCamera() 
	{
		// define the current mouse position
		float xpos = Input.mousePosition.x;
		float ypos = Input.mousePosition.y;
		// new vector for our movement
		Vector3 movement = new Vector3(0,0,0);
		bool mouseScroll = false;
		
		/* Horizontal camera movement
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
		
		// Vertical camera movement
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
		
		// Make sure movement is in the direction the camera is pointing
		// But ignore the vertical tilt of the camera to get sensible scrolling
		movement = Camera.main.transform.TransformDirection(movement);
		movement.y = 0;
		
		// Away from ground movement
		movement.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");
		
		//calculate desired camera position based on received input
		Vector3 origin = Camera.main.transform.position;
		Vector3 destination = origin;
		destination.x += movement.x;
		destination.y += movement.y;
		destination.z += movement.z;
		
		// Limits the distance the camera can be from the ground be between a minimum and maximum distance
		if(destination.y > ResourceManager.MaxCameraHeight) 
		{
			destination.y = ResourceManager.MaxCameraHeight;
		} 
		else if(destination.y < ResourceManager.MinCameraHeight) 
		{
			destination.y = ResourceManager.MinCameraHeight;
		}
		
		// If a change in position is detected perform the necessary update
		if(destination != origin) 
		{
			Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
		}

		if(!mouseScroll) 
		{
			player.hud.SetCursorState(CursorState.Select);
		}
	}
	// Used to totate the camera
	private void RotateCamera() 
	{
		Vector3 origin = Camera.main.transform.eulerAngles;
		Vector3 destination = origin;
		
		// Detect rotation amount if ALT is being held and the Right mouse button is down
		if(((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))||(Input.GetKey(KeyCode.Mouse2))) 
		{
			/*destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;*/
			destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
		}
		
		// If a change in position is detected perform the necessary update
		if(destination != origin) 
		{
			Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
		}
	}

	// Check to see if the left or right mouse is clicked
	private void MouseActivity() 
	{
		if(Input.GetMouseButtonDown(0)) LeftMouseClick();
		else if(Input.GetMouseButtonDown(1)) RightMouseClick();
		MouseHover ();
	}
	// If left mouse click
	private void LeftMouseClick() 
	{
		//If the Mouse is in bounds, actually do something
		if (player.hud.MouseInBounds ()) 
		{
			// Save whatever you clicked on to hitObject using the FindHitObject function
			GameObject hitObject = FindHitObject ();

			// Find where the mouse click was using FindHitPoint function
			hitPoint = FindHitPoint ();

			//Debug.Log( hitObject.name ); // shot out an error message

			//If you hit an object in a valid position
			if(hitObject && hitPoint != ResourceManager.InvalidPosition) 
			{
				//If you already have something selected
				if(player.SelectedObject) 
				{
					//Debug.Log( player.SelectedObject.name );

					//If the player has a unit selected
					if(player.SelectedObject.tag == "Unit")
					{
						//Debug.Log( "0" );
						holdSquad = ResourceManager.GetUnit (player.SelectedObject.name).squad; // fixed it bby sort of
						// used for debuging purposes
						if(holdSquad)
						{
							//Debug.Log (holdSquad);
							//Debug.Log ("have selected object and tag = unit");
						}
						//Debug.Log( "before" );
						player.SelectedObject.MouseClickSquad(holdSquad, hitPoint, player);

						//Debug.Log( "after" );
					}
					//If the player doesnt have a unit selected, proceed as normal
					//Each class can write an override function for MouseClick
					else
					{
						//Debug.Log( "Selected object & non Unit" );
						player.SelectedObject.MouseClick(hitObject, hitPoint, player);
					}
				}
				//If you dont have anything selected and you didnt click on the ground
				else if(hitObject.tag != "Ground") 
				{
					Debug.Log (hitObject);
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
	}

	// If no object is hit, return null
	// If object is hit, return that object
	// Need this to not return the capsule, but return the unit, currently doesnt work properly
	private GameObject FindHitObject() 
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) return hit.collider.gameObject; // might need to change the return value
		return null;
	}

	// If the raycast doesnt hit anything, return invalidPosition
	// If the raycast hits something, return the point it hit
	private Vector3 FindHitPoint() 
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if(Physics.Raycast(ray, out hit)) return hit.point;
		return ResourceManager.InvalidPosition;
	}

	// Decides what to do when there is a right mouse click
	private void RightMouseClick() 
	{
		if(player.hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject) 
		{
			player.SelectedObject.SetSelection(false, player.hud.GetPlayingArea());
			player.SelectedObject = null;
		}
	}

	// Decides what to do
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

	// Pause menu function
	private void OpenPauseMenu() 
	{
		Time.timeScale = 0.0f;
		GetComponentInChildren< PauseMenu >().enabled = true;
		GetComponent< UserInput >().enabled = false;
		Cursor.visible = true;
		ResourceManager.MenuOpen = true;
	}
}