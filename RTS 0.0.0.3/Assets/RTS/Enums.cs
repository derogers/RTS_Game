namespace RTS 
{
	//These are useful for declaring things like a collection of states 
	//(as opposed to using an array of strings that we then have to access in annoying ways)
	public enum CursorState { Select, Move, Attack, PanLeft, PanRight, PanUp, PanDown, Harvest }
	public enum ResourceType { Money, Power }
}