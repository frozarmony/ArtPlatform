using UnityEngine;
using System.Collections;
using Leap;

public class MoveTracker : GestureTracker {
	
	/****************
	 *   Constants  *
	 ****************/
	
	private const int CONDITION_COUNT		= 5;
	private const float TRESHOLD_COEF		= 0.4f;
	
	/****************
	 *  References  *
	 ****************/

	private HandManager rightHand;
	private HandManager leftHand;
	private int meetedConditionCount;
	private bool isTracking;
	private Vector3 lastRightHandPosition;
	private Vector3 lastLeftHandPosition;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public MoveTracker(MainManager manager, HandManager leftHandRef, HandManager rightHandRef) : base(manager){
		this.rightHand = rightHandRef;
		this.leftHand = leftHandRef;
		this.lastRightHandPosition = Vector3.zero;
		this.lastLeftHandPosition = Vector3.zero;
		this.isTracking = false;
		this.meetedConditionCount = 0;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnUpdate(){
		if (!rightHand.IsSynchronized () || !leftHand.IsSynchronized() ) {											// Handle Reset On Hand Desynchronisation
			lastRightHandPosition = Vector3.zero;
			lastLeftHandPosition = Vector3.zero;
			isTracking = false;
			meetedConditionCount = 0;
		}
		else{
			float openingCoef = (leftHand.OpeningCoef() + rightHand.OpeningCoef()) / 2.0f;
			if (!isTracking) {														// Handle Start of Gesture
				if(openingCoef < TRESHOLD_COEF){
					if(meetedConditionCount < CONDITION_COUNT){
						++meetedConditionCount;
					}
					else{
						lastRightHandPosition = rightHand.GetAnchor(HandManager.HAND_ANCHOR_PALM).position;
						lastLeftHandPosition = leftHand.GetAnchor(HandManager.HAND_ANCHOR_PALM).position;
						meetedConditionCount = 0;
						isTracking = true;
					}
				}
				else{																// Reset Condition Count
					meetedConditionCount = 0;
				}
			}
			else{																	// Handle Moving
				// Init Useful Vars
				Vector3 lastMiddlePoint = ComputeMiddlePoint(lastLeftHandPosition, lastRightHandPosition);
				Vector3 lastRotVect = lastMiddlePoint - lastLeftHandPosition;

				Vector3 newRightHandPosition = rightHand.GetAnchor(HandManager.HAND_ANCHOR_PALM).position;
				Vector3 newLeftHandPosition = leftHand.GetAnchor(HandManager.HAND_ANCHOR_PALM).position;
				Vector3 newMiddlePoint = ComputeMiddlePoint(newLeftHandPosition, newRightHandPosition);
				Vector3 newRotVect = newMiddlePoint - newLeftHandPosition;

				// Compute Translation
				Vector3 translation = lastMiddlePoint - newMiddlePoint;

				// Compute Rotation infos
				float angle = 0f;
				Vector3 axis = Vector3.zero;
				Quaternion.FromToRotation(lastRotVect, newRotVect).ToAngleAxis(out angle, out axis);

				// Move
				manager.GetCurrentCanvas().Translate(-translation, Space.World);
				manager.GetCurrentCanvas().RotateAround(newMiddlePoint, axis, angle);
					
				// Update Last Positions
				lastRightHandPosition = newRightHandPosition;
				lastLeftHandPosition = newLeftHandPosition;
				
				if(openingCoef > TRESHOLD_COEF){							// Handle End of Gesture
					if(meetedConditionCount < CONDITION_COUNT){
						++meetedConditionCount;
					}
					else{
						lastRightHandPosition = Vector3.zero;
						lastLeftHandPosition = Vector3.zero;
						isTracking = false;
						meetedConditionCount = 0;
					}
				}
				else{																// Reset Condition Count
					meetedConditionCount = 0;
				}
			}
		}
	}
	
	/******************
	 *  Tool Methods  *
	 ******************/

	private Vector3 ComputeMiddlePoint(Vector3 a, Vector3 b){
		return new Vector3 ((a.x + b.x) / 2f, (a.y + b.y) / 2f, (a.z + b.z) / 2f);
	}
	
}