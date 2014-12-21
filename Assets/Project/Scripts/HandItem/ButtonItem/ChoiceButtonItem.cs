using UnityEngine;
using System.Collections;

public class ChoiceButtonItem : ButtonItem {

	/******************
	 * Initialization *
	 ******************/

	public void InitChoiceDisplay(Texture choiceTexture){
		// Init LookAtCamera Script
		this.gameObject.AddComponent<LookAtCamera> ().targetCamera = manager.GetMainCamera ();

		// Init Choice Texture
		this.transform.GetChild (0).renderer.material.mainTexture = choiceTexture;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void SetSelected(bool selected){
		isSelected = selected;
		this.GetComponent<Animator>().SetBool("Selected", isSelected);
	}
	
	protected override void SetFocus(bool focusOn){
		// Do Nothing
	}
}
