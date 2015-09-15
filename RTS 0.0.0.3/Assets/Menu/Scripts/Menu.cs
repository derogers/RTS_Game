using UnityEngine;
using System.Collections;
using RTS;

// The menus class sets up the basic menu structure and basic universal menu functions 

public class Menu : MonoBehaviour 
{
	
	public GUISkin mySkin;
	public Texture2D header;
	
	protected string[] buttons;

	// Use this for initialization
	protected virtual void Start () 
	{
		SetButtons();
	}
	
	protected virtual void OnGUI() 
	{
		DrawMenu();
	}
	
	protected virtual void DrawMenu() 
	{
		// Default implementation for a menu consisting of a vertical list of buttons
		GUI.skin = mySkin;
		float menuHeight = GetMenuHeight();
		
		float groupLeft = Screen.width / 2 - ResourceManager.MenuWidth / 2;
		float groupTop = Screen.height / 2 - menuHeight / 2;
		GUI.BeginGroup(new Rect(groupLeft, groupTop, ResourceManager.MenuWidth, menuHeight));
		
		// Creates Background box
		GUI.Box(new Rect(0, 0, ResourceManager.MenuWidth, menuHeight), "");

		// Header image
		GUI.DrawTexture(new Rect(ResourceManager.Padding, ResourceManager.Padding, ResourceManager.HeaderWidth, ResourceManager.HeaderHeight), header);
		
		// Creates Menu buttons
		if(buttons != null) 
		{
			float leftPos = ResourceManager.MenuWidth / 2 - ResourceManager.ButtonWidth / 2;
			float topPos = 2 * ResourceManager.Padding + header.height;
			for(int i = 0; i < buttons.Length; i++) 
			{                if(i > 0) topPos += ResourceManager.ButtonHeight + ResourceManager.Padding;
				if(GUI.Button(new Rect(leftPos, topPos, ResourceManager.ButtonWidth, ResourceManager.ButtonHeight), buttons[i])) 
				{
					HandleButton(buttons[i]);
				}
			}
		}
		GUI.EndGroup();
	}
	
	protected virtual void SetButtons() 
	{
		// A child class needs to set this for buttons to appear
	}
	
	protected virtual void HandleButton(string text) 
	{
		// A child class needs to set this to handle button clicks
	}
	
	protected virtual float GetMenuHeight() 
	{
		// Resets buttonHeight to zero
		float buttonHeight = 0;
		// If the vector "buttons" is not empty, multiply the number of elements in the vector "buttons" by the height of a button
		if (buttons != null) 
		{
			buttonHeight = buttons.Length * ResourceManager.ButtonHeight;
		}

		// Resets paddingHeight to 2 * Padding
		float paddingHeight = 2 * ResourceManager.Padding;
		// If vector "buttons" is not empty, add  on (the number of elements in the vector "buttons" * Padding)
		// So padding Height = (2 * Padding) + (buttons.Length * Padding)
		if (buttons != null) 
		{
			paddingHeight += buttons.Length * ResourceManager.Padding;
		}
		// Returns the menu height
		// the menu height = HeaderHeight + buttonHeight + padding Height
		return ResourceManager.HeaderHeight + buttonHeight + paddingHeight;
	}

	// Function to completly quit the game
	// Doesnt save anything
	protected void ExitGame() 
	{
		Application.Quit();
	}
}