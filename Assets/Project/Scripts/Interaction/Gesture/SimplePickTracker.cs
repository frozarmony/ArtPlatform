﻿using UnityEngine;
using System.Collections;
using Leap;

public class SimplePickTracker : GestureTracker {
	
	/****************
	 *   Constants  *
	 ****************/
	
	public const int CONDITION_COUNT		= 5;
	public const float PICK_COEF			= 0.1f;
	public const float DROP_COEF			= 0.6f;
	
	/****************
	 *  References  *
	 ****************/
	
	private HandManager rightHand;
	private int meetedConditionCount;
	private GameObject pickedElement;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public SimplePickTracker(MainManager manager, HandManager rightHandRef) : 
	base(manager, new HelpMessage[]{new HelpMessage("Pick objets with right hand.",manager.clickHelpTexture)}){
		this.rightHand = rightHandRef;
		this.pickedElement = null;
		this.meetedConditionCount = 0;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnUpdate(){
		ArtMaterial mat = manager.GetCurrentSimplePickMaterial ();
		if( mat != null)
			if (!rightHand.IsSynchronized ()) {
				if(pickedElement != null)
					Object.Destroy(pickedElement);
				pickedElement = null;
				meetedConditionCount = 0;
			}
			else{
				if (pickedElement == null) {
					if(rightHand.PickingCoef() < PICK_COEF){
						if(meetedConditionCount < CONDITION_COUNT){
							++meetedConditionCount;
						}
						else{
							pickedElement = (GameObject)Object.Instantiate(mat.gameObject);
							UpdatePickedElement();
							meetedConditionCount = 0;
						}
					}
				}
				else{
					UpdatePickedElement();
					if(rightHand.PickingCoef() > DROP_COEF){
						if(meetedConditionCount < CONDITION_COUNT){
							++meetedConditionCount;
						}
						else{
							manager.DoAction(new SimplePickAction(manager, mat, pickedElement.transform.position, pickedElement.transform.rotation));
							Object.Destroy(pickedElement);
							pickedElement = null;
							meetedConditionCount = 0;
						}
					}
				}
			}
	}

	/******************
	 *  Tool Methods  *
	 ******************/

	private void UpdatePickedElement(){
		Transform thumb = rightHand.GetAnchor (HandManager.HAND_ANCHOR_THUMB);
		Vector3 pointA = rightHand.GetAnchor (HandManager.HAND_ANCHOR_INDEX).position;
		Vector3 pointB = thumb.position;
		Vector3 midPoint = new Vector3((pointA.x+pointB.x)/2.0f,(pointA.y+pointB.y)/2.0f,(pointA.z+pointB.z)/2.0f);
		pickedElement.transform.position = midPoint;
		pickedElement.transform.rotation = thumb.rotation;
	}

}
