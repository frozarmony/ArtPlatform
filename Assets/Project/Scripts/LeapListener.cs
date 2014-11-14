using UnityEngine;
using System.Collections;
using Leap;

public class LeapListener : Listener {
	
	public Rigidbody rocket;

	// On Init
	override public void OnInit(Controller ctrl){
		Debug.Log("Hi");
		ctrl.EnableGesture (Gesture.GestureType.TYPESCREENTAP);
	}
	
	// On new frame
	public void OnNewFrame (Controller ctrl) {
		if(ctrl.Frame().Gestures().Count > 0){
			Gesture gest = ctrl.Frame().Gestures()[0];
			
			if(gest.Type == Gesture.GestureType.TYPESCREENTAP){
				ScreenTapGesture typeGest = new ScreenTapGesture(gest);
				if(rocket == null){
					Debug.Log("No Rocket");
				}
				else{
					Rigidbody rgd = (Rigidbody)Object.Instantiate(rocket, Vector3.forward, Quaternion.identity);
					rgd.MovePosition(typeGest.Pointable.TipPosition.ToUnityScaled()*25f);
					rgd.velocity = 10*typeGest.Pointable.Direction.ToUnity();
				}
			}
		}
	}
}
