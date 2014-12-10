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
	
	/*******************
	 * Buttons Factory *
	 *******************/

	protected ButtonTrigger CreateStandardButton(GameObject buttonPrefab, MainManager manager, int handAnchorId){
		// Instantiate Button
		GameObject standardButton = (GameObject) Object.Instantiate (buttonPrefab);
		
		// Create and Add ButtonTrigger script
		ButtonTrigger trig = standardButton.AddComponent<StandardButtonTrigger> ();
		trig.InitButtonTrigger (manager, handAnchorId);

		return trig;
	}
	
	protected ButtonTrigger CreateChoiceButton(MainManager manager, int handAnchorId){
		// Instantiate Button
		GameObject choiceButton = (GameObject) Object.Instantiate (manager.choiceButton);
		
		// Create and Add ButtonTrigger script
		ChoiceButtonTrigger trig = choiceButton.AddComponent<ChoiceButtonTrigger> ();
		trig.InitButtonTrigger (manager, handAnchorId);
		
		return trig;
	}

}
