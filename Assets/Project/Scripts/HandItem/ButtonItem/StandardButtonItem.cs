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
		if (focusOn) {
			this.renderer.material.shader = SHADER_FOCUS_ON;
			this.renderer.material.color = COLOR_FOCUS_ON;
		} else if(!isSelected) {
			this.renderer.material.shader = SHADER_FOCUS_OFF;
			this.renderer.material.color = COLOR_FOCUS_OFF;
		}
	}

}
