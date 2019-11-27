using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public bool seesTarget;
    [SerializeField] public GameObject enemy;
    private  FieldOfView thisTanksView;
    void Start()
    {
        seesTarget = false;
        thisTanksView = GetComponent<FieldOfView>();
    }    

    // Update is called once per frame
    void Update()
    {
      
       
      //  thisTanksView = GetComponent<FieldOfView>();
       seesTarget = false;
       for (int i = 0; i < thisTanksView.visibleTargets.Count; i++){
           if(thisTanksView.visibleTargets[i].gameObject == enemy ) seesTarget = true;
       }
        
    }
}
