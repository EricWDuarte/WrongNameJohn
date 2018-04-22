using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

	public static BackgroundManager instance;

	public float speed;

	public Transform RightPilar;
	private float maxXRightPilar;
	public Transform LeftPilar;
	private float maxXLeftPilar;

	public GameObject background;
	public SpriteRenderer grad;
	private float backgroundSize;

	public GameObject spikes;
	private SpriteRenderer spikeRenderer;
	private BoxCollider2D spikeCollider;
	private float spikesSize;

	public Material normal;
	public Material crazy;

	public float offsetSpeed = -0.06f;
	public Renderer myRenderer;

	void Start () {

		instance = this;

		spikeRenderer = spikes.GetComponent<SpriteRenderer>();
		spikeCollider = spikes.GetComponent<BoxCollider2D>();

		RightPilar.transform.position = new Vector2 (((4f + 1f)/2), 5.5f);
		LeftPilar.transform.position = new Vector2 ( -((4f + 1f)/2), 5.5f);

		spikeRenderer.size =  new Vector2(4f, 1f);
		spikeCollider.size = new Vector2(4f, 1f);

		background.transform.localScale = new Vector3 (0.6f, 1.2f, background.transform.localScale.z);
		grad.size = new Vector2(10f, 12f);

		maxXRightPilar = RightPilar.position.x;
		maxXLeftPilar = LeftPilar.position.x;
		backgroundSize = background.transform.localScale.x;
		spikesSize = spikeRenderer.size.x;

	}

	void Update () {
		//OffSet();

		if (BlockBehavior.crazyInProgress == false) {
			background.GetComponent<MeshRenderer>().sharedMaterial.mainTextureOffset += new Vector2 (0, offsetSpeed);
		} else {
			background.GetComponent<MeshRenderer>().sharedMaterial.mainTextureOffset += new Vector2 (0, -offsetSpeed * 2f);
		}


		if (RightPilar.position.x != maxXRightPilar)
			LerpPos(RightPilar, maxXRightPilar);

		if (LeftPilar.position.x != maxXLeftPilar)
			LerpPos(LeftPilar, maxXLeftPilar);

		if (background.transform.localScale.x != backgroundSize)
			LerpScale(background, backgroundSize);

		if (spikeRenderer.size.x != spikesSize) {
			LerpSize(spikeRenderer, spikesSize);
			spikeCollider.size = spikeRenderer.size;
		}
			
		myRenderer.material.mainTextureOffset -= new Vector2 (offsetSpeed, 0);

	}

	public void Enlarge () {
		maxXRightPilar += 1f;
		maxXLeftPilar -= 1f;
		backgroundSize += 0.2f;
		spikesSize += 2f;
	}

	void LerpPos (Transform pos, float maxPos) {
		pos.position = new Vector3( Mathf.Lerp( pos.position.x, maxPos, speed * Time.deltaTime ), pos.position.y, 0 );

		if (Mathf.Abs(pos.position.x) > Mathf.Abs(maxPos) - 0.01f) {
			Vector3 temp = pos.position;
			temp.x = maxPos;
			pos.position = temp;
		}
			
	}

	void LerpSize (SpriteRenderer sprite, float size) {
		sprite.size = new Vector3( Mathf.Lerp( sprite.size.x, size, speed * Time.deltaTime ), sprite.size.y, 0 );

		if (sprite.size.x > size - 0.02f) {
			Vector3 temp = sprite.size;
			temp.x = size;
			sprite.size = temp;
		}
	}

	void LerpScale (GameObject Go, float size) {
		
		Go.transform.localScale = new Vector3( Mathf.Lerp( Go.transform.localScale.x, size, speed * Time.deltaTime ), Go.transform.localScale.y, Go.transform.localScale.z );

		if (Go.transform.localScale.x > size - 0.02f) {
			Vector3 temp = Go.transform.localScale;
			temp.x = size;
			Go.transform.localScale = temp;
		}
	}

	public void ChangeBackground(bool crazyInProgress) {
		if (crazyInProgress) 
			background.GetComponent<MeshRenderer>().material = crazy;
		else
			background.GetComponent<MeshRenderer>().material = normal;
	}

	void OffSet() {
//		Vector2 offSet = background.GetComponent<MeshRenderer>().sharedMaterial.GetTextureOffset(0);
//		offSet.y += 0.1f * Time.deltaTime;
//		background.GetComponent<MeshRenderer>().sharedMaterial.SetTextureOffset(0, offSet);
		Vector2 offSet2 = normal.GetTextureOffset(0);
		offSet2.y += 0.1f;
		normal.SetTextureOffset(0, offSet2);
	}

}
