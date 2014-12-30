using UnityEngine;
using System.Collections;

public abstract class AbstractInterraction : MonoBehaviour {
	
	/****************
	 *  References  *
	 ****************/
	
	private bool loaded;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public AbstractInterraction(){
		this.loaded = false;
	}
	
	/******************
	 *    Methods     *
	 ******************/

	public bool IsLoaded(){
		return loaded;
	}

	public void Load(MainManager manager){
		if (IsLoaded ()) {
			Debug.LogError("Can't load Interraction: It's already loaded.");
		}
		else{
			loaded = true;
			OnLoad(manager);
		}
	}

	public void Update(){
		if(IsLoaded())
			OnUpdate ();
	}
	
	public void Unload(){
		if (!IsLoaded ()) {
			Debug.LogError("Can't unload Interraction: It's not already loaded.");
		}
		else{
			loaded = false;
			OnUnload();
		}
	}
	
	/******************
	 *  Declaration   *
	 ******************/
	
	protected abstract void OnLoad (MainManager manager);
	protected abstract void OnUpdate ();
	protected abstract void OnUnload ();

}
