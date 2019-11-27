using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public bool seesTarget;
    [SerializeField] public GameObject enemy;
    private  FieldOfView thisTanksView;
    private AIFire fireScript;
    void Start()
    {
        seesTarget = false;
        thisTanksView = GetComponent<FieldOfView>();
        fireScript = GetComponent<AIFire>();
    }    

    // Update is called once per frame
    void Update()
    {
      
       
      //  thisTanksView = GetComponent<FieldOfView>();
       seesTarget = false;
       for (int i = 0; i < thisTanksView.shootableTargets.Count; i++){
           if(thisTanksView.shootableTargets[i].gameObject == enemy ) seesTarget = true;
       }


       if (seesTarget){
       /*USE AIFire script*/
            fireScript.holdDown = true;
       }else fireScript.holdDown = false;
        
    }
}
