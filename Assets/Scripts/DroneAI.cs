using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    
    enum DroneState
    {
        Idle,
        Move,
        Attack,
        Damage,
        Die
    }
    
    DroneState state = DroneState.Idle;

    
    public float idleDelayTime = 2;
    
    float currentTime;
    
    public float moveSpeed = 1;
    
    Transform tower;
    
    NavMeshAgent agent;
  
    public float attackRange = 3;
  
    public float attackDelayTime = 2;

  
    [SerializeField]
   
    int hp = 3;

   
    Transform explosion;
    ParticleSystem expEffect;
    AudioSource expAudio;

    void Start()
    {
     
        tower = GameObject.Find("Tower").transform;

        agent = GetComponent<NavMeshAgent>();
        agent.enabled = false;
        
        agent.speed = moveSpeed;

        explosion = GameObject.Find("Explosion").transform;
        expEffect = explosion.GetComponent<ParticleSystem>();
        expAudio = explosion.GetComponent<AudioSource>();
    }

    void Update()
    {
        
        switch (state)
        {
            case DroneState.Idle:
                Idle();
                break;
            case DroneState.Move:
                Move();
                break;
            case DroneState.Attack:
                Attack();
                break;
            case DroneState.Damage:
                break;
            case DroneState.Die:
                break;
        }
    }

  
    private void Idle()
    {
    
        currentTime += Time.deltaTime;

        
        if (currentTime > idleDelayTime)
        {
           
            state = DroneState.Move;
          
            agent.enabled = true;
        }
    }

    
    private void Move()
    {
        
        agent.SetDestination(tower.position);

      
        if (Vector3.Distance(transform.position, tower.position) < attackRange)
        {
            state = DroneState.Attack;

          
            agent.enabled = false;
           
            currentTime = attackDelayTime;
        }
    }

    private void Attack()
    {
      
        currentTime += Time.deltaTime;
       
        if (currentTime > attackDelayTime)
        {
           
            //Tower.Instance.HP--;
        
            currentTime = 0;
        }
    }

   
    public void OnDamageProcess()
    {
        hp--;

        if (hp > 0)
        {
            state = DroneState.Damage;
       
            StopAllCoroutines();
            StartCoroutine(Damage());
        }
     
        else
        {
            explosion.position = transform.position;
            
            expEffect.Play();
            
            expAudio.Play();
           
            Destroy(gameObject);
        }
    }

    IEnumerator Damage()
    {
        agent.enabled = false;
    
        Material mat = GetComponentInChildren<MeshRenderer>().material;
        
        Color originalColor = mat.color;
        
        mat.color = Color.red;

    
        yield return new WaitForSeconds(0.1f);

        mat.color = originalColor;
     
        state = DroneState.Idle;
      
        currentTime = 0;
    }
}