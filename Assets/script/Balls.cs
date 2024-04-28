using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class Balls : MonoBehaviour
{

    [SerializeField] private float InitialSpeed = 10;
    [SerializeField] private float SpeedIncrease = 0.25f;
    [SerializeField] private Text PlayerScore;
    [SerializeField] private Text AIscore;

    public TrailRenderer trail;
    public int HitCounter = 0;
    private Rigidbody2D rb;
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("StartBall", 2f);
        
    }

    private void StartBall()
    {
        rb.velocity = new Vector2(-1,0.34f) * (InitialSpeed + SpeedIncrease * HitCounter);
    }

    private void ResetBall()
    {
        rb.velocity = new Vector2(0, 0);
        HitCounter = 0;
        trail.enabled = false;
        transform.position = new Vector2(0, 0);
        HitCounter = 0;
        StartCoroutine(wait());
        Invoke("StartBall", 2f);

    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.5f);
        trail.enabled = true;
    }
    
    private void PlayerBounce(Transform myObject)
    {
        HitCounter+=1;
        Vector2 ballPos = transform.position;
        Vector2 playerPos = myObject.position;
        audioManager.PlaySFX(audioManager.Hit);
        float xDirection, yDirection;
        if (transform.position.x > 0)
        {
            xDirection = -1;
        }
        else
        {
            xDirection = 1;
        }
        yDirection = (ballPos.y - playerPos.y) / myObject.GetComponent<Collider2D>().bounds.size.y;
        if (yDirection == 0)
        {
            yDirection = 0.25f;
        }

        rb.velocity = new Vector2(xDirection, yDirection) * (InitialSpeed + (SpeedIncrease * HitCounter));
        
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "AI")
        {
            PlayerBounce(collision.transform);
      
        }
    }


    //sistem nilai 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(transform.position.x > 0)
        {
            ResetBall();
        
            PlayerScore.text = (int.Parse(PlayerScore.text) + 1).ToString();
        }
        else
        {
            ResetBall();
            AIscore.text = (int.Parse(AIscore.text) + 1).ToString();
        }
    }
}
