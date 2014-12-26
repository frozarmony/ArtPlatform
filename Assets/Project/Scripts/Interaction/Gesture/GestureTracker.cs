using UnityEngine;
using System.Collections;
using Leap;

public abstract class GestureTracker {
	
	/****************
	 *  References  *
	 ****************/
	
	protected MainManager manager;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public GestureTracker(MainManager mainManager){
		this.manager = mainManager;
	}
	
	/******************
	 *  Declaration   *
	 ******************/
	
	public virtual void OnLoad (){}
	public abstract void OnUpdate ();
	public virtual void OnUnload (){}
	
}
