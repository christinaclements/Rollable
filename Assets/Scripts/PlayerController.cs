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
    private Vector3 targetPos;
    [SerializeField] private bool isMoving = false;

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
        if (isMoving){
            // Move the player towards the target position
            Vector3 direction = targetPos - rb.position;
            direction.Normalize();
            rb.AddForce(direction * speed);
        }
        // Stop moving the player if it is close to the target position
        if (Vector3.Distance(rb.position, targetPos) < 0.5f){
            isMoving = false;
        }
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
            gameObject.SetActive(false);
            //Destroy(gameObject, .05f);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lose!";
            collision.gameObject.GetComponent<AudioSource>().Play();
            collision.gameObject.GetComponentInChildren<Animator>().SetFloat("speed_f", 0);
        }
    }
    private void Update(){
        if (Input.GetMouseButton(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(ray.origin, ray.direction * 50, Color.yellow);
            RaycastHit hit; // Define variable to hold raycast hit information

            // Check if raycast hits an object
            // Check if raycast hits an object
            if (Physics.Raycast(ray, out hit)){
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")){
                    targetPos = hit.point; // Set target position
                    isMoving = true; // Start player movement
                }
            }
            else{
                isMoving = false; // Stop player movement
            }
        }
    }
}
