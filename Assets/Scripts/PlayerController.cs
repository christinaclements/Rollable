using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;
    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;
    public AudioSource collect;
    public AudioSource win;
    public GameObject explosionFX;
    public GameObject pickupFX;

    void Start() {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        AudioSource[] audioSource = GetComponents<AudioSource>();
        collect  = audioSource[0];
        win = audioSource[1];
    }
    void OnMove(InputValue movementValue){
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }
    
    private void FixedUpdate(){
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
        
    }
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("PickUp")) {
            var currentPickupFX = Instantiate(pickupFX, other.transform.position, Quaternion.identity);
            other.gameObject.SetActive(false);
            Destroy(currentPickupFX, 3);
            count++;
            SetCountText();
            collect.Play();
        }
    }
    void SetCountText(){
        countText.text = "Count: " + count.ToString();
        if (count >= 13){
            winTextObject.SetActive(true);
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            win.Play();
        }
    }
    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Enemy")){
            Instantiate(explosionFX, transform.position, Quaternion.identity);
            Destroy(gameObject, .05f);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            collision.gameObject.GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponentInChildren<Animator>().SetFloat("speed_f", 0);      
            
            
        }
    }

}
