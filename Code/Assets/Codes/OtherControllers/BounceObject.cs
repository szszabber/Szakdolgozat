using UnityEngine;
using UnityEngine.Events;

public class BounceObject : MonoBehaviour
{
    public float velocityMax = 100;
    public float velocityMin = -100;

    public float xMax;
    public float yMax;
    public float xMin;
    public float yMin;

    private float x;
    private float y;
    private float movementSpeed = 2f;

    void Start()
    {
        x = Random.Range(velocityMin, velocityMax);
        y = Random.Range(velocityMin, velocityMax);
    }

    void Update()
    {
        if (transform.localPosition.x > xMax)
        {
            x = Random.Range(velocityMin, 0.0f); ;
            movementSpeed = 0.0f;
        }
        if (transform.localPosition.x < xMin)
        {
            x = Random.Range(0.0f, velocityMax);
            movementSpeed = 0.0f;
        }
        if (transform.localPosition.y > yMax)
        {
            y = Random.Range(velocityMin, 0.0f); ;
            movementSpeed = 0.0f;
        }
        if (transform.localPosition.y < yMin)
        {
            y = Random.Range(0.0f, velocityMax);
            movementSpeed = 0.0f;
        }

        if (movementSpeed > 0.1f)
        {
            x = Random.Range(velocityMin, velocityMax);
            y = Random.Range(velocityMin, velocityMax);
            movementSpeed = 0.02f;
        }

        transform.localPosition = new Vector3(transform.localPosition.x + x, transform.localPosition.y + y, 0f);
    }
}


//using UnityEngine;
//using System.Collections;

//public class BounceObject : MonoBehaviour
//{
//    public bool animPos = true;
//    public Vector3 posAmplitude = Vector3.one;
//    public Vector3 posSpeed = Vector3.one;

//    public bool animRot = true;
//    public Vector3 rotAmplitude = Vector3.one * 20;
//    public Vector3 rotSpeed = Vector3.one;

//    public bool animScale = false;
//    public Vector3 scaleAmplitude = Vector3.one * 0.1f;
//    public Vector3 scaleSpeed = Vector3.one;

//    private Vector3 origPos;
//    private Vector3 origRot;
//    private Vector3 origScale;

//    private float startAnimOffset = 0;

//    /**
//     * Awake
//     */
//    void Awake()
//    {
//        origPos = transform.position;
//        origRot = transform.eulerAngles;
//        origScale = transform.localScale;
//        startAnimOffset = Random.Range(0f, 540f);        // so that the xyz anims are already offset from each other since the start
//    }

//    /**
//     * Update
//     */
//    void Update()
//    {

//        /* position */
//        if (animPos)
//        {
//            Vector3 pos;
//            pos.x = origPos.x + posAmplitude.x * Mathf.Sin(posSpeed.x * Time.time + startAnimOffset);
//            pos.y = origPos.y + posAmplitude.y * Mathf.Sin(posSpeed.y * Time.time + startAnimOffset);
//            pos.z = origPos.z + posAmplitude.z * Mathf.Sin(posSpeed.z * Time.time + startAnimOffset);
//            transform.position = pos;
//        }

//        /* rotation */
//        if (animRot)
//        {
//            Vector3 rot;
//            rot.x = origRot.x + rotAmplitude.x * Mathf.Sin(rotSpeed.x * Time.time + startAnimOffset);
//            rot.y = origRot.y + rotAmplitude.y * Mathf.Sin(rotSpeed.y * Time.time + startAnimOffset);
//            rot.z = origRot.z + rotAmplitude.z * Mathf.Sin(rotSpeed.z * Time.time + startAnimOffset);
//            transform.eulerAngles = rot;
//        }

//        /* scale */
//        if (animScale)
//        {
//            Vector3 scale;
//            scale.x = origScale.x * (1 + scaleAmplitude.x * Mathf.Sin(scaleSpeed.x * Time.time + startAnimOffset));
//            scale.y = origScale.y * (1 + scaleAmplitude.y * Mathf.Sin(scaleSpeed.y * Time.time + startAnimOffset));
//            scale.z = origScale.z * (1 + scaleAmplitude.z * Mathf.Sin(scaleSpeed.z * Time.time + startAnimOffset));
//            transform.localScale = scale;
//        }
//    }
//}
