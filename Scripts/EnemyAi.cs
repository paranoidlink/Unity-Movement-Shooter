using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
   public NavMeshAgent agent;

   public Transform player;

   public LayerMask whatIsGround, whatIsPlayer;

   public float health;

   //Patroling
   public Vector3 walkPoint;
   bool walkPointSet;
   public float walkPointRange;

   //Attacking
   public float timeBetweenAttacks;
   bool alreadyAttacked;
   public GameObject projectile;
   public float attackForce;

   //States
   public float sightRange, attackRange;
   public bool playerInSightRange, playerInAttackRange;

   private void Awake()
   {
       player = GameObject.Find("PlayerObj").transform;
       agent = GetComponent<NavMeshAgent>();
   }

   private void Update()
   {
       //Check for sight and attack range
       playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
       playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

       if (!playerInSightRange && !playerInAttackRange) Patroling();
       if (playerInSightRange && !playerInAttackRange) Chasing();
       if (playerInSightRange && playerInAttackRange) Attacking();
   }

   private void Patroling()
   {
       if (!walkPointSet) SearchWalkPoint();

       if (walkPointSet)
           agent.SetDestination(walkPoint);

       Vector3 distanceToWalkPoint = transform.position - walkPoint;

       //Walkpoint reached
       if (distanceToWalkPoint.magnitude < 1f)
           walkPointSet = false;

   }

   private void SearchWalkPoint()
   {
       //Calculate random point in range
       float randomZ = Random.Range(-walkPointRange, walkPointRange);
       float randomX = Random.Range(-walkPointRange, walkPointRange);

       walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

       //Check Walkpoint is in bounds
       if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
           walkPointSet = true;
   }

   private void Chasing()
   {
       // Enemy is chasing the player
       agent.SetDestination(player.position);
   }

   private void Attacking()
   {
       //Keep enemy still while they shoot
       agent.SetDestination(transform.position);

       // Calculate the distance to the player
       float distanceToPlayer = Vector3.Distance(transform.position, player.position);

       if (distanceToPlayer > attackRange) // If the player is out of attack range, chase instead of attacking
       {
           Chasing();
           return;
       }

       transform.LookAt(player);

       if (!alreadyAttacked)
       {
           // Attack code
           Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
           rb.AddForce(transform.forward * attackForce, ForceMode.Impulse);

           alreadyAttacked = true;
           Invoke(nameof(ResetAttack), timeBetweenAttacks);
       }
   }

   private void ResetAttack()
   {
       alreadyAttacked = false;
   }

   public void TakeDamage(int damage)
   {
       health -= damage;

       if (health <= 0) 
       {
        //gameObject.GetComponent<playerHealth>().Heal(20);
        Invoke(nameof(DestroyEnemy), 0.5f);      

       }
   }

   private void DestroyEnemy()
   {
       Destroy(gameObject);
   }
}