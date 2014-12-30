using UnityEngine;
using System.Collections;

public class InterractionMenu : HandMenu {
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public InterractionMenu(MainManager manager) : base(manager, MainManager.ContextOfGesture.Interraction){}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnLoad(){
		// Do Nothing
	}
	
	public override void OnTouch(int hanchorId){
		// Do Nothing
	}
}
