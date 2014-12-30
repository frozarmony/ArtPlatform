using UnityEngine;
using System.Collections;
using Leap;

public class PriestTracker : GestureTracker {
	
	/****************
	 *   Constants  *
	 ****************/
	
	private const int CONDITION_COUNT		= 5;
	private const float TRESHOLD_COEF		= 0.1f;
	
	/****************
	 *  References  *
	 ****************/
	
	private HandManager rightHand;
	private HandManager leftHand;
	private int meetedConditionCount;
	private bool activated;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public PriestTracker(MainManager manager, HandManager leftHandRef, HandManager rightHandRef) : base(manager){
		this.rightHand = rightHandRef;
		this.leftHand = leftHandRef;
		this.meetedConditionCount = 0;
		this.activated = false;
	}
	
	/******************
	 * Implementation *
	 ******************/

	public override void OnLoad(){
		this.activated = false;
		this.meetedConditionCount = 0;
	}
	
	public override void OnUpdate(){
		if (!rightHand.IsSynchronized () || !leftHand.IsSynchronized() ) {											// Handle Reset On Hand Desynchronisation
			meetedConditionCount = 0;
		}
		else if(!activated){
			// Compute Priest Coef
			float priestCoef = 0f;
			for(int i=0; i<HandManager.HAND_ANCHOR_COUNT; ++i){
				priestCoef += Vector3.Distance( leftHand.GetAnchor(i).position, rightHand.GetAnchor(i).position );
			}
			priestCoef /= 100f;

			if( priestCoef < TRESHOLD_COEF ){
				if(meetedConditionCount < CONDITION_COUNT){
					++meetedConditionCount;
				}
				else{
					// Perform Priest Action
					manager.LoadMenu("InterractionMenu");
					activated = true;
					meetedConditionCount = 0;
				}
			}
		}
	}
	
}