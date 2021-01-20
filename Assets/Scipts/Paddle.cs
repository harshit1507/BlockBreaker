using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    // configuration paramaters

    [SerializeField] float minX = 1f;
    [SerializeField] float maxX = 15f;
    [SerializeField] float screenWidthInUnits = 16f;
    [SerializeField] AudioClip hitSound;

    // cached references
    GameSession gameSession;
    Ball ball;
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        ball = FindObjectOfType<Ball>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 paddlePos = new Vector2(Mathf.Clamp(GetXPos(), minX, maxX), transform.position.y);
        transform.position = paddlePos;
    }

    private float GetXPos()
    {
        if(gameSession.IsAutoPlayEnabled())
        {
            return ball.transform.position.x;
        }
        else
        {
            return Input.mousePosition.x / Screen.width * screenWidthInUnits;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ball"))
        {
            PlayPaddleHitSFX();
        }
    }

    private void PlayPaddleHitSFX()
    {
        AudioSource.PlayClipAtPoint(hitSound, Camera.main.transform.position);
    }
}
