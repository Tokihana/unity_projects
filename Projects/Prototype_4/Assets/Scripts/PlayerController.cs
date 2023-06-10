using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup = false;
    private int powerupStrength = 20;
    private int powerupTime = 7;
    private Vector3 indicatorOffset = new Vector3(0f, -0.5f, 0f);
    public GameObject powerupIndicator;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        playerRb.AddForce(speed * (focalPoint.transform.forward * verticalInput + focalPoint.transform.right * horizontalInput));

        powerupIndicator.transform.position = transform.position + indicatorOffset;

    }

    // called when trigger
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountDownRountine());
            powerupIndicator.SetActive(true);
        }
    }

    // called when collision
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            // when an enemy collide with poweruped player,
            // add a force on this enemy in the direction the player towards enemy
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 knockForce = (collision.gameObject.transform.position - gameObject.transform.position).normalized;

            // Debug.Log("Enemy collided with poweruped player");
            enemyRigidbody.AddForce(knockForce * powerupStrength, ForceMode.Impulse);

        }
    }

    // IEnumerator to make the powerup status remain for several seconds
    // rather than last forever
    IEnumerator PowerUpCountDownRountine()
    {
        yield return new WaitForSeconds(powerupTime);
        hasPowerup= false;
        powerupIndicator.SetActive(false);
    }
}
