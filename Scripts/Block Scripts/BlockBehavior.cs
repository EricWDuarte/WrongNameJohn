using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehavior : MonoBehaviour {

	public int maxHp = 3;
	private float hp;

	public float basePillChance = 0.02f;

	public static float crazyTime = 10f;
	public static bool crazyInProgress = false;

	private bool onGround = false;
	private bool dead = false;

	private bool checkOnce = true;

	public LayerMask GroundLayer;

	public Transform SensorDown, SensorLeft, SensorUp, SensorRight;
	public Transform[] Sensors = new Transform[4];

	private SpriteRenderer thisSprite;

	public static ArrayList blockLists = new ArrayList();
	public ArrayList blockList;
	public static ArrayList deadLists = new ArrayList();
	public ArrayList deadList;

	public BlockSpawn.Block blockInfo;

	public GameObject Pill;
	public GameObject SilverCoin;
	public GameObject GoldCoin;


	void Start () {
		
		Sensors[0] = SensorDown;
		Sensors[1] = SensorLeft;
		Sensors[2] = SensorUp;
		Sensors[3] = SensorRight;

		hp = maxHp;
		thisSprite = gameObject.GetComponent<SpriteRenderer>();

		if (crazyInProgress == true) {
			blockInfo.typeNow = blockInfo.type2;
			thisSprite.sprite = blockInfo.sprite2;
		} 
		else {
			blockInfo.typeNow = blockInfo.type;
			thisSprite.sprite = blockInfo.sprite;
		}	
	}

	void Update () {
		
		onGround = gameObject.GetComponent<BlockDescend>().onGround;

		if (!onGround) {
			checkOnce = true;
			if (dead) { 
				SafeExplode();
			}
		} else {
			if (checkOnce == true)
			OneTimeFunctions();
		}
		
	} //  Update

	BlockBehavior[] GetBlocks (Transform[] sensors) {
		
		BlockBehavior[] blocks = new BlockBehavior[4];

		for (int i = 0; i < sensors.Length; i++) {

			if (Physics2D.OverlapCircleAll(sensors[i].position, 0.05f, GroundLayer).Length > 0) {
				GameObject obj = Physics2D.OverlapCircle(sensors[i].position, 0.05f, GroundLayer).gameObject;

				BlockBehavior blockScript = obj.GetComponent<BlockBehavior>();

				if (blockScript.blockInfo.typeNow == blockInfo.typeNow && blockScript.onGround) {

					blocks[i] = obj.GetComponent<BlockBehavior>();
				}
			}
		}

		return blocks;
	}

	void OneTimeFunctions () {

		if (ThisBlockList( this ) != null) 
			ArrayCleaner(ThisBlockList( this ));

		if (ThisBlockList( this ) == null)
			AddToList();

		checkOnce = false;
	}

	void ArrayCleaner (ArrayList arrayToClean) {
		
		// se não tiver nenhum block em volta com a mesma array que esse, tira esse bloco da array

		BlockBehavior[] blocks = GetBlocks(Sensors);

		for (int i = 0; i < blocks.Length; i++) {
			if(ThisBlockList(blocks[i]) == arrayToClean) {
				return;
			}
		}

		arrayToClean.Remove(this);

	}

	void AddToList () {
		
		BlockBehavior[] blocks = GetBlocks(Sensors);
		ArrayList thisBlockArray = ThisBlockList(this);

		for (int i = 0; i < blocks.Length; i++) {
			
			BlockBehavior block = blocks[i];
			ArrayList otherBlockArray = ThisBlockList(block);

			if (block != null && otherBlockArray != null) {

				if (thisBlockArray == null) {
					otherBlockArray.Add(this);
					thisBlockArray = otherBlockArray;
				}

				else if (thisBlockArray != null && thisBlockArray != otherBlockArray){

					thisBlockArray.AddRange(otherBlockArray);
					blockLists.Remove(otherBlockArray);

				}
			}
		}

		if (ThisBlockList(this) == null) {
			CreateBlockList().Add(this);
		}
	}

	void UpdateDeadList () {
		BlockBehavior[] blocks = GetBlocks(Sensors);

		ArrayList thisDeadArray = ThisDeadList(this);

		for (int i = 0; i < blocks.Length; i++) {

			BlockBehavior block = blocks[i];

			ArrayList otherDeadArray = ThisDeadList(block);

			if (block != null && block.dead && otherDeadArray != null ) {
				
				if (thisDeadArray == null) {
					
					otherDeadArray.Add(this);
					thisDeadArray = otherDeadArray;
				}
				else if (thisDeadArray != null && thisDeadArray != otherDeadArray) {

					thisDeadArray.AddRange(otherDeadArray);
					deadLists.Remove(otherDeadArray);
				}
			}
		} // for loop

		if (ThisDeadList(this) == null) {
			CreateDeadList().Add(this);
		}
	} // class



	void GotHit () {

		hp -= 1;

		if (BlockBehavior.crazyInProgress)
			hp -= 1;

		if (hp >= 0)
		thisSprite.color = new Color (hp/maxHp,	hp/maxHp, hp/maxHp, 1f);

		if (dead) {

			if (onGround && AllDead())
			DestroyAll();

			TransfereDamage();
		}

		if (hp <= 0) {
			thisSprite.color = new Color (0, 0, 0, 1f);
			dead = true;
			UpdateDeadList();
		}
	}

	void TransfereDamage () {
		ArrayList temp = ThisDeadList(this);
		ArrayList targets = new ArrayList();

		foreach(BlockBehavior block in temp) {

			BlockBehavior[] tempBlocks = block.GetBlocks(block.Sensors);

			foreach (BlockBehavior tempBlock in tempBlocks) {
				if (tempBlock && !tempBlock.dead && !temp.Contains(tempBlock)) {
					targets.Add(tempBlock);
				}
			}
		}

		foreach(BlockBehavior block in targets) {
			block.GotHit();
		}
	}

	bool AllDead () {
		foreach(BlockBehavior script in ThisBlockList(this)) {
			if (script != null) {
				if (!script.dead) {
					return false;
				}
			}
		}

		return true;
	}

	void DestroyAll () {
		int score;
		int NBlocks = ThisBlockList(this).Count;
		int gold = Mathf.FloorToInt(NBlocks / 3f);

		score = (int)Mathf.Pow(NBlocks, 2);

		if (GameManager.level > 0) {
			float chance = Mathf.Log(NBlocks) * basePillChance;
			if (chance < 0)
				chance = 0;
			
			if (Random.Range(0f, 1f) < chance && crazyInProgress == false) {
				Instantiate(Pill, transform.position, Quaternion.identity);
			}
		}

		if (gold > 0) {
			for (int i = 0; i < gold; i++) {
				GameObject clone =  Instantiate(GoldCoin, transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0), Quaternion.identity);
				clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * 100f, 0));
			}
		}
		
		GameManager.instance.AddScore(score);
		SfxManager.instance.BlockExplode(NBlocks);

		foreach(BlockBehavior script in ThisBlockList(this)) {
			script.Explode();
		}
	}

	void Explode () {
		GameObject clone =  Instantiate(SilverCoin, transform.position, Quaternion.identity);
		clone.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * 100f, 0));
		Destroy(gameObject);
	}

	void SafeExplode () {
		SfxManager.instance.BlockExplode(1);
		ArrayList temp = ThisBlockList(this);
		if (temp != null) {
			temp.Remove(this);
			ArrayCleaner(temp);
		}

		Explode();
	}

	ArrayList CreateDeadList() {
		deadList = new ArrayList();
		deadLists.Add(deadList);
		return deadList;
	}

	ArrayList ThisDeadList (BlockBehavior block) {
		foreach(ArrayList list in deadLists) {
			if (list.Contains(block)) {
				return list;
			}
		}

		return null;
	}

	ArrayList CreateBlockList() {
		blockList = new ArrayList();
		blockLists.Add(blockList);
		return blockList;
	}

	ArrayList ThisBlockList (BlockBehavior block) {
		foreach(ArrayList list in blockLists) {
			if (list.Contains(block)) {
				return list;
			}
		}

		return null;
	}

	public static void StartCrazy () {


		if (crazyInProgress == false) {
			GameManager.instance.StartCoroutine(CrazyTimer());
			SoundManager.instance.CrazyMusic();
			crazyInProgress = true;
		}
	}

	public static IEnumerator CrazyTimer() {

		BackgroundManager.instance.ChangeBackground(true);

		BlockBehavior[] scripts = GameObject.Find("BlockSpawner").GetComponentsInChildren<BlockBehavior>();

		blockLists.Clear();
		deadLists.Clear();

		for (int i = 0; i < scripts.Length; i++) {
			scripts[i].UpdateGraphics(true);	
		}

		yield return new WaitForSeconds( crazyTime );
	
		scripts = GameObject.Find("BlockSpawner").GetComponentsInChildren<BlockBehavior>();

		blockLists.Clear();
		deadLists.Clear();

		for (int i = 0; i < scripts.Length; i++) {
			scripts[i].UpdateGraphics(false);	
		}

		crazyInProgress = false;

		BackgroundManager.instance.ChangeBackground(false);
		SoundManager.instance.Background();

	}

	public void UpdateGraphics ( bool crazy ) {
		
		if (crazy == true) {
			blockInfo.typeNow = blockInfo.type2;
			if (thisSprite != null)
			thisSprite.sprite = blockInfo.sprite2;
		} 
		else {
			blockInfo.typeNow = blockInfo.type;
			if (thisSprite != null)
			thisSprite.sprite = blockInfo.sprite;
		}
			
		if (onGround) {
			checkOnce = true;
			AddToList();
		}

		if (dead)
		UpdateDeadList();

	}
}
