using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using Newtonsoft.Json.Linq;

public class Partner : MonoBehaviour
{
  WebSocket ws;
  public int health = 3;
  public float speed;
  public float jumpForce;
  public GameObject fireBall;
  public Transform firePoint;

  private Rigidbody2D rigidBody;
  private Animator animator;
  private CircleCollider2D circleCollider;
  private BoxCollider2D boxCollider;
  private bool isJumping;
  private bool isFiring;
  private bool isDying;
  private const int GROUND_LAYER = 6;
  private enum AnimationStates
  {
    Idle,
    Run,
    Jump,
    Fire,
    Die,
  }

  float horizontalAxisIntensity;
  private bool isFiringInput;

  private bool isJumpingInput;

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
      if (type == "GAME_STATS")
      {
        horizontalAxisIntensity = float.Parse(json.GetValue("horizontalAxisIntensity").ToString());
        isFiringInput = bool.Parse(json.GetValue("isFiring").ToString());
        isJumpingInput = bool.Parse(json.GetValue("isJumping").ToString());
        print("E agr");
        Dispatcher.Instance.Invoke(() =>
        {


        });
      }
    };
    // play mandar position pra sincornizar
    ws.Send("{\"type\":\"RECONNECT_PARTNER\", \"gameId\": \"" + GlobalData.gameId + "\", \"accessToken\": \"" + GlobalData.accessToken + "\"}");
    if (GlobalData.isHost)
    {
      gameObject.transform.position = new Vector2(-2.8f, -2.714f);
    }
    else
    {
      gameObject.transform.position = new Vector2(-4.230389f, -2.714f);
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
    if (isJumpingInput)
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
    if (isFiringInput)
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
      isDying = true;
      circleCollider.isTrigger = true;
      boxCollider.isTrigger = true;
      rigidBody.bodyType = UnityEngine.RigidbodyType2D.Kinematic;
      SetTransition(AnimationStates.Die);
      GameController.instance.GameOver();
    }
  }

  public void AddHealth(int health)
  {
    this.health += health;
    UpdateLives();
  }

  void OnCollisionEnter2D(Collision2D collision2D)
  {
    if (collision2D.gameObject.layer == GROUND_LAYER)
    {
      isJumping = false;
    }
  }
}
