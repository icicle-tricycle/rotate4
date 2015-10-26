using UnityEngine;
using System.Collections;

public class startScreenSpin : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * Time.deltaTime * 10);
		//transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
	}
}
