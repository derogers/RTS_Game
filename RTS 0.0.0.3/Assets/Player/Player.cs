using UnityEngine;
using System.Collections.Generic;
using RTS;

public class Player : MonoBehaviour 
{
	// Stores the player username
	public string username;

	// Says if the player is human or not
	public bool human;
	public HUD hud;
	// Stores the WorldObject you click on
	public WorldObject SelectedObject { get; set; }
	public int startPoints;
	public Color teamColor;
	// Will probably get rid of these values. Might have some sort of points holder for player to spend to build their army
	public int startMoney, startMoneyLimit, startPower, startPowerLimit;
	private Dictionary< ResourceType, int > resources, resourceLimits;
	
	// Use this for initialization
	void Start () 
	{
		hud = GetComponentInChildren< HUD >();
		// Resources are not set up correctly, but we do need them for
		// Creating a custom army using a points system
		AddStartResourceLimits();
		AddStartResources();
	}

	void Awake() 
	{
		resources = InitResourceList();
		resourceLimits = InitResourceList();
	}

	// Update is called once per frame
	void Update () 
	{
		if(human) 
		{
			hud.SetResourceValues(resources, resourceLimits);
		}
	}
	// Allows you to add a unit to the player
	// Need to make sure this works with th squad code properly when implemented
	public void AddUnit(string unitName, Vector3 spawnPoint, Quaternion rotation) 
	{
		Debug.Log ("add " + unitName + " to player");
		Units units = GetComponentInChildren< Units >();
		Unit newUnit = (Unit)Instantiate(ResourceManager.GetUnit(unitName), spawnPoint, rotation);
		newUnit.transform.parent = units.transform;
	}

	private Dictionary< ResourceType, int > InitResourceList() 
	{
		Dictionary< ResourceType, int > list = new Dictionary< ResourceType, int >();
		list.Add(ResourceType.Money, 0);
		list.Add(ResourceType.Power, 0);
		return list;
	}

	private void AddStartResourceLimits() 
	{
		IncrementResourceLimit(ResourceType.Money, startMoneyLimit);
		IncrementResourceLimit(ResourceType.Power, startPowerLimit);
	}
	
	private void AddStartResources() 
	{
		AddResource(ResourceType.Money, startMoney);
		AddResource(ResourceType.Power, startPower);
	}

	public void AddResource(ResourceType type, int amount) 
	{
		resources[type] += amount;
	}
	
	public void IncrementResourceLimit(ResourceType type, int amount) 
	{
		resourceLimits[type] += amount;
	}
}