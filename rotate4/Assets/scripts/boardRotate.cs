using UnityEngine;
using System.Collections;

public class boardRotate : MonoBehaviour {


    public float rotationInterval;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void rotate()
    {
        transform.Rotate(new Vector3(0f, 0f, rotationInterval));
        Debug.Log(transform.eulerAngles.y);
        if ((Mathf.Abs(transform.eulerAngles.y) > 90) && (Mathf.Abs(transform.eulerAngles.y) < 270))
        {
            rotationInterval = 0;
            /*transform.eulerAngles = new Vector3(
                Mathf.Floor(transform.eulerAngles.x),
                Mathf.Floor(transform.eulerAngles.y),
                Mathf.Floor(transform.eulerAngles.z));*/
            transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }
}
