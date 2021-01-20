using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Configuration paramaters

    [SerializeField] Paddle paddle1;
    [SerializeField] float xPush = 0f;
    [SerializeField] float yPush = 12f;
    [SerializeField] float ballSpeed = 10f;
    [SerializeField] float paddleVelocityImpactPercentage = 0.3f;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = 0.2f;

    // state
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    // Cached compopnent references
    Rigidbody2D myRigidBody2D;
    AudioSource myAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        paddleToBallVector = transform.position - paddle1.transform.position;
        myAudioSource = GetComponent<AudioSource>();
        myRigidBody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!hasStarted)
        {   
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
        
    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void LaunchOnMouseClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            myRigidBody2D.velocity = (new Vector2(Random.Range(-xPush,xPush), Random.Range(0.5f,yPush)).normalized) * ballSpeed;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        

        if(hasStarted)
        {
            AudioClip clip;
            Vector2 velocityTweak = new Vector2(Random.Range(0f, randomFactor), Random.Range(0f, randomFactor));
            if(collision.collider.CompareTag("Paddle"))
            {
                velocityTweak += collision.rigidbody.velocity * paddleVelocityImpactPercentage;
                clip = ballSounds[4];
                myAudioSource.PlayOneShot(clip);
            }
            if(collision.collider.CompareTag("UnBreakable"))
            {
                clip = ballSounds[3];
                myAudioSource.PlayOneShot(clip);
            }
            if (collision.collider.CompareTag("Walls"))
            {
                clip = ballSounds[1];
                myAudioSource.PlayOneShot(clip);
            }
            if(collision.collider.CompareTag("Breakable"))
            {
                clip = ballSounds[2];
                myAudioSource.PlayOneShot(clip);
            }
                
            

            myRigidBody2D.velocity = (myRigidBody2D.velocity + velocityTweak).normalized * ballSpeed;
        }
    }
}
