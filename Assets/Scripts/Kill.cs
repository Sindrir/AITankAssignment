using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kill : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public bool canShootTarget;
    [SerializeField] public bool canSeeTarget;
    [SerializeField] public GameObject enemy;
    public GraphNode lastSeen;
    public GraphNode currentNode;

    private Complete.TankMovement enemyMovementScript;
    private Complete.TankMovement myMovementScript;
    private Kill enemyKillScript;
    private  FieldOfView thisTanksView;
    private AIFire fireScript;
    void Start()
    {
        lastSeen = null;
        canShootTarget = false;
        canSeeTarget = false;
        thisTanksView = GetComponent<FieldOfView>();
        fireScript = GetComponent<AIFire>();
        enemyMovementScript = enemy.GetComponent<Complete.TankMovement>();
        enemyKillScript = enemy.GetComponent<Kill>();
        myMovementScript = GetComponent<Complete.TankMovement>();
    }    

    // Update is called once per frame
    void Update()
    {
        if(myMovementScript.enabled)currentNode = myMovementScript.LastNode2();
       canShootTarget = false;
       for (int i = 0; i < thisTanksView.shootableTargets.Count; i++)
           if(thisTanksView.shootableTargets[i].gameObject == enemy )
                canShootTarget = true;

       canSeeTarget = false;
       for (int i = 0; i < thisTanksView.visibleTargets.Count; i++)
           if(thisTanksView.visibleTargets[i].gameObject == enemy )
                canSeeTarget = true;
       
       if (canShootTarget)              //USE AIFire script
            fireScript.holdDown = true;
        else
            fireScript.holdDown = false;

       if(canSeeTarget) // Use the enemy's TankMovement Script to get last visited node
            lastSeen = enemyKillScript.currentNode;
       /*
            lastSeen = enemyMovementScript.LastNode2();
            if(enemyChaseScript.enabled) lastSeen = enemyChaseScript.currentNode;
            */
    }
}
