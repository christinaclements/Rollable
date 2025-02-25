using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public AudioSource gameOver;
    public Transform player;
    private NavMeshAgent navMeshAgent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        navMeshAgent = GetComponent<NavMeshAgent>();
        gameOver = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        //Debug.Log(player.position);
        if (player != null){
            navMeshAgent.SetDestination(player.position);
        } 
    }

}
