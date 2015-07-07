using UnityEngine;
using RTS;

public class PauseMenu : Menu 
{
	
	private Player player;

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		//initializes the player
		player = transform.root.GetComponent< Player >();
	}
	// every frame, check to see if the player presses escape
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape)) Resume();
	}
	// the list of different buttons in the pause menu
	// can easily add to this list
	// just add it to the array of strings
	// put a case for it in the handlebutton function
	// then write the function
	protected override void SetButtons ()
	{
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
	// resumes the game
	// need to change up the cursor.visible script, it is causing the cursor to not appear when it needs to
	private void Resume() 
	{
		// makes sure the game is running at normal speed
		Time.timeScale = 1.0f;
		GetComponent< PauseMenu >().enabled = false;
		if (player) 
		{
			player.GetComponent< UserInput > ().enabled = true;
		}
		Cursor.visible = false;
		ResourceManager.MenuOpen = false;
	}
	// if the player clicks the exit to main menu button
	private void ReturnToMainMenu() 
	{
		Application.LoadLevel("MainMenu");
		Cursor.visible = true;
	}
}