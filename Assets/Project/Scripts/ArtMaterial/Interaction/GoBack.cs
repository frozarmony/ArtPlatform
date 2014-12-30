using UnityEngine;
using System.Collections;

public class GoBack : AbstractInterraction {

	/****************
	 *  References  *
	 ****************/
	
	private Vector3 originPosition;
	private Quaternion originRotation;

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
		originPosition = this.transform.position;
		originRotation = this.transform.rotation;

		// Load necessary components
		this.gameObject.AddComponent<Rigidbody> ();
		rigidbody.useGravity = false;
		this.collider.enabled = true;
	}

	protected override void OnUpdate (){
		if(this.transform.position != originPosition || this.transform.rotation!= originRotation){
			// Translate
			Vector3 idealTranslateVelocity = originPosition - transform.position;
			Vector3 force = idealTranslateVelocity - rigidbody.velocity;
			rigidbody.AddForce(force * 0.5f);

			// Rotation
			/*Quaternion.
			Vector3 idealAngularVelocity = originRotation transform.rotation;
			Vector3 torque = idealAngularVelocity - rigidbody.angularVelocity;
			rigidbody.AddRelativeTorque(torque * 0.1f);*/
		}
	}

	protected override void OnUnload (){
		// Unload necessary components
		Destroy(this.gameObject.GetComponent<Rigidbody>());
		this.collider.enabled = false;

		// Reset origin transform
		this.transform.position = originPosition;
		this.transform.rotation = originRotation;
	}


}
