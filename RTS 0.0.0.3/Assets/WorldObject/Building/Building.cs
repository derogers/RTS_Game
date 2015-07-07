using UnityEngine;
using System.Collections.Generic;
using RTS;

public class Building : WorldObject 
{

	//we might have buildings in our game, but currently not sure of the
	//purpose/role of them. Currently they are set up like a traditional
	//RTS. Last on the ToDo list
	public float maxBuildProgress = 10.0f;
	public Texture2D rallyPointImage, sellImage;
	public AudioClip finishedJobSound;
	public float finishedJobVolume = 1.0f;
	
	protected Vector3 spawnPoint, rallyPoint;
	protected Queue<string> buildQueue;
	
	private float currentBuildProgress = 0.0f;
	private bool needsBuilding = false;

	protected override void Awake() 
	{
		base.Awake();
		buildQueue = new Queue< string >();
		float spawnX = selectionBounds.center.x + transform.forward.x * selectionBounds.extents.x + transform.forward.x * 10;
		float spawnZ = selectionBounds.center.z + transform.forward.z + selectionBounds.extents.z + transform.forward.z * 10;
		spawnPoint = new Vector3(spawnX, 0.0f, spawnZ);
	}
	// Use this for initialization
	protected override void Start () 
	{
		base.Start();
	}
	
	protected override void Update () 
	{
		base.Update();
		ProcessBuildQueue();
	}
	
	protected override void OnGUI() 
	{
		base.OnGUI();
	}

	protected void CreateUnit(string unitName) 
	{
		buildQueue.Enqueue(unitName);
	}

	protected void ProcessBuildQueue() 
	{
		if(buildQueue.Count > 0) 
		{
			currentBuildProgress += Time.deltaTime * ResourceManager.BuildSpeed;
			if(currentBuildProgress > maxBuildProgress) 
			{
				if(player) player.AddUnit(buildQueue.Dequeue(), spawnPoint, transform.rotation);
				currentBuildProgress = 0.0f;
			}
		}
	}

	public string[] getBuildQueueValues() 
	{
		string[] values = new string[buildQueue.Count];
		int pos=0;
		foreach(string unit in buildQueue) values[pos++] = unit;
		return values;
	}
	
	public float getBuildPercentage() 
	{
		return currentBuildProgress / maxBuildProgress;
	}
}