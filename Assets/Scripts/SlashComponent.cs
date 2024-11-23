using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashComponent : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
   
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(1,0,0);
        if (spriteRenderer.flipX)
        {
            direction = direction * -1.0f;
        }
        transform.position +=  direction * Time.deltaTime;
    } 
     void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Destroy(other.gameObject); 
        }

    }
}
