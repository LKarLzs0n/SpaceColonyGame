using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{

    public ColonyManager colony;
    private NavMeshAgent agent;
    public Stockpile stockpile;
    private ProductionBuilding workplace;
    public House home;
    
    private enum State {Idle, GoingToWork, Working, GoingToDepot, GoingHome, Resting, GoingToFetch, GoingToGather}
    private State state = State.Idle;
    private float workTimer;
    private StorageDepot depot;
    private ResourceAmount[] carrying;
    private Gatherable haul;

    [SerializeField] private int tripsBeforeRest = 5;
    [SerializeField] private float restTime = 10f;
    private int trips;
    private float restTimer;
    private Renderer body;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        body = GetComponent<Renderer>();
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
            
            GoToWork();
            return;
        }

        if (stockpile.GetNearestDepot(transform.position) == null) return;

        haul = colony.FindNearestMarked(transform.position);
        if (haul == null) return;

        haul.carrier = this;
        agent.SetDestination(haul.GetNearestPoint(transform.position));
        state = State.GoingToGather;
    }

    void UpdateGoingToWork()
    {
        if (!HasArrived()) return;
        
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
            depot = stockpile.GetNearestDepot(transform.position);
            if (depot == null) return;
            
            agent.SetDestination(depot.GetNearestPoint(transform.position));
        }
        
        if (!HasArrived()) return;
        
        stockpile.AddAll(carrying);
        
        trips++;
        if (trips >= tripsBeforeRest && home != null)
        {
            trips = 0;
            agent.SetDestination(home.GetNearestPoint(transform.position));
            state = State.GoingHome;
            return;
        }
        
        GoToWork();
    }

    void UpdateGoingHome()
    {
        if (home == null)
        {
            GoToWork();
            return; 
        }

        if (!HasArrived()) return;
        
        body.enabled = false;
        restTimer = 0f;
        state = State.Resting;
    }

    void UpdateResting()
    {
        restTimer += Time.deltaTime;
        if (restTimer < restTime) return;
        
        body.enabled = true;
        GoToWork();
    }

    void UpdateGoingToFetch()
    {
        if (depot == null)
        {
            GoToWork();
            return;
        }

        if (!HasArrived()) return;

        if (!stockpile.CanAfford(workplace.data.input)) return;
        
        stockpile.Spend(workplace.data.input);
        
        agent.SetDestination(workplace.GetNearestPoint(transform.position));
        
        state = State.GoingToWork;

    }

    void GoToWork()
    {

        if (workplace == null)
        {
            state = State.Idle;
            return;

        }

        if (workplace.data.input.Length > 0)
        {
            depot = stockpile.GetNearestDepot(transform.position);
            if (depot != null)
            {
                agent.SetDestination(depot.GetNearestPoint(transform.position));
            }
            state = State.GoingToFetch;
            return;
        }
        agent.SetDestination(workplace.GetNearestPoint(transform.position));
        state = State.GoingToWork;
    }

    void UpdateGoingToGather()
    {
        if (haul == null || !haul.marked)
        {
            if (haul != null) haul.carrier = null;
            haul = null;
            state = State.Idle;
            return;
        }
        
        if (!HasArrived()) return;

        carrying = new ResourceAmount[] { haul.contents };
        colony.RemoveMarked(haul);
        Destroy(haul.gameObject);
        haul = null;

        depot = null;
        state = State.GoingToDepot;

    }
    
    void Update()
    {
        if (workplace == null && (state == State.GoingToWork || state == State.Working || state == State.GoingToFetch))
        {
            state = State.Idle;
            body.enabled = true;
        }
        

        switch (state)
        {
            case State.Idle: UpdateIdle(); break;
            case State.GoingToWork: UpdateGoingToWork(); break;
            case State.Working: UpdateWorking(); break;
            case State.GoingToDepot: UpdateGoingToDepot(); break;
            case State.GoingHome: UpdateGoingHome(); break;
            case State.Resting: UpdateResting(); break;
            case State.GoingToFetch: UpdateGoingToFetch(); break;
            case State.GoingToGather: UpdateGoingToGather(); break;
        }
    }
}
