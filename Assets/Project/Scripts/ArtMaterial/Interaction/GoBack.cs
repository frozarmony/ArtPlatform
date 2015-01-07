using UnityEngine;
using System.Collections;

public class GoBack : AbstractInterraction {

	/****************
	 *  References  *
	 ****************/
	
	private Vector3 originPosition;
	private Quaternion originRotation;
	private Quaternion endRotation;
	private float fractionRotation;

	/******************
	 *  Constructor   *
	 ******************/
	
	public GoBack() : base(){
		originPosition = Vector3.zero;
		originRotation = Quaternion.identity;
	}
	
	/******************
	 * Implementation *
	 ******************/

	protected override void OnLoad (MainManager manager){
		// Check necessary components
		if(this.collider == null)
			Debug.LogError("Error : This ArtMaterial (" + this + ") must have a collider to use GoBack Script");
		
		// Save origin transform
		originPosition = this.transform.localPosition;
		originRotation = this.transform.localRotation;

		// Load necessary components
		this.gameObject.AddComponent<Rigidbody> ();
		rigidbody.useGravity = false;
		this.collider.enabled = true;
	}

	protected override void OnUpdate (){
		if(this.transform.localPosition != originPosition || this.transform.rotation!= originRotation){
			// Translate
			Vector3 idealTranslateVelocity = originPosition - transform.localPosition;
			Vector3 force = idealTranslateVelocity - rigidbody.velocity;
			rigidbody.AddForce(force * 0.5f);

			// Rotation
			float distance = idealTranslateVelocity.sqrMagnitude;
			if(distance > 0.1f){
				fractionRotation = 0;
				endRotation = transform.localRotation;
			}
			else if(fractionRotation < 1f){
				fractionRotation += 0.001f;
				transform.localRotation = Quaternion.Lerp(endRotation, originRotation, fractionRotation);
			}
			else{
				// Reset origin transform
				this.transform.localPosition = originPosition;
				this.transform.localRotation = originRotation;
			}
			/*Vector3 idealAngularVelocity = originRotation - transform.rotation;
			Vector3 torque = idealAngularVelocity - rigidbody.angularVelocity;
			rigidbody.AddRelativeTorque(torque * 0.1f);*/
		}
	}

	protected override void OnUnload (){
		// Unload necessary components
		Destroy(this.gameObject.GetComponent<Rigidbody>());
		this.collider.enabled = false;

		// Reset origin transform
		this.transform.localPosition = originPosition;
		this.transform.localRotation = originRotation;
	}


}
