using UnityEngine;
using System.Collections;

public abstract class HandMenu {

	/****************
	 *  References  *
	 ****************/

	protected MainManager manager;
	
	/******************
	 *  Constructor   *
	 ******************/

	public HandMenu(MainManager mainManager){
		this.manager = mainManager;
	}

	/******************
	 *  Declaration   *
	 ******************/

	public abstract void OnLoad ();
	public abstract void OnTouch (int anchorId);

}
