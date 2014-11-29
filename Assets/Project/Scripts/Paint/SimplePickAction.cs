using UnityEngine;
using System.Collections;

public class SimplePickAction : AbstractAction {
	
	/****************
	 *  References  *
	 ****************/

	private GameObject matPrefab;
	private Vector3 pickPos;
	private GameObject matInstance;

	/******************
	 *  Constructor   *
	 ******************/
	
	public SimplePickAction(MainManager manager, GameObject matPrefabRef, Vector3 pickPosRef) : base(manager){
		matPrefab = matPrefabRef;
		pickPos = pickPosRef;
		matInstance = null;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void Do(){
		if (matInstance != null)
			Debug.LogError ("Impossible to do SimplePickAction : it's already done.");
		else
			matInstance = manager.PaintOnCanvas (matPrefab, pickPos, Quaternion.identity);
	}
	
	public override void Undo(){
		if (matInstance == null)
			Debug.LogError ("Impossible to undo SimplePickAction : it's not already done.");
		else
			matInstance = manager.PaintOnCanvas (matPrefab, pickPos, Quaternion.identity);
	}

}
