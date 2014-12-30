using UnityEngine;
using System.Collections;

public class SimplePickAction : AbstractAction {
	
	/****************
	 *  References  *
	 ****************/

	private ArtMaterial mat;
	private Vector3 pickPos;
	private Quaternion pickRot;
	private GameObject matInstance;

	/******************
	 *  Constructor   *
	 ******************/
	
	public SimplePickAction(MainManager manager, ArtMaterial matRef, Vector3 pickPosRef, Quaternion pickRotRef) : base(manager){
		mat = matRef;
		pickPos = pickPosRef;
		pickRot = pickRotRef;
		matInstance = null;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void Do(){
		if (matInstance != null)
			Debug.LogError ("Impossible to do SimplePickAction : it's already done.");
		else
			matInstance = manager.PaintOnCanvas (mat.gameObject, pickPos, pickRot);
	}
	
	public override void Undo(){
		if (matInstance == null)
			Debug.LogError ("Impossible to undo SimplePickAction : it's not already done.");
		else {
			Object.Destroy(matInstance);
			matInstance = null;
		}
	}

}
