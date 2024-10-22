using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossAI : MonoBehaviour
{

    [SerializeField] int HP;
    [SerializeField] Image HPBar;
    [SerializeField] float rotationSpeed;
    [SerializeField] int bossViewAngle;

    [SerializeField] int pauseDurationMin;
    [SerializeField] int pauseDurationMax;

    [SerializeField] int minRotations;
    [SerializeField] int maxRotations;

    [SerializeField] float riseSpeed;
    [SerializeField] float riseMaxHeight;


    Vector3 playerDirection;

    Color colorOriginal;

    float angleToPlayer;
    float previousRotation_Y;
    float startingHeight;

    int origHP;

    bool isSpinPaused;

    float totalRotation;
    Quaternion targetRotation;


    private State state;
    private enum State
    {
        Spawned,
        Rise,
        Stationary,
        Spin,
        Dead
    }


    // Start is called before the first frame update
    void Start()
    {
        origHP = HP;
        HPBar.fillAmount = (float)HP / origHP;

        //so game doesn't as long as boss is alive
        gameManager.instance.updateGameGoal(1);

        startingHeight = transform.position.y;
        state = State.Rise;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case State.Rise:
                liftBoss();
                break;

            case State.Stationary:
                isPlayerBehindBoss();
                break;

            case State.Spin:
                spinBoss();
                break;

            case State.Dead:

                break;
        }
    }


    void isPlayerBehindBoss()
    {
        //have raycast track player, save to external variable
        playerDirection = gameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDirection.x, 0, playerDirection.z), transform.forward);


        Debug.DrawRay(transform.position, playerDirection);


        RaycastHit hit;
        Physics.Raycast(transform.position, playerDirection, out hit);

        //is behind boss
        if (angleToPlayer >= bossViewAngle)
        {
            //save current rotation
            previousRotation_Y = transform.eulerAngles.y;

            //select angle to spin to
            //roll a dice
            totalRotation = 90 * (Random.Range(minRotations, maxRotations + 1)); //normally uses max as number it can't roll up to


            //swap to spin state and start spin delay timer
            isSpinPaused = true;
            state = State.Spin;
            StartCoroutine(pauseBeforeSpin());
        }
    }




    IEnumerator pauseBeforeSpin()
    {
        //roll a dice, have boss pause that many seconds before spinning
        int pauseDuration = Random.Range(pauseDurationMin, pauseDurationMax + 1); //normally uses max as number it can't roll up to
        
        yield return new WaitForSeconds(pauseDuration);

        isSpinPaused = false;
    }



    void spinBoss()
    {
        if (false == isSpinPaused)
        {

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);




            //has exceeded 360 limit
            if (transform.rotation.y < 0)
            {
                //set to (0,0,0)
                transform.rotation = Quaternion.identity;

                //how far have we already rotated
                float rotationsSoFar = 360 - previousRotation_Y;

                //subtract how far we've rotated, from total desired rotation amount
                totalRotation -= rotationsSoFar;


                //set previous rotation back to zero
                previousRotation_Y = 0;

            }



            //if current rotation exceeds desired rotation, stop, snap to desired rotation
            if (transform.eulerAngles.y >= totalRotation + previousRotation_Y)
            {
                transform.Rotate(0, 0, 0);
                transform.rotation = Quaternion.Euler(new Vector3(0, totalRotation + previousRotation_Y, 0));
                state = State.Stationary;
            }
        }
    }



    void liftBoss()
    {
        transform.Translate(Vector3.up * riseSpeed * Time.deltaTime, Space.World);

        if (transform.position.y >= startingHeight + riseMaxHeight)
        {
            transform.Translate(Vector3.zero);
            state = State.Stationary;
        }
    }


    public void takeDamage(int amount)
    {
        HP -= amount;

        //update boss HP bar above it
        HPBar.fillAmount = (float)HP / origHP;


        if (HP <= 0)
        {
            Destroy(gameObject);
            gameManager.instance.gameWon();
        }
    }
}
