using UnityEngine;
using System.Collections;

public class StandardButtonItem : ButtonItem {
	
	/****************
	 *   Constants  *
	 ****************/

	private static Shader SHADER_FOCUS_OFF		= Shader.Find("Transparent/Diffuse");
	private static Shader SHADER_FOCUS_ON		= Shader.Find("Transparent/Specular");
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void SetSelected(bool selected){
		isSelected = selected;
		SetFocus (isSelected);
	}
	
	protected override void SetFocus(bool focusOn){
		// Do Nothing
	}

}
