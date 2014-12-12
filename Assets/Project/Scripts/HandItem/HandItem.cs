using UnityEngine;
using System.Collections;

public abstract class HandItem : MonoBehaviour {
	
	/****************
	 *  References  *
	 ****************/

	private bool isLoaded;
	protected MainManager manager;
	protected int handAnchorId;
	
	/******************
	 *   Constructor  *
	 ******************/
	
	public HandItem(){
		// Init References
		isLoaded = false;
		manager = null;
		handAnchorId = -1;
	}
	
	/******************
	 * Initialization *
	 ******************/
	
	public virtual void InitHandItem(MainManager mainManager, int anchorId){
		// Init References
		isLoaded = false;
		manager = mainManager;
		handAnchorId = anchorId;
	}
	
	/******************
	 *    Getters     *
	 ******************/
	
	public int GetHandHanchorId(){
		return handAnchorId;
	}

	public bool IsLoaded(){
		return isLoaded;
	}
	
	/******************
	 *  Start Methods *
	 ******************/

	public void StartUnloading(){
		this.GetComponent<Animator> ().SetBool ("Enable", false);
	}

	/******************
	 *  Event Methods *
	 ******************/

	public void Load(){
		if(isLoaded){
			Debug.LogError ("HandItem '" + gameObject.name + "' can't be loaded: HandItem is already loaded!");
		}
		else{
			isLoaded = true;
			OnLoaded ();
		}
	}

	public void Unload(){
		if(!isLoaded){
			Debug.LogError ("HandItem '" + gameObject.name + "' can't be unloaded: HandItem is not loaded yet!");
		}
		else{
			isLoaded = false;
			OnUnloaded ();
			manager.NotifyHandItemUnloaded();
		}
	}
	
	/******************
	 *  Declaration   *
	 ******************/
	
	protected abstract void OnLoaded();
	protected abstract void OnUnloaded();

}
