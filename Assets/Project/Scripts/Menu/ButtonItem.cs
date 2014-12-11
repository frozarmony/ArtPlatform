using UnityEngine;
using System.Collections;

public abstract class ButtonItem : HandItem {
	
	/****************
	 *   Constants  *
	 ****************/

	public static Color COLOR_FOCUS_OFF		= Color.white;
	public static Color COLOR_FOCUS_ON		= new Color(100,255,255);

	/****************
	 *  References  *
	 ****************/

	protected bool isSelected;
	
	/******************
	 * Initialization *
	 ******************/

	public override void InitHandItem(MainManager mainManager, int anchorId){
		base.InitHandItem (mainManager, anchorId);

		// Init References
		isSelected = false;

		// Set Focus Off
		SetFocus(false);
	}
	
	/******************
	 *    Triggers    *
	 ******************/

	void OnTriggerEnter(Collider collid){
		if (collid.tag == "BoneTriggerer") {
			SetFocus(true);
		}
	}

	void OnTriggerExit(Collider collid){
		if (collid.tag == "BoneTriggerer") {
			SetFocus(false);
			manager.NotifyButtonPush (handAnchorId);
		}
	}
	
	/******************
	 *  Declaration   *
	 ******************/

	public abstract void SetSelected (bool selected);
	protected abstract void SetFocus (bool focusOn);

	protected override void OnLoaded(){}
	protected override void OnUnloaded(){}

}
