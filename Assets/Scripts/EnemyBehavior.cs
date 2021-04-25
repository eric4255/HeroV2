using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EnemyBehavior : MonoBehaviour
{
    private GlobalBehavior mGlobalBehavior = null;
    [SerializeField]
    List<Transform> waypoints = new List<Transform>(); 
    [SerializeField]

    int waypointIndex = 0;
    float health = 1f;
	Renderer r;
	public float mSpeed = 20f;

	// Use this for initialization
	void Start()
	{
		transform.position = waypoints[waypointIndex].transform.position;
        mGlobalBehavior = FindObjectOfType<GlobalBehavior>();
        r = GetComponent<Renderer>();
    }

    void Awake()
    {
        Transform parent = GameObject.FindWithTag("Waypoints").transform;
        int count = parent.childCount;
        for(int i = 0; i < count; i++)
        {
            waypoints.Add(parent.GetChild(i));
        }
    }
	// Update is called once per frame
	void Update()
	{
        //transform.position += (mSpeed * Time.smoothDeltaTime) * transform.up;
        //GlobalBehavior globalBehavior = GameObject.Find("GameManager").GetComponent<GlobalBehavior>();

        Move();

        if (health == 0)
        {
            Destroy(gameObject);
            mGlobalBehavior.EnemyDestroyed();
        }

	}

    void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position,
                                                    waypoints[waypointIndex].transform.position,
                                                    mSpeed * Time.deltaTime);

        if (transform.position == waypoints[waypointIndex].transform.position)
        {
            waypointIndex += 1;
        }

        if (waypointIndex == waypoints.Count)
        {
            waypointIndex = 0;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Egg")
        {
            health -= 0.25f;
            Color old = r.material.color;
            Color newc = new Color(old.r, old.g, old.b, old.a * 0.8f);
            r.material.color = newc;
            Destroy(collision.gameObject);
            mGlobalBehavior.DestroyAnEgg();
        }

        if (collision.tag == "Player")
        {
            health -= 1f;
        }

	}
}

