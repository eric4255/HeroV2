using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehavior : MonoBehaviour
{
    private GlobalBehavior mGlobalBehavior = null;
    float health = 1f;
    Renderer r;

    // Start is called before the first frame update
    void Start()
    {
        mGlobalBehavior = FindObjectOfType<GlobalBehavior>();
        r = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (health == 0)
        {
            Debug.Log("Way point hit");
            float x = Random.Range(-15.0f, 15.0f);
            float y = Random.Range(-15.0f, 15.0f);
            Vector3 temp = new Vector3(x, y, 0);
            transform.position += temp;
            health = 1;
            Color old = r.material.color;
            Color newc = new Color(old.r, old.g, old.b, (old.a * 0.8f) * 4f );
            r.material.color = newc;
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
    }
}
