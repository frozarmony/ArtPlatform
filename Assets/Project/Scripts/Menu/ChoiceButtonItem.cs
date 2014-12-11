using UnityEngine;
using System.Collections;

public class ChoiceButtonItem : ButtonItem {
	
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
