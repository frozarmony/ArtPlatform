﻿using UnityEngine;
using System.Collections;

public class ChoiceButtonItem : ButtonItem {

	/******************
	 * Initialization *
	 ******************/

	public void InitChoiceDisplay(Texture choiceTexture){
		// Find ChoiceDisplay Ref
		Transform choiceDisplay = this.transform.FindChild("ChoiceDisplay");

		if(choiceDisplay != null){
			// Init LookAtCamera Script
			choiceDisplay.gameObject.AddComponent<LookAtCamera> ().targetCamera = manager.GetMainCamera ();

			// Init Choice Texture
			choiceDisplay.renderer.material.mainTexture = choiceTexture;
		}
	}

	public void SetMainColors(Color mainColor, Color specularColor){
		// Find ChoiceDisplay Ref
		Transform choiceDisplay = this.transform.FindChild("ChoiceDisplay");

		if(choiceDisplay != null){			
			// Init Choice Texture
			choiceDisplay.renderer.material.SetColor("_Color", specularColor);
			choiceDisplay.renderer.material.SetColor("_SpecColor", specularColor);
		}
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
