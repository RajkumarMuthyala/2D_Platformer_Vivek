
using UnityEngine;

public class Spikehead : EnemyDamage
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float checkDelay;
    private float checkTimer;
    private Vector3 destination;

    private bool attacking;

    private Vector3[] directions = new Vector3[4];


    private void Update()
    {
        if (attacking)
        {
            transform.Translate(destination *  Time.deltaTime * speed);
            
        }
        else
        {
            checkTimer += Time.deltaTime;
            if (checkTimer > checkDelay)
            {
                checkForPlayer();
            }
        }
    }

    private void checkForPlayer()
    {
        CalculateDirection();

        for (int i = 0; i < directions.Length; i++) 
        { 
            Debug.DrawRay(transform.position, directions[i], Color.red);
        }
    }


    private void CalculateDirection()
    {
        directions[0] = transform.right * range;
        directions[1] = -transform.right * range;
        directions[2] = transform.up * range;
        directions[3] = -transform.right * range;

    }

}
