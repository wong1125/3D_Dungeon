using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;



public class Enemy : MonoBehaviour, IInvestigatable
{

    [SerializeField] float playerFollowDistance;
    private float distancePlayer;
    private NavMeshAgent agent;
    private Vector3 startPosition;
    private float lastCheckedTime;
    private float resetLimt = 30f;

    [SerializeField] string objName;
    [SerializeField] string destription;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        distancePlayer = (transform.position - CharacterManager.Instance.Player.transform.position).sqrMagnitude;
        if (distancePlayer < playerFollowDistance)
        {
            FollowPlayer();
            lastCheckedTime = Time.time;
        }
        //일정 시간 플레이어가 탐색 범위 밖에 있으면 위치 초기화
        else if (Time.time - lastCheckedTime > resetLimt)
            transform.position = startPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CharacterManager.Instance.Player.condition.HealthChanger(-10);
        }
    }

    void FollowPlayer()
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
        {
            agent.SetDestination(CharacterManager.Instance.Player.transform.position);
        }
    }

    public bool CanInteract { get; set; } = false;

    public string GetDataString()
    {
        string str = $"[{objName}]\n{destription}";
        return str;
    }

    public void InteractReaction()
    {
        return;
    }
}
