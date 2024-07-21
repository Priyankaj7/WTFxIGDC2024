using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();  
        destination = FindObjectOfType<NavMeshObstacle>().transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(destination, transform.position) < 1 &&GameController.instance._currentItems.Count>0)
        {
           destination = GameController.instance._currentItems[Random.Range(0, GameController.instance._currentItems.Count)].transform.position;

        }
        agent.destination = destination;
    }
}
