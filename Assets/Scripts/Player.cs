using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;

public class Player : MonoBehaviour
{
  WebSocket ws;
  public int health = 3;
  public float speed;
  public float jumpForce;
  public GameObject fireBall;
  public Transform firePoint;
  public Transform level2InitialPoint;

  private Rigidbody2D rigidBody;
  private Animator animator;
  private CircleCollider2D circleCollider;
  private BoxCollider2D boxCollider;
  private bool isJumping;
  private bool isFiring;
  private bool isDying;
  public static int GROUND_LAYER = 6;
  private enum AnimationStates
  {
    Idle,
    Run,
    Jump,
    Fire,
    Die,
  }

  void Start()
  {
    rigidBody = GetComponent<Rigidbody2D>();
    animator = GetComponent<Animator>();
    circleCollider = GetComponent<CircleCollider2D>();
    boxCollider = GetComponent<BoxCollider2D>();

    UpdateLives();

    ws = new WebSocket("ws://localhost:3000");
    ws.Connect();
    ws.OnMessage += (sender, e) =>
    {
      string data = e.Data;
      JObject json = JObject.Parse(data);
      string type = json.GetValue("type").ToString();
    };
    ws.Send("{\"type\":\"RECONNECT_PLAYER\", \"gameId\": \"" + GlobalData.gameId + "\", \"accessToken\": \"" + GlobalData.accessToken + "\"}");
    if (GlobalData.isHost)
    {
      gameObject.transform.position = new Vector2(-4.230389f, -2.714f);
    }
    else
    {
      gameObject.transform.position = new Vector2(-2.8f, -2.714f);
    }
  }

  void Update()
  {
    if (health > 0)
    {
      Jump();
      HandleFireBallCoroutine();
    }
  }

  void FixedUpdate()
  {
    if (health > 0)
    {
      Move();
    }
  }

  void UpdateLives()
  {
    GameController.instance.UpdateLives(health);
  }

  void SetTransition(AnimationStates state)
  {
    animator.SetInteger("transition", (int)state);
  }

  void Move()
  {
    float horizontalAxisIntensity = Input.GetAxis("Horizontal");

    ws.Send("{\"type\":\"PLAYER_MOVE\", \"horizontalAxisIntensity\": " + horizontalAxisIntensity + ", \"velocityY\": " + rigidBody.velocity.y + ", \"gameId\": \"" + GlobalData.gameId + "\", \"accessToken\": \"" + GlobalData.accessToken + "\", \"x\": " + transform.position.x + ", \"y\": " + transform.position.y + ", \"userId\": \"" + GlobalData.userId + "\"}");

    rigidBody.velocity = new Vector2(horizontalAxisIntensity * speed, rigidBody.velocity.y);

    bool isGoingRight = horizontalAxisIntensity > 0;
    bool isGoingLeft = horizontalAxisIntensity < 0;
    bool isStopped = horizontalAxisIntensity == 0;
    bool isOnTransitionalState = isJumping || isFiring || isDying;

    if (isGoingRight)
    {
      if (!isOnTransitionalState)
      {
        SetTransition(AnimationStates.Run);
      }

      transform.eulerAngles = new Vector3(0, 0, 0);
    }
    else if (isGoingLeft)
    {
      if (!isOnTransitionalState)
      {
        SetTransition(AnimationStates.Run);
      }

      transform.eulerAngles = new Vector3(0, 180, 0);
    }
    else if (isStopped && !isOnTransitionalState)
    {
      SetTransition(AnimationStates.Idle);
    }
  }

  void Jump()
  {
    string isJumpingJson = Input.GetButtonDown("Jump") ? "true" : "false";

    ws.Send("{\"type\":\"PLAYER_JUMPING\", \"isJumping\":" + isJumpingJson + ", \"gameId\": \"" + GlobalData.gameId + "\", \"accessToken\": \"" + GlobalData.accessToken + "\", \"x\": " + transform.position.x + ", \"y\": " + transform.position.y + ", \"horizontalAxisIntensity\": " + Input.GetAxis("Horizontal") + ", \"userId\": \"" + GlobalData.userId + "\"}");
    if (isJumpingJson == "true")
    {
      if (!isJumping)
      {
        isJumping = true;
        SetTransition(AnimationStates.Jump);
        rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
      }
    }
  }

