using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // player move speed
    public float speed = 10.0f;
    public float moveSpeed = 10.0f;

    // player movement bounds
    public float xBounds = 15.5f;
    public float zBounds = 7.5f;

    // player health
    private int health = 5;
    private int healNum = 1;

    // player collide damage
    private int collideDamage = 1;


    // control fire
    bool canFire = true;
    public GameObject projectile;
    float fireRate = 0.15f; // fire rate of the player

    // powerup
    float power = 2f; // times that player fired per shoot
    float maxPower = 5;
    int roundPower = 2;

    // fire status
    float fireSpread = 10f;
    float spread = 10f;
    float projectileSpace = 0.7f;


    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = speed;
        fireSpread = spread;
    }

    // Update is called once per frame
    void Update()
    {
        // move player
        PlayerMovement();

        // keep player's position in bounds
        ConstraintPlayerPosition();

        // Fire when player press Z down
        if(Input.GetKeyDown(KeyCode.Z) && canFire)
        {
            StartCoroutine(Fire());
        }

    }

    // call this method in Update() to cope movement
    void PlayerMovement()
    {
        SpeedChange(); 

        // get player input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // use Translate() to move
        transform.Translate(moveSpeed * Time.deltaTime * (Vector3.right * horizontalInput + Vector3.forward * verticalInput));
    }

    // call this method in Update() to contraint players position
    void ConstraintPlayerPosition()
    {
        // keep player's position in bounds
        if ((transform.position.x > 0 ? transform.position.x : -transform.position.x) > xBounds)
        {
            transform.position = new Vector3(transform.position.x > 0 ? xBounds : -xBounds, transform.position.y, transform.position.z);
        }
        if ((transform.position.z > 0 ? transform.position.z : -transform.position.z) > zBounds)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z > 0 ? zBounds : -zBounds);
        }
    }

    // this method is called to change player's speed
    void SpeedChange()
    {
        // if player press shift, cut the speed to half
        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = speed / 2;
            fireSpread = spread / 2;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = speed;
            fireSpread = spread;
        }
    }

    // handle trigger
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().ChangeHealth(collideDamage);
            ChangeHealth(other.gameObject.GetComponent<Enemy>().CollideDamage());
        }
        if(other.gameObject.CompareTag("Enemy Projectile"))
        {
            ChangeHealth(other.gameObject.GetComponent<Projectile>().Damage());
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("Heal"))
        {
            ChangeHealth(-healNum);
            Destroy(other.gameObject);
        }
        if(other.gameObject.CompareTag("Powerup"))
        {
            Powerup();
            Destroy(other.gameObject);
        }
    }

    // change health
    public void ChangeHealth(int damage)
    {
        health -= damage;
        Debug.Log("Player Health: " + health);
        if (health <= 0)
        {
            Debug.Log("Game Over!");
        }
    }

    // handle with powerup
    private void Powerup()
    {
        // Debug.Log("You just picked up a powerup");
        if(power < maxPower)
        {
            power += 0.05f;
        }
        if(Mathf.FloorToInt(power) > roundPower)
        {
            roundPower = Mathf.FloorToInt(power);
            spread += 10f;
            fireSpread = spread;
        }
        // Debug.Log(power + " " + roundPower);
    }

    // coroutine to control the speed of fire
    IEnumerator Fire()
    {
        canFire = false;
        float degree = fireSpread / (roundPower - 1);
        for(int i = 0; i < roundPower; ++i) {

            Quaternion rotate = Quaternion.Euler(0, -fireSpread/2 + degree * i, 0);
            Vector3 direction = transform.rotation * rotate * Vector3.forward;
            for(int j = 0; j < roundPower; ++j)
            { 
                Instantiate(projectile, transform.position + direction * j * projectileSpace, rotate);
            }
            
        }
        
        yield return new WaitForSeconds(fireRate);
        canFire = true;
    }
}
