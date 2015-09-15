using UnityEngine;
using RTS;

// Defines how the in-game pause menu will behave

public class PauseMenu : Menu 
{
	
	private Player player;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		// Initializes the player
		player = transform.root.GetComponent< Player >();
	}

	// Every frame, check to see if the player presses escape
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape)) Resume();
	}
	/*
	 * Desplays the list of different buttons in the pause menu
	 * 
	 * Can easily add to this list, just:
	 * 1.	add the name of the button to the array of strings
	 * 2.	append a case in switch(text) of the HandleButton function
	 * 3. write the function(s) in the PauseMenu class.
	*/
	protected override void SetButtons ()
	{
		// buttons is a protected string declared in Menu.cs
		buttons = new string[] {"Resume", "Exit To Main Menu"};
	}
	
	protected override void HandleButton (string text) 
	{
		switch(text) 
		{
		case "Resume": Resume(); break;
		case "Exit To Main Menu": ReturnToMainMenu(); break;
		default: break;
		}
	}

	// Resumes the game
	private void Resume() 
	{
		// Makes sure the game is running at normal speed
		Time.timeScale = 1.0f;
		GetComponent< PauseMenu >().enabled = false;
		if (player) 
		{
			player.GetComponent< UserInput > ().enabled = true;
		}
		Cursor.visible = false;
		ResourceManager.MenuOpen = false;
	}
	// If the player clicks the exit to main menu button
	// There is a bug in the cursor.visible script that makes it invisible if you go back to the main menu
	private void ReturnToMainMenu() 
	{
		Application.LoadLevel("MainMenu");
		Cursor.visible = true;
	}
}