using UnityEngine;
using System.Collections;

public class ButtonTrigger : MonoBehaviour {
	
	/****************
	 *   Constants  *
	 ****************/

	private static Shader SHADER_FOCUS_OFF	= Shader.Find("Diffuse");
	private static Shader SHADER_FOCUS_ON	= Shader.Find("Self-Illumin/Diffuse");

	/****************
	 *  References  *
	 ****************/

	private MainManager manager;
	private int handAnchorId;
	
	/******************
	 * Initialization *
	 ******************/

	public void InitButtonTrigger(MainManager mainManager, int anchorId){
		// Init References
		manager = mainManager;
		handAnchorId = anchorId;

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
	 *  Tool Methods  *
	 ******************/

	public void SetFocus(bool focusOn){
		if (focusOn) {
			this.renderer.material.shader = SHADER_FOCUS_ON;
		} else {
			this.renderer.material.shader = SHADER_FOCUS_OFF;
		}
	}

}
