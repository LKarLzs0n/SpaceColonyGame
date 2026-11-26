using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{

    public ColonyManager colony;
    private NavMeshAgent agent;
    public Stockpile stockpile;
    private ProductionBuilding workplace;
    public House home;
    
    private enum State {Idle, GoingToWork, Working, GoingToDepot}
    private State state = State.Idle;
    private float workTimer;
    private StorageDepot depot;
    private ResourceAmount[] carrying;
    

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    bool HasArrived()
    {
        return !agent.pathPending && agent.remainingDistance <= 0.5f;
    }

    void UpdateIdle()
    {
        workplace = colony.FindFreeWorkplace(transform.position);

        if (workplace != null)
        {
            workplace.worker = this;
            
            agent.SetDestination(workplace.GetNearestPoint(transform.position));
            state = State.GoingToWork;
        }
    }

    void UpdateGoingToWork()
    {
        if (!HasArrived()) return;

        if (!stockpile.CanAfford(workplace.data.input)) return;
        
        stockpile.Spend(workplace.data.input);
        workTimer = 0f;
        state = State.Working;
    }

    void UpdateWorking()
    {
        workTimer += Time.deltaTime;
        if (workTimer < workplace.data.workTime) return;
        
        depot = stockpile.GetNearestDepot(transform.position);
        if (depot == null) return;

        carrying = workplace.data.output;
        
        agent.SetDestination(depot.GetNearestPoint(transform.position));
        state = State.GoingToDepot;
    }

    void UpdateGoingToDepot()
    {
        if (depot == null)
        {
            state = State.Working;
            return;
        }
        
        if (!HasArrived()) return;
        
        stockpile.AddAll(carrying);
        
        agent.SetDestination(workplace.GetNearestPoint(transform.position));
        state = State.GoingToWork;
    }
    
    void Update()
    {
        if (workplace == null)
        {
            state = State.Idle;
        }

        switch (state)
        {
            case State.Idle: UpdateIdle(); break;
            case State.GoingToWork: UpdateGoingToWork(); break;
            case State.Working: UpdateWorking(); break;
            case State.GoingToDepot: UpdateGoingToDepot(); break;
        }
    }
}
