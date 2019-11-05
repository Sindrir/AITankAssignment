using System;
using System.Collections.Generic;
using UnityEngine;

namespace Complete
{
    public class TankMovement : MonoBehaviour
    {
        public int m_PlayerNumber = 1;              // Used to identify which tank belongs to which player.  This is set by this tank's manager.
        public float m_Speed = 12f;                 // How fast the tank moves forward and back.
        public float m_TurnSpeed = 180f;            // How fast the tank turns in degrees per second.
        public AudioSource m_MovementAudio;         // Reference to the audio source used to play engine sounds. NB: different to the shooting audio source.
        public AudioClip m_EngineIdling;            // Audio to play when the tank isn't moving.
        public AudioClip m_EngineDriving;           // Audio to play when the tank is moving.
        public float m_PitchRange = 0.2f;           // The amount by which the pitch of the engine noises can vary.

        private string m_MovementAxisName;          // The name of the input axis for moving forward and back.
        private string m_TurnAxisName;              // The name of the input axis for turning.
        private Rigidbody m_Rigidbody;              // Reference used to move the tank.
        private float m_MovementInputValue;         // The current value of the movement input.
        private float m_TurnInputValue;             // The current value of the turn input.
        private float m_OriginalPitch;              // The pitch of the audio source at the start of the scene.
        private ParticleSystem[] m_particleSystems; // References to all the particles systems used by the Tanks
        [SerializeField] private List<GraphNode> _visitedNodes; // Holds all nodes you've been through while traversing 
        int backtrack = 0;                          // Counter for when you're backtracking to the last node with unvisited

        // PERSONAL VARIABLES
        [SerializeField]
        private GraphNode _targetNode;
        [SerializeField]
        private Graph _graph;

        private void Awake()
        {
            m_Rigidbody = GetComponent<Rigidbody>();
            _visitedNodes = new List<GraphNode>();
        }


        private void OnEnable()
        {
            // When the tank is turned on, make sure it's not kinematic.
            m_Rigidbody.isKinematic = false;

            // Also reset the input values.
            m_MovementInputValue = 0f;
            m_TurnInputValue = 0f;

            // We grab all the Particle systems child of that Tank to be able to Stop/Play them on Deactivate/Activate
            // It is needed because we move the Tank when spawning it, and if the Particle System is playing while we do that
            // it "think" it move from (0,0,0) to the spawn point, creating a huge trail of smoke
            m_particleSystems = GetComponentsInChildren<ParticleSystem>();
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Play();
            }
        }


        private void OnDisable()
        {
            // When the tank is turned off, set it to kinematic so it stops moving.
            m_Rigidbody.isKinematic = true;

            // Stop all particle system so it "reset" it's position to the actual one instead of thinking we moved when spawning
            for (int i = 0; i < m_particleSystems.Length; ++i)
            {
                m_particleSystems[i].Stop();
            }
        }


        private void Start()
        {
            // The axes names are based on player number.
            m_MovementAxisName = "Vertical" + m_PlayerNumber;
            m_TurnAxisName = "Horizontal" + m_PlayerNumber;

            // Store the original pitch of the audio source.
            m_OriginalPitch = m_MovementAudio.pitch;
        }

