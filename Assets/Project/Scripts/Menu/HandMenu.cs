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

	protected StandardButtonItem CreateStandardButton(GameObject buttonPrefab, MainManager manager, int handAnchorId){
		// Instantiate Button
		GameObject buttonObject = (GameObject) Object.Instantiate (buttonPrefab);
		
		// Create, Add & Init StandardButtonItem script
		StandardButtonItem standardButton = buttonObject.AddComponent<StandardButtonItem> ();
		standardButton.InitHandItem (manager, handAnchorId);

		// If Exist Synchronized HandItem with HandItemInterface
		HandItemInterface interf = buttonObject.GetComponent<HandItemInterface> ();
		if(interf != null)
			interf.Init(standardButton);

		return standardButton;
	}
	
	protected ChoiceButtonItem CreateChoiceButton(MainManager manager, int handAnchorId){
		// Instantiate Button
		GameObject buttonObject = (GameObject) Object.Instantiate (manager.choiceButton);
		
		// Create, Add & Init ChoiceButtonItem script
		ChoiceButtonItem choiceButton = buttonObject.AddComponent<ChoiceButtonItem> ();
		choiceButton.InitHandItem (manager, handAnchorId);
		
		// If Exist Synchronized HandItem with HandItemInterface
		HandItemInterface interf = buttonObject.GetComponent<HandItemInterface> ();
		if(interf != null)
			interf.Init(choiceButton);
		
		return choiceButton;
	}

	protected MaterialProjectorItem CreateMaterialProjectorItem(MainManager manager, int handAnchorId){
		// Instantiate Button
		GameObject itemObject = (GameObject) Object.Instantiate (manager.materialProjector);
		
		// Create, Add & Init ChoiceButtonItem script
		MaterialProjectorItem materialProjector = itemObject.AddComponent<MaterialProjectorItem> ();
		materialProjector.InitHandItem (manager, handAnchorId);
		
		// If Exist Synchronized HandItem with HandItemInterface
		HandItemInterface interf = itemObject.GetComponent<HandItemInterface> ();
		if(interf != null)
			interf.Init(materialProjector);
		
		return materialProjector;
	}

}
