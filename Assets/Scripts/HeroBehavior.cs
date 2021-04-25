using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroBehavior : MonoBehaviour {
    
    public Text mEnemyCountText = null;
    public EggStatSystem mEggStat = null;
    public float mHeroSpeed = 20f;
    public float kHeroRotateSpeed = 90f/2f; // 90-degrees in 2 seconds
    public Text driver = null;
    public float mHeroRotateSpeed = 90f / 2f;
    public bool mFollowMousePosition = true;
    private bool keyboard = false;
    private GlobalBehavior mGlobalBehavior = null;

    private int mPlanesTouched = 0;

    void Start () {
        Debug.Assert(mEggStat != null);
        mGlobalBehavior = FindObjectOfType<GlobalBehavior>();
    }
	
	// Update is called once per frame
	void Update () {
        //UpdateMotion();
        wasdMovement();
        BoundPosition();
        ProcessEggSpwan();
    }

    private void UpdateMotion()
    {
        mHeroSpeed += Input.GetAxis("Vertical");
        transform.position += transform.up * (mHeroSpeed * Time.smoothDeltaTime);
        transform.Rotate(Vector3.forward, -1f * Input.GetAxis("Horizontal") *
                                    (kHeroRotateSpeed * Time.smoothDeltaTime));
    }

    private void wasdMovement()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            keyboard = !keyboard;
            mFollowMousePosition = !mFollowMousePosition;
            if (keyboard)
            {
                driver.text = "Driver: Keyboard";
            }
            else
            {
                driver.text = "Driver: Mouse";
            }

        }

        Vector3 pos = transform.position;

        if (mFollowMousePosition)
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Debug.Log("Position is " + pos);
            pos.z = 0f;  // <-- this is VERY IMPORTANT!
            // Debug.Log("Screen Point:" + Input.mousePosition + "  World Point:" + p);
        }
        if (Input.GetKey(KeyCode.W))
        {
            pos += ((mHeroSpeed * Time.smoothDeltaTime) * transform.up);
        }

        if (Input.GetKey(KeyCode.S))
        {
            pos -= ((mHeroSpeed * Time.smoothDeltaTime) * transform.up);
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(transform.forward, -mHeroRotateSpeed * Time.smoothDeltaTime);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(transform.forward, mHeroRotateSpeed * Time.smoothDeltaTime);
        }
        transform.position = pos;
    }

    private void BoundPosition()
    {
        GlobalBehavior.WorldBoundStatus status = GlobalBehavior.sTheGlobalBehavior.ObjectCollideWorldBound(GetComponent<Renderer>().bounds);
        switch (status)
        {
            case GlobalBehavior.WorldBoundStatus.CollideBottom:
            case GlobalBehavior.WorldBoundStatus.CollideTop:
                transform.up = new Vector3(transform.up.x, -transform.up.y, 0.0f);
                break;
            case GlobalBehavior.WorldBoundStatus.CollideLeft:
            case GlobalBehavior.WorldBoundStatus.CollideRight:
                transform.up = new Vector3(-transform.up.x, transform.up.y, 0.0f);
                break;
        }
    }

    private void ProcessEggSpwan()
    {
        if (mEggStat.CanSpawn()) {
            if (Input.GetKey("space"))
                mEggStat.SpawnAnEgg(transform.position, transform.up);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Debug.Log("Here x Plane: OnTriggerEnter2D");
            mPlanesTouched = mPlanesTouched + 1;
            mEnemyCountText.text = "Planes touched: " + mPlanesTouched;
            Destroy(collision.gameObject);
            mGlobalBehavior.EnemyDestroyed();
        }
        
    }
}
