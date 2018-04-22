using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawn : MonoBehaviour {

	public static BlockSpawn instance;

	public bool stop = false;

	public Transform blockParent;

	public bool sameColorTest = false;
	private int blockAmount = 4;

	[SerializeField]
	private int arenaWidth;

	[SerializeField]
	private int startingHeight;

	public GameObject blockGO;

	public GameObject blockSpike;

	public Sprite blockDefault;
	public Sprite blockBlue;
	public Sprite blockYellow;
	public Sprite blockRed;
	public Sprite blockGreen;
	public Sprite blockVapor;
	public Sprite blockMagenta;

	public enum BlockType : int {
		Blue,
		Yellow,
		Red,
		Green,
		Vapor,
		Magenta

	}

	public class Block {
		
		public GameObject blockGO;

		public BlockType typeNow;
		public BlockType type;
		public BlockType type2;

		public Sprite sprite;
		public Sprite sprite2;

		public Vector3 pos;
		public bool dead = false;
	}

	private BlockBehavior blockScript;

	private bool GameStarted = true;
	private bool called = false;

	public float startTimer = 5.0f;
	public float crazyTimeAmount;
	public float timeAmount;
	public float timeToFall = 3.0f;

	public int spiked = 0;

	void Awake () {
		instance = this;

		if (sameColorTest) {
			blockAmount = 0;
		}
		
		PlaceStartingBlocks();
	}

	void Update () {

		timeAmount = 8f/arenaWidth;
		crazyTimeAmount = timeAmount/2f;

		if (BlockBehavior.crazyInProgress) {
			timeToFall = crazyTimeAmount;
		}
		else {
			timeToFall = timeAmount;
		}

		if (!called){
			if (GameStarted) {
				StartCoroutine(TimerStart());
					called = true;
			}
		}

	}

	IEnumerator TimerStart () {
		yield return new WaitForSeconds(startTimer);
		GetBlockInfo();
	} 

	IEnumerator TimerReDo () {
		yield return new WaitForSeconds(timeToFall);
		GetBlockInfo();
	}

	void GetBlockInfo () {
		
		Block blockInfo = new Block();
		blockInfo.type = (BlockType)Random.Range(0, blockAmount);
		blockInfo.type2 = (BlockType)Random.Range(4, 6);
		blockInfo.pos = new Vector3(Random.Range(0, arenaWidth) - ((arenaWidth - 1f)/2f), 11.5f, 0);

		PlaceBlock (blockInfo);

	}

	void PlaceBlock (Block block) {

		switch (block.type) {

		case BlockType.Blue:
			block.sprite = blockBlue;

			break;

		case BlockType.Yellow:
			block.sprite = blockYellow;
			break;

		case BlockType.Red:
			block.sprite = blockRed;
			break;

		case BlockType.Green:
			block.sprite = blockGreen;
			break;
		
		default:
			block.sprite = blockDefault;
			break;
		} 


		switch (block.type2) {

		case BlockType.Vapor:
			block.sprite2 = blockVapor;
			break;

		case BlockType.Magenta:
			block.sprite2 = blockMagenta;
			break;

		default:
			block.sprite2 = blockDefault;
			break;
		}

		if (Physics2D.OverlapCircle(block.pos, 0.05f, LayerMask.GetMask("Ground")) == null) {

			GameObject clone = Instantiate(blockGO, block.pos, Quaternion.identity, blockParent);
			block.blockGO = clone;
			clone.GetComponent<BlockBehavior>().blockInfo = block;
			

			if (spiked > 0) {
				Instantiate(blockSpike, clone.transform.position, Quaternion.identity, clone.transform);
				spiked--;
			} 

		}

		if (!stop)
			StartCoroutine(TimerReDo());
	}

	void PlaceStartingBlocks () {

		Vector3 offset = new Vector3(-(arenaWidth - 1f)/2f, 0, 0);

		int x = arenaWidth;
		int y = startingHeight;


		for (int i = 0; i < x; i++) {
			for (int j = 0; j < y; j++) {

				Block block = new Block();

				block.type = (BlockType)Random.Range(0, blockAmount);
				block.type2 = (BlockType)Random.Range(4, 6);


				Vector3 pos;


				switch (block.type) {

				case BlockType.Blue:
					block.sprite = blockBlue;
					break;

				case BlockType.Yellow:
					block.sprite = blockYellow;
					break;

				case BlockType.Red:
					block.sprite = blockRed;
					break;

				case BlockType.Green:
					block.sprite = blockGreen;
					break;

				default:
					block.sprite = blockDefault;
					break;
				}


				switch (block.type2) {

				case BlockType.Vapor:
					block.sprite2 = blockVapor;
					break;

				case BlockType.Magenta:
					block.sprite2 = blockMagenta;
					break;

				default:
					block.sprite2 = blockDefault;
					break;
				}

				pos = offset;
				pos.x += i;
				pos.y += j;


				GameObject clone = Instantiate(blockGO, pos, Quaternion.identity, blockParent);
				block.blockGO = clone;
				clone.GetComponent<BlockBehavior>().blockInfo = block;
				clone.GetComponent<BlockDescend>().callOnce = false;
				 
			}
		}
	}

	public void EnlargeArena () {
		arenaWidth += 2;	
	}
} //CLASS
