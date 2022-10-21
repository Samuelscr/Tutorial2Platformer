using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public float speed;
    public float jumpSpeed = 10f;

    public AudioClip Winclip;
    public AudioSource source;

    public Animator animator;

    private bool isTouchingGround; //checks if player in on ground
    public Transform groundCheck; //empty gameobject underplayer foot for ground checking
    public float groundCheckRadius; //checks for any overlaps
    public LayerMask groundLayer; //layer instead of tag

    public Text score;
    public Text WinText;
    public Text LoseText; 

    private int playerLives = 3;

    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;
    public GameObject m_GotHitScreen;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        score.text = (GameController.control.score).ToString();
        WinText.enabled = false;
        LoseText.enabled = false;

       playerLives = 0;
        heart1.SetActive(true);
        heart2.SetActive(true);
        heart3.SetActive(true);
}

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        animator.SetFloat("Speed", Mathf.Abs(hozMovement));

        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetKey(KeyCode.W) && isTouchingGround)
        {
            rd2d.velocity = new Vector2(rd2d.velocity.x, jumpSpeed);
            animator.SetBool("IsJumping", true);
        }
        if(hozMovement > 0)
        {
            gameObject.transform.localScale = new Vector3(.45f, .45f, 1);
        }
        else if(hozMovement <0)
        {
            gameObject.transform.localScale = new Vector3(-.45f, .45f, 1);
        }
        if(m_GotHitScreen != null)
        {
            if(m_GotHitScreen.GetComponent<Image>().color.a > 0)
            {
                var color = m_GotHitScreen.GetComponent<Image>().color;
                color.a -= 0.01f;
                m_GotHitScreen.GetComponent<Image>().color = color;
            }
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (GameController.control.score == 4)
        {
            
            SceneManager.LoadScene("Level2");
            GameController.control.score++;

        }
        if (GameController.control.score == 9)
        {
            WinText.enabled = true;
            source.PlayOneShot(Winclip);
        }
        if (collision.gameObject.layer == 6)
        { 
            animator.SetBool("IsJumping", false);
            //Debug.Log("Jump is False");
        }
        if (collision.collider.tag == "Enemy")
        {
            playerLives += 1;
            Destroy(collision.collider.gameObject);
            gotHurt();
            if(playerLives ==1)
            {
                heart3.SetActive(false);
            }
            if (playerLives == 2)
            {
                heart2.SetActive(false);
            }
            if (playerLives == 3)
            {
                heart1.SetActive(false);
                Destroy(gameObject);
                LoseText.enabled = true;
            }
        }

    }

    void gotHurt()
    {
        var color = m_GotHitScreen.GetComponent<Image>().color;
        color.a = 0.8f;
        m_GotHitScreen.GetComponent<Image>().color = color;
    }

    void OnTriggerEnter2D(Collider2D coin )
    {
        if(coin.CompareTag("Coin"))
        {
            
            score.text = (GameController.control.score).ToString();
            Destroy(coin.gameObject);
            GameController.control.score++;
            Debug.Log(GameController.control.score);
        }
    }

    

}
