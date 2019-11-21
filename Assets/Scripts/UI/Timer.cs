using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;
        text.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        text.text = timer.ToString("F");
    }
}
