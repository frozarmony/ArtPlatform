using UnityEngine;
using System.Collections;

public class MaterialProjectorItem  : HandItem {
	
	/****************
	 *  References  *
	 ****************/

	private Transform sphere;
	
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

		// Load new material
		materialInstance.localRotation = sphere.rotation;
		materialInstance.parent = sphere;
		materialInstance.localPosition = Vector3.zero;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	protected override void OnLoaded(){}
	protected override void OnUnloaded(){}
}
