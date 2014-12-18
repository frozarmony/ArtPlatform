using UnityEngine;
using System.Collections;

public class PaintTracker : GestureTracker {
	
	/****************
	 *   Constants  *
	 ****************/
	
	public const int CONDITION_COUNT		= 5;
	public const float TRESHOLD_COEF		= 0.7f;
	
	/****************
	 *  References  *
	 ****************/
	
	private HandManager controlHand;
	private HandManager paintHand;
	private int meetedConditionCount;
	private PaintAction paintTrace;
	private float lastPointSize;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public PaintTracker(MainManager manager, HandManager leftHandRef, HandManager rightHandRef) : base(manager){
		this.controlHand = leftHandRef;
		this.paintHand = rightHandRef;
		this.paintTrace = null;
		this.meetedConditionCount = 0;
		this.lastPointSize = 0;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	public override void OnUpdate(){
		ArtPaintingMaterial mat = (ArtPaintingMaterial)manager.GetCurrentPaintingMaterial ();
		if( mat != null)
			if (!controlHand.IsSynchronized () || !paintHand.IsSynchronized()) {	// Handle Reset On Hand Desynchronisation
				if(paintTrace != null && paintTrace.IsDrawing())
					paintTrace.CancelDrawing();
				paintTrace = null;
				meetedConditionCount = 0;
			}
			else{
				float openingCoef = controlHand.OpeningCoef();
				if (paintTrace == null) {											// Handle Start of Gesture
					if(openingCoef < TRESHOLD_COEF){
						if(meetedConditionCount < CONDITION_COUNT){
							++meetedConditionCount;
						}
						else{
							paintTrace = new PaintAction(manager, mat);
							paintTrace.StartDrawing();
							meetedConditionCount = 0;
							lastPointSize = TRESHOLD_COEF - openingCoef;
						}
					}
				}
				else{																// Handle Drawing
					paintTrace.Draw(paintHand.GetAnchor(HandManager.HAND_ANCHOR_INDEX).position, lastPointSize);
					lastPointSize = TRESHOLD_COEF - openingCoef;

					if(controlHand.OpeningCoef() > TRESHOLD_COEF){					// Handle End of Gesture
						if(meetedConditionCount < CONDITION_COUNT){
							++meetedConditionCount;
						}
						else{
							manager.DoAction(paintTrace);
							paintTrace.EndDrawing();
							paintTrace = null;
							meetedConditionCount = 0;
						}
					}
				}
			}
	}

}
