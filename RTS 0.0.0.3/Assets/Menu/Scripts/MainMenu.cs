using UnityEngine;
using System.Collections;
using RTS;

// Defines how the main menu of the game will behave

public class MainMenu : Menu 
{
	/*
	 * Desplays the list of different buttons in the pause menu
	 * 
	 * Can easily add to this list, just:
	 * 1.	add the name of the button to the array of strings
	 * 2.	append a case in switch(text) of the HandleButton function
	 * 3. write the function(s) in the MainMenu class.
	*/
	protected override void SetButtons () 
	{
		// buttons is a protected string declared in Menu.cs
		buttons = new string[] {"New Game", "Quit Game"};
	}
	
	protected override void HandleButton (string text) 
	{
		switch(text) 
		{
		case "New Game": NewGame(); break;
		case "Quit Game": ExitGame(); break;
		default: break;
		}
	}

	// If the player presses the "New Game" button
	private void NewGame() 
	{
		// Closes the menu
		ResourceManager.MenuOpen = false;
		// Loads the level called "Alpha Level 0.0.1"
		Application.LoadLevel("Alpha Level 0.0.1");
		// Makes sure that the loaded level runs at normal speed
		Time.timeScale = 1.0f;
	}
}