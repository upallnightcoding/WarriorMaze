using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcCntrl : MonoBehaviour
{
    [SerializeField] private MazeData mazeData;
    [SerializeField] private float idleTime;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float turnDistance;

    private CharacterController charCntrl;

    private MazeGenerator maze;

    private MazeCell currentCell;
    private MazeCell targetCell;
    private MazeCell previousCell;

    private OrcStateType state = OrcStateType.IDLE;
    private float idleWait = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        maze = mazeData.GetMaze();

        previousCell = null;
        currentCell = maze.PickRandomCell();
        targetCell = PickFreeNeighbor(currentCell, previousCell);

        transform.position = currentCell.Position;

        charCntrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        switch(state) 
        {
            case OrcStateType.IDLE:
                state = CheckIdleTime(Time.deltaTime);
                break;
            case OrcStateType.PATROL:
                state = Patrol(Time.deltaTime);
                break;
        }
    }

    private OrcStateType CheckIdleTime(float dt) 
    {
        idleWait += dt;

        return((idleWait >= idleTime) ? OrcStateType.PATROL : OrcStateType.IDLE);
    }

    private OrcStateType Patrol(float dt) 
    {
        float distance = Vector3.Distance(targetCell.Position, transform.position);

        if (distance < turnDistance) 
        {
            targetCell = PickFreeNeighbor(currentCell, previousCell);
            previousCell = currentCell;
            currentCell = targetCell;
        }

        MoveToTargetPoint(targetCell.Position, dt);

        return(OrcStateType.PATROL);
    }

    private MazeCell PickFreeNeighbor(MazeCell current, MazeCell previous)
    {
        List<MazeCell> freeList = current.ListFreeNeighbor(); 

        if ((previous != null) && (freeList.Count > 1)) {
            freeList.RemoveAll(cell => cell.IsEqual(previous));
        }

        return(freeList[Random.Range(0, freeList.Count)]);
    }

    private void MoveToTargetPoint(Vector3 target, float dt)
    {
        float distance = Vector3.Distance(target, transform.position);
        Vector3 direction = (target - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * dt);

        charCntrl.Move(transform.forward * moveSpeed * dt);
    }

    private void OnDrawGizmos() 
    {
        if (targetCell != null) 
        {
            Gizmos.DrawLine(targetCell.Position, transform.position);
        }
    }
}

public enum OrcStateType
{
    IDLE,
    PATROL
}