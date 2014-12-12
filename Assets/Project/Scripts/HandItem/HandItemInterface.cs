using UnityEngine;
using System.Collections;

public class HandItemInterface : MonoBehaviour {
	
	/****************
	 *  References  *
	 ****************/

	private HandItem handItem;
	
	/******************
	 *   Constructor  *
	 ******************/

	public HandItemInterface() : base(){
		this.handItem = null;
	}
	
	/******************
	 * Initialization *
	 ******************/

	public void Init(HandItem handItemRef){
		this.handItem = handItemRef;
	}
	
	/****************
	 * Sync Methods *
	 ****************/

	// This function must be called by Animator when HandItem is loaded
	public void OnLoaded(){
		if(handItem == null)
			Debug.LogError ("HandItem '" + gameObject.name + "' can't be loaded: HandItem reference is null!");
		else
			handItem.Load();
	}

	// This function must be called by Animator when HandItem is unloaded
	public void OnUnloaded(){
		if(handItem == null)
			Debug.LogError ("HandItem '" + gameObject.name + "' can't be unloaded: HandItem reference is null!");
		else
			handItem.Unload();
	}

}