        private void Update()
        {
            // Store the value of both input axes.
            //m_MovementInputValue = Input.GetAxis (m_MovementAxisName);
            //m_TurnInputValue = Input.GetAxis (m_TurnAxisName);
            m_MovementInputValue = 1;
            if (_targetNode != null)
            {
                var targetDistance = _targetNode.gameObject.transform.position - transform.position;
                var targetDirection = targetDistance;

                var tankRotationEuler = m_Rigidbody.transform.rotation.eulerAngles;
                var targetRotationEuler = Quaternion.LookRotation(targetDirection, Vector3.up).eulerAngles;
                //var deltaRotation = tankRotationEuler - targetRotationEuler;
                //var deltaR = Vector3.Cross(tankRotationEuler, targetRotationEuler).normalized;
                var deltaRy = targetRotationEuler.y - tankRotationEuler.y;
                //Debug.Log(deltaRy);
                if ((deltaRy > 5 && deltaRy < 180) || deltaRy < -180)
                {
                    m_TurnInputValue = 0.5f;
                }
                else if ((deltaRy < -5 && deltaRy > -180) || deltaRy > 180)
                {
                    m_TurnInputValue = -0.5f;
                }
                else
                {
                    m_TurnInputValue = 0;
                }

                var distance = Mathf.Sqrt(Mathf.Pow(targetDistance.x, 2) + Mathf.Pow(targetDistance.z, 2));
                if (distance < 0.52f)
                {
                    AddVisited(_targetNode);    // Mark the node as visited 
                    _targetNode = FindNextNode();
                }




                //_turret.transform.rotation = Quaternion.LookRotation(targetDirection, Vector3.up);

                //m_TurnInputValue = targetDirection.x + targetDirection.z;
            }



            EngineAudio();
        }
        /*
        GraphNode FindNextNode()
        {

            return newNode;
        }
        */
        /* Richards turret turn function
        
         */


        private void EngineAudio()
        {
            // If there is no input (the tank is stationary)...
            if (Mathf.Abs(m_MovementInputValue) < 0.1f && Mathf.Abs(m_TurnInputValue) < 0.1f)
            {
                // ... and if the audio source is currently playing the driving clip...
                if (m_MovementAudio.clip == m_EngineDriving)
                {
                    // ... change the clip to idling and play it.
                    m_MovementAudio.clip = m_EngineIdling;
                    m_MovementAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
            else
            {
                // Otherwise if the tank is moving and if the idling clip is currently playing...
                if (m_MovementAudio.clip == m_EngineIdling)
                {
                    // ... change the clip to driving and play.
                    m_MovementAudio.clip = m_EngineDriving;
                    m_MovementAudio.pitch = UnityEngine.Random.Range(m_OriginalPitch - m_PitchRange, m_OriginalPitch + m_PitchRange);
                    m_MovementAudio.Play();
                }
            }
        }


        private void FixedUpdate()
        {
            // Adjust the rigidbodies position and orientation in FixedUpdate.
            Move();
            Turn();
        }


        private void Move()
        {
            // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
            Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;

            // Apply this movement to the rigidbody's position.
            m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
        }


        private void Turn()
        {
            // Determine the number of degrees to be turned based on the input, speed and time between frames.
            float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;

            // Make this into a rotation in the y axis.
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            // Apply this rotation to the rigidbody's rotation.
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
        }

        private bool HasBeenVisited(GraphNode node)
        {
            for (int i = 0; i < _visitedNodes.Count; i++)
                if (_visitedNodes[i] == node)
                    return true;
            return false;
        }

        private bool IsAdjacent(GraphNode node)
        {
            for (int i = 0; i < _targetNode.Adjacent.Count; i++)
                if (_targetNode.Adjacent[i] == node)
                    return true;
            return false;
        }

        private GraphNode UnvisitedAdjacent(GraphNode target)
        {
            for (int i = 0; i < target.Adjacent.Count; i++)
                if (!HasBeenVisited(target.Adjacent[i]))
                    return target.Adjacent[i];
            return null;
        }

        private GraphNode LastNode() 
        {
            GraphNode lastVisited = null; 

            // Find the last visited adjacent node
            while (!IsAdjacent(lastVisited))
                lastVisited = _visitedNodes[_visitedNodes.Count - ++backtrack];
            return lastVisited;
        }

        private void AddVisited(GraphNode node)             // Make sure the node hasn't already been added, and then add it
        {
            for (int i = 0; i < _visitedNodes.Count; i++)
                if (_visitedNodes[i] == node)
                    return;
            _visitedNodes.Add(node);
        }

        private GraphNode FindNextNode()
        {
            if (_visitedNodes.Count == _graph.Nodes.Count)  // If you've already been through every node, then reset it
            {
                _visitedNodes.Clear();
            }

            GraphNode node = UnvisitedAdjacent(_targetNode); // Finds the first unvisited adjacent
            if (node)                                        // If there are any unvisited adjacents  
            {
                backtrack = 0;                               // Reset the backtracking
                return node;
            }
            else
                return LastNode();
        }
    }


}