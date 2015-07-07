using UnityEngine;
using System.Collections;
using RTS;

public class MainMenu : Menu 
{
	// talks about most of this stuff in the pausemenu.cs
	protected override void SetButtons () 
	{
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
	// if the player presses the New Game button
	private void NewGame() 
	{
		// loads the level called "Alpha Level 0.0.1"
		ResourceManager.MenuOpen = false;
		Application.LoadLevel("Alpha Level 0.0.1");
		//makes sure that the loaded level runs at normal speed
		Time.timeScale = 1.0f;
	}
}