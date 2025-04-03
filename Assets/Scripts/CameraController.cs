using UnityEngine;

public class CameraController : MonoBehaviour {
    public GameObject player;
    private Vector3 playerPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        playerPos = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update(){
        transform.position = player.transform.position + playerPos;
    }
}
