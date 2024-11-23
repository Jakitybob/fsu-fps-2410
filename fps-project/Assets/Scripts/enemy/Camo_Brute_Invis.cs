using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camo_Brute_Invis : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRender;
    [SerializeField] float viewableDistance;
    [SerializeField] float OpacityAmount;

    bool playerInRange;

    Vector3 playerDirection;
    float distanceToPlayer;

    // Start is called before the first frame update
    void Start()
    {
        spriteRender.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        checkPlayerDistance();

        if (playerInRange && spriteRender.color != new Color(1f, 1f, 1f, OpacityAmount))
        {
            uncloak();
        }
        else if (!playerInRange && spriteRender.color != new Color(1f, 1f, 1f, 0f))
        {
            cloak();
        }
    }

    void cloak()
    {
        spriteRender.color = new Color(1f, 1f, 1f, 0f);
    }


    void uncloak()
    {
        spriteRender.color = new Color(1f, 1f, 1f, OpacityAmount);
    }


    void checkPlayerDistance()
    {
        distanceToPlayer = Vector3.Distance(gameObject.transform.position, gameManager.instance.player.transform.position);

        if (distanceToPlayer <= viewableDistance)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }
}
