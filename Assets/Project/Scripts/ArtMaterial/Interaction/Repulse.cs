using UnityEngine;
using System.Collections;

public class Repulse : AbstractInterraction {
	
	/****************
	 *  References  *
	 ****************/
	
	private Vector3 originPosition;
	private HandManager rightHand;
	
	/******************
	 *  Constructor   *
	 ******************/
	
	public Repulse() : base(){
		originPosition = Vector3.zero;
		rightHand = null;
	}
	
	/******************
	 * Implementation *
	 ******************/
	
	protected override void OnLoad (MainManager manager){
		// Ref Manager
		rightHand = manager.GetRightHand();
		
		// Save origin transform
		originPosition = this.transform.localPosition;

		// Load necessary components
		this.gameObject.AddComponent<Rigidbody> ();
		rigidbody.useGravity = false;
	}
	
	protected override void OnUpdate (){
		// Default MagnetCenter
		Vector3 magnetCenter = originPosition;

		// RightHand Magnet Computation
		if (rightHand.IsSynchronized ()) {
			magnetCenter = rightHand.GetAnchor(HandManager.HAND_ANCHOR_PALM).position - originPosition;
			float dist = magnetCenter.sqrMagnitude;
			if(dist != 0){
				magnetCenter = originPosition + magnetCenter * (-3f / dist);
			}
		}

		// Update Force
		Vector3 idealTranslateVelocity = magnetCenter - transform.localPosition;
		Vector3 force = idealTranslateVelocity - rigidbody.velocity;
		rigidbody.AddForce(force*3f);
	}
	
	protected override void OnUnload (){
		// Deref Manager
		rightHand = null;
		
		// Unload necessary components
		Destroy(this.gameObject.GetComponent<Rigidbody>());
		
		// Reset origin transform
		this.transform.localPosition = originPosition;
	}
	
}
