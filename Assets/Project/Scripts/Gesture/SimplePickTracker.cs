using UnityEngine;
using System.Collections;
using Leap;

public class SimplePickTracker : GestureTracker {
	
	/****************
	 *   Constants  *
	 ****************/
	
	public const int CONDITION_COUNT		= 10;
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
	
	public SimplePickTracker(MainManager manager, HandManager hand) : base(manager){
		this.rightHand = hand;
		this.pickedElement = null;
		this.meetedConditionCount = 0;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnUpdate(){
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
						pickedElement = (GameObject)Object.Instantiate(manager.matTest);
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
						manager.DoAction(new SimplePickAction(manager, manager.matTest, pickedElement.transform.position));
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
		Vector3 pointA = rightHand.GetAnchor (HandManager.HAND_ANCHOR_INDEX).position;
		Vector3 pointB = rightHand.GetAnchor (HandManager.HAND_ANCHOR_THUMB).position;
		Vector3 midPoint = new Vector3((pointA.x+pointB.x)/2.0f,(pointA.y+pointB.y)/2.0f,(pointA.z+pointB.z)/2.0f);
		pickedElement.transform.position = midPoint;
	}

}
