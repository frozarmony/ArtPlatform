using UnityEngine;
using System.Collections;

public class LookAtCamera : MonoBehaviour {
	
	/****************
	 *  References  *
	 ****************/

	public Transform targetCamera;
	
	/*****************
	 *    Update     *
	 *****************/

	void Update () {
		this.transform.LookAt (targetCamera.position);
	}
}
