using UnityEngine;
using System.Collections;

public abstract class ButtonTrigger : MonoBehaviour {
	
	/****************
	 *   Constants  *
	 ****************/

	public static Color COLOR_FOCUS_OFF		= Color.white;
	public static Color COLOR_FOCUS_ON		= new Color(100,255,255);

	/****************
	 *  References  *
	 ****************/

	protected MainManager manager;
	protected int handAnchorId;
	protected bool isSelected;
	
	/******************
	 * Initialization *
	 ******************/

	public void InitButtonTrigger(MainManager mainManager, int anchorId){
		// Init References
		manager = mainManager;
		handAnchorId = anchorId;
		isSelected = false;

		// Set Focus Off
		SetFocus(false);
	}
	
	/******************
	 *    Getters     *
	 ******************/

	public int GetHandHanchorId(){
		return handAnchorId;
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

}
