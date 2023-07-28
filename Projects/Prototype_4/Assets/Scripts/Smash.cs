using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Smash : MonoBehaviour
{
    // smash durance
    private float smashTime = 10f;
    // range that smash can affect
    private float smashRange = 20f;
    // smash power
    private float smashPower = 25f;
    // jump power
    private float jumpPower = 15f;
    // UI indicator of this power up
    public GameObject smashIndicator;
    // player rigidbody
    private Rigidbody rb;
    // prevent double jump
    private bool hasJump = false;
    // control smash power up
    private bool hasSmash = false;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // follow the player
        smashIndicator.transform.position = transform.position + new Vector3(0f, 1.2f, 0f);
        
        // get jump Input
        if(!hasJump & Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector3(0, jumpPower, 0), ForceMode.Impulse);
            hasJump = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Smash"))
        {
            // Debug.Log("You have Smash Power!");
            Destroy(other.gameObject);
            smashIndicator.SetActive(true);
            hasSmash = true;
            StartCoroutine(SmashCoolDown());
        }
    }

    IEnumerator SmashCoolDown()
    {
        yield return new WaitForSeconds(10);
        smashIndicator.SetActive(false);
        hasSmash = false;
        // Debug.Log("Smash Power Faded!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(hasJump && collision.collider.CompareTag("Land"))
        {
            hasJump = false;
            // smash enemies in range
            if(hasSmash)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < enemies.Length; i++)
                {
                    Vector3 direction = enemies[i].transform.position - transform.position;
                    if (direction.magnitude < smashRange)
                    {
                        Rigidbody enemyRb = enemies[i].GetComponent<Rigidbody>();
                        enemyRb.velocity = Vector3.zero;
                        enemyRb.AddForce(direction.normalized * smashPower, ForceMode.Impulse);
                    }
                }
            }  
        }
    }
}
