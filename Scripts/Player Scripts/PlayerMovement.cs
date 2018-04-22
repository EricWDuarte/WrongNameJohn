using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
	
	[SerializeField]
	private float acc = 0.5f;

	[SerializeField]
	private float deacc = 0.3f;

	[SerializeField]
	private float speed = 1.0f;

	[SerializeField]
	private float jumpForce = 500.0f;

	private float jumpTimer;

	private float posMommentum;
	private float negMommentum;
	private float mommentum;

	private bool canTurn = true;
	private bool canJump = true;
	private bool canCollide = true;
	private bool fallOnce = true;

	private Rigidbody2D rb;
	public LayerMask GroundLayer;
	public LayerMask SpikeLayer;
	public LayerMask Pilar;

	public Transform GroundCheck;
	public Transform UpCheck;
	public Transform gunPoint;

	private Animator anim;

	private PlayerHP hp;

	void Start () {
		hp = gameObject.GetComponent<PlayerHP>();
		anim = gameObject.GetComponent<Animator>();

		rb = gameObject.GetComponent<Rigidbody2D>();
		IgnoreCollision(false);

	}
	

	void Update () {

		bool touching = Physics2D.OverlapCircleAll(gunPoint.position, 0.05f, GroundLayer).Length +
			Physics2D.OverlapCircleAll(gunPoint.position, 0.05f, Pilar).Length > 0;

		float dir = Input.GetAxisRaw("Horizontal");

		if (dir != 0) {
			if (canTurn) {
				Vector3 temp = new Vector3 (dir * -1, 1, 1);
				transform.localScale = temp;
				anim.SetBool("walk", true);
			}
		} else {
			anim.SetBool("walk", false);
		}

		if (!touching)
			rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);

	}

	void FixedUpdate () {

		jumpTimer += Time.deltaTime;

		if (canCollide) {

			Vector3 offSet = new Vector3(0.2f, 0.005f, 0);
			canJump = Physics2D.OverlapArea(GroundCheck.position - offSet, GroundCheck.position  + offSet, GroundLayer.value);

			if (canJump && jumpTimer > 0.2f) {
				if (Input.GetAxisRaw("Jump") > 0) {
					fallOnce = true;
					jumpTimer = 0;
					rb.AddForce(new Vector3(0,jumpForce));
					//anim.SetBool("jump", true);
					anim.Play("Jump Animation");
					SfxManager.instance.Jump();
				}
			}

			if (canJump && fallOnce && jumpTimer > 0.2f) {
				fallOnce = false;
				SfxManager.instance.Fall();
			}

			if (canJump && Physics2D.OverlapCircle(UpCheck.position, 0.05f, GroundLayer)) {
				GetHit();
				Vector3 pos = transform.position;
				pos.y += 1f;
				transform.position = pos;
			}

			if (Physics2D.OverlapCircle(GroundCheck.position, 0.05f, SpikeLayer)) {
				Died();
			}
		}

	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.tag == "Spike" && canCollide) {
			Died();
		}

		if (col.tag == "Coin" && canCollide) {
			col.transform.parent.SendMessage("GetCoin");
			Destroy(col.transform.parent.gameObject);
		}
	}

	void OnCollisionEnter2D (Collision2D col) {
		if (col.gameObject.tag == "Pill" && canCollide) {
			BlockBehavior.StartCrazy();
			Destroy(col.gameObject);
		}
	}

	void GetHit () {
		hp.LoseHp();
	}

	public void Died () {
		GameManager.instance.PlayerDied();
		IgnoreCollision(true);

		canTurn = false;

		rb.velocity = Vector2.zero;
		rb.AddForce (new Vector2 (0f, 800f));
	}

	void IgnoreCollision (bool ignore) {
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Ground"), ignore);
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Spikes"), ignore);
		canCollide = !ignore;		
	}
}