  void HandleFireBallCoroutine()
  {
    StartCoroutine("FireBall");
  }

  IEnumerator FireBall()
  {
    string isFiringJson = ((Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.X)) && !isFiring) ? "true" : "false";

    ws.Send("{\"type\":\"PLAYER_FIRING\", \"isFiring\":" + isFiringJson + ", \"accessToken\": \"" + GlobalData.accessToken + "\", \"x\": " + transform.position.x + ", \"y\": " + transform.position.y + ", \"gameId\": \"" + GlobalData.gameId + "\", \"userId\": \"" + GlobalData.userId + "\"}");
    if (isFiringJson == "true")
    {
      isFiring = true;

      SetTransition(AnimationStates.Fire);

      GameObject fb = Instantiate(fireBall, firePoint.position, firePoint.rotation);

      bool isGoingRight = transform.rotation.y == 0;
      bool isGoingLeft = transform.rotation.y == 180;

      if (isGoingRight)
      {
        fb.GetComponent<FireBall>().isGoingRight = true;
      }

      if (isGoingLeft)
      {
        fb.GetComponent<FireBall>().isGoingRight = false;
      }

      yield return new WaitForSeconds(0.25f);

      isFiring = false;
    }
  }

  private void KillPlayer()
  {
    ws.Send("{\"type\":\"PLAYER_DYING\", \"accessToken\": \"" + GlobalData.accessToken + "\", \"gameId\": \"" + GlobalData.gameId + "\"}");
    isDying = true;
    circleCollider.isTrigger = true;
    boxCollider.isTrigger = true;
    rigidBody.bodyType = UnityEngine.RigidbodyType2D.Kinematic;
    SetTransition(AnimationStates.Die);
    GameController.instance.GameOver();
  }

  public void OnDamage(int damage)
  {
    health -= damage;
    UpdateLives();
    animator.SetTrigger("hit");

    if (transform.rotation.y >= 0)
    {
      transform.position += new Vector3(-0.5f, 0, 0);
    }
    else
    {
      transform.position += new Vector3(0.5f, 0, 0);
    }

    if (health <= 0)
    {
      KillPlayer();
    }
  }

  public void AddHealth(int health)
  {
    this.health += health;
    UpdateLives();
  }


  public void DecreaseHealth(int health)
  {
    this.health -= health;
    UpdateLives();
  }

  void OnCollisionEnter2D(Collision2D collision2D)
  {
    if (collision2D.gameObject.layer == GROUND_LAYER)
    {
      isJumping = false;
    }

    if (collision2D.gameObject.tag == "Hole")
    {
      rigidBody.AddForce(new Vector2(0, 25), ForceMode2D.Impulse);
      DecreaseHealth(1);

      if (health <= 0)
      {
        KillPlayer();
      }
    }
  }

  void OnTriggerEnter2D(Collider2D collision)
  {
    if (collision.gameObject.tag == "Door")
    {
      transform.position = new Vector2(level2InitialPoint.position.x, level2InitialPoint.position.y);
      CurrentLevel.Instance.setCurrentLevel(2);
    }

    if (collision.gameObject.tag == "Treasure")
    {
      GameController.instance.GameWon();
      ws.Send("{\"type\":\"PLAYER_WON\", \"accessToken\": \"" + GlobalData.accessToken + "\", \"gameId\": \"" + GlobalData.gameId + "\", \"userId\": \"" + GlobalData.userId + "\"}");
      Destroy(gameObject);
      CurrentLevel.Instance.setCurrentLevel(1);
    }
  }
}
