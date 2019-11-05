using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * 10f * deltaTime;
    }

    private void FixedUpdate()
    {
        gameObject.transform.position = gameObject.transform.position + gameObject.transform.forward * 1f; 
    }
}
