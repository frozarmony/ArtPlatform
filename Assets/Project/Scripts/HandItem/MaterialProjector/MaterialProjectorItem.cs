using UnityEngine;
using System.Collections;

public class MaterialProjectorItem  : HandItem {
	
	/****************
	 *  References  *
	 ****************/

	private Transform sphere;
	private Transform preview;
	
	/******************
	 * Initialization *
	 ******************/
	
	public override void InitHandItem(MainManager mainManager, int anchorId){
		base.InitHandItem (mainManager, anchorId);
		
		// Init References
		sphere = transform.GetChild (0);
	}
	
	/*******************
	 *  Tools Methods  *
	 *******************/

	public void LoadMaterial(Transform materialInstance){
		// Unload old material
		sphere.DetachChildren ();

		preview = materialInstance;

		// Load new material
		if(IsLoaded()){
			preview.localRotation = sphere.rotation;
			preview.parent = sphere;
			preview.localPosition = Vector3.zero;
		}
		else
			preview.localScale = Vector3.zero;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	protected override void OnLoaded(){
		// Finish Preview Loading if needed
		if (preview != null) {
			preview.localScale = Vector3.one;
			preview.localRotation = sphere.rotation;
			preview.parent = sphere;
			preview.localPosition = Vector3.zero;
		}
	}

	protected override void OnUnloaded(){}
}
