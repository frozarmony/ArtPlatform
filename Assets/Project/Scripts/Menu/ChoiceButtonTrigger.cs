using UnityEngine;
using System.Collections;

public class ChoiceButtonTrigger : ButtonTrigger {
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void SetSelected(bool selected){
		isSelected = selected;
		this.GetComponent<Animator>().SetBool("selected", isSelected);
	}
	
	protected override void SetFocus(bool focusOn){
		// Do Nothing
	}
}
