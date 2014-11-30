using UnityEngine;
using System.Collections;

public abstract class HandMenu {

	/****************
	 *  References  *
	 ****************/

	protected MainManager manager;
	private MainManager.ContextOfGesture contextGesture;
	
	/******************
	 *  Constructor   *
	 ******************/

	public HandMenu(MainManager mainManager, MainManager.ContextOfGesture contextGestureRef){
		this.manager = mainManager;
		this.contextGesture = contextGestureRef;
	}
	
	/******************
	 *    Getters     *
	 ******************/

	public MainManager.ContextOfGesture GetContextOfGesture(){
		return contextGesture;
	}

	/******************
	 *  Declaration   *
	 ******************/

	public abstract void OnLoad ();
	public abstract void OnTouch (int anchorId);

}
