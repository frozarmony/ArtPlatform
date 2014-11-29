using UnityEngine;
using System.Collections;

public abstract class AbstractAction {

	/****************
	 *  References  *
	 ****************/
	
	protected MainManager manager;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public AbstractAction(MainManager mainManager){
		this.manager = mainManager;
	}
	
	/******************
	 *  Declaration   *
	 ******************/
	
	public abstract void Do ();
	public abstract void Undo ();

}
