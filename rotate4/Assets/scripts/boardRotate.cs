using UnityEngine;
using System.Collections;

public class boardRotate : MonoBehaviour {


    private float rotationInterval;
    public float RotationInterval
    {
        get { return rotationInterval; }
        set
        {
            rotationInterval = value;
            startAngle = currentAngle = transform.eulerAngles.y;
            targetAngle = startAngle + (Mathf.Sign(rotationInterval) * 90);
            //if(targetAngle)
            Debug.Log("rotation interval changed, start angle: " + startAngle + " target: " + targetAngle + " rot interval: " + rotationInterval);
        }
    }

    /// <summary>
    /// the angle the board started at before it's rotation.
    /// </summary>
    private float startAngle;
    /// <summary>
    /// This is used as a placeholder to check logic, used because it does not automatically loop above 360, and below 0
    /// </summary>
    private float currentAngle;
    private float targetAngle;

	// Use this for initialization
	void Start () {
        currentAngle = transform.eulerAngles.y;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void rotate()
    {
        currentAngle += rotationInterval;
        //Debug.Log(transform.eulerAngles.y);
        Debug.Log("rotation interval: " + rotationInterval + "| cw angle case: " + (currentAngle < targetAngle) + "| ccw angle case: " + (currentAngle > targetAngle));
        //Debug.Log("cw angle case: " + (currentAngle < targetAngle));
        //Debug.Log("ccw angle case: " + (currentAngle > targetAngle));
        if(
            //cw case
            //(rotationInterval < 0) && (Mathf.Abs(transform.eulerAngles.y) >= angleConvert(90+startAngle))
            ((rotationInterval < 0) && (currentAngle < targetAngle))
            ||
            //ccw case
            //(rotationInterval > 0) && (Mathf.Abs(transform.eulerAngles.y) >= angleConvert(270 + startAngle))
            ((rotationInterval > 0) && (currentAngle > targetAngle))
          )
        {
            //transform.eulerAngles = new Vector3(90, 0, startAngle + 90 * Mathf.Abs(rotationInterval));
            rotationInterval = 0;

        }
        transform.Rotate(new Vector3(0f, 0f, rotationInterval));
        transform.eulerAngles = new Vector3(
            Mathf.Round(transform.eulerAngles.x),
            Mathf.Round(transform.eulerAngles.y),
            Mathf.Round(transform.eulerAngles.z)
            );



        /*if ((Mathf.Abs(transform.eulerAngles.y) > 90) && (Mathf.Abs(transform.eulerAngles.y) < 270))
        {
            rotationInterval = 0;
            /*transform.eulerAngles = new Vector3(
                Mathf.Floor(transform.eulerAngles.x),
                Mathf.Floor(transform.eulerAngles.y),
                Mathf.Floor(transform.eulerAngles.z));*/
            //transform.eulerAngles = new Vector3(90, 0, 0);
        //}*/
    }

    private float angleConvert(float input)
    {
        if (input > 360)
            return angleConvert(input - 360);
        else if (input < 0)
            return angleConvert(input + 360);
        return (input);

    }
}
