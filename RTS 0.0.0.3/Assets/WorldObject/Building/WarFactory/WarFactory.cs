using UnityEngine;
using System.Collections.Generic;
using RTS;

public class WarFactory : Building 
{

	// specific building made that is suppose to create paladins
	// we could possibly add this script to a unit with some work?

	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
		actions = new string[] { "Paladin" };
	}

	public override void PerformAction(string actionToPerform) 
	{
		base.PerformAction(actionToPerform);
		CreateUnit(actionToPerform);
	}
}