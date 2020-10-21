using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float timeCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        timeCounter += Time.deltaTime;
        float x = Mathf.Cos(timeCounter);
        float y = Mathf.Sin(timeCounter);
        transform.position = new Vector2(x, y);
    }
}
