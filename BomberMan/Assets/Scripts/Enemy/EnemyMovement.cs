using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour, IDamage
{
    public uint enemyDamage = 0;
    [SerializeField]
    private float speed = 0.1f;
    private Transform enemySprite;
    private float startTime;
    private float currentTime;
    private float totalDistance;
    private List<Vector2> freePositions = new List<Vector2>();
    private bool isBlocked = false;
    private bool isMoveable = false;
    private Vector2 currentCell;
    private Vector2 nextCell;
    private IEnemySpawner enemySpawner;
    private IMapProcessing mapProcessing;
    WaitForSeconds WaitForSeconds = new WaitForSeconds(.1f);
    Animator anim;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        enemySprite = transform.GetChild(0);
        currentCell = nextCell = transform.position;
        enemySpawner = ServiceLocator.GetService<IEnemySpawner>();
        mapProcessing = ServiceLocator.GetService<IMapProcessing>();
        startTime = Time.time;
        isMoveable = CanMove();
        if (isMoveable)
        {
            isBlocked = false;
            GetNextCell();
            totalDistance = Vector3.Distance(currentCell, nextCell);
        }
        else
        {
            StartCoroutine(CheckIfBlocked());
        }
    }
    bool CanMove()
    {
        freePositions = new List<Vector2>();
        CheckFreePosition(Vector3.up + transform.position);
        CheckFreePosition(Vector3.down + transform.position);
        CheckFreePosition(Vector3.left + transform.position);
        CheckFreePosition(Vector3.right + transform.position);
        if (freePositions.Count > 0)
            return true;
        else
            return false;
    }
    void CheckFreePosition(Vector2 _pos)
    {
        GameObject cell = mapProcessing.GetCellAtPosition(Vector2Int.FloorToInt(_pos));
        if (cell == null || cell.GetComponent<EnemyMovement>() != null)
            freePositions.Add(_pos);
    }
    void GetNextCell()
    {
        int num = Random.Range(0, freePositions.Count);
        nextCell = freePositions[num];
        LookAtNextCell(currentCell, nextCell);
    }
    void LookAtNextCell(Vector3 _startPos, Vector3 _endPos)
    {
        Vector3 direction = _endPos - _startPos;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle += 90;
        if (targetAngle == 0 || targetAngle == 360)
            anim.SetTrigger("Down");
        else if (targetAngle == 90)
            anim.SetTrigger("Right");
        else if (targetAngle == 180)
            anim.SetTrigger("Up");
        else if (targetAngle == 270)
            anim.SetTrigger("Left"); 
    }
    IEnumerator CheckIfBlocked()
    {
        yield return WaitForSeconds;
        if (CanMove())
        {
            isBlocked = false;
            yield return null;
        }
        StartCoroutine(CheckIfBlocked());
    }
    void Update()
    {
        if (!isBlocked)
            Move();
    }
    void Move()
    {
        if (Vector3.Distance(transform.position, nextCell) > .1f)
        {
            currentTime = Time.time;
            transform.position = Vector3.MoveTowards(transform.position, nextCell, speed * Time.deltaTime);
            float distanceCovered = (currentTime - startTime) * speed;
            float fraction = distanceCovered / totalDistance;
            transform.position = Vector3.Lerp(currentCell, nextCell, fraction);
        }
        else
        {
            startTime = Time.time;
            transform.position = nextCell;
            currentCell = nextCell;
            isMoveable = CanMove();
            if (isMoveable)
            {
                GetNextCell();
                totalDistance = Vector3.Distance(transform.position, nextCell);
            }
        }
    }

    public void Damage()
    {
        mapProcessing.EmptyGrid(Vector2Int.FloorToInt(currentCell));
        ServiceLocator.GetService<IEnemySpawner>().RemoveEnemy(gameObject);
        Destroy(gameObject);
    }
}
