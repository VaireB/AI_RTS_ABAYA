using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    public Button mineButton;
    public Button patrolButton;
    public PlayerController playerController;
    public Texture2D mineCursor; // Custom cursor for mining
    public Texture2D patrolCursor; // Custom cursor for patrolling
    private enum ActionType { None, Patrol, Mine }
    private ActionType currentAction = ActionType.None;
    private Vector2 hotSpot = Vector2.zero;

    void Start()
    {
        mineButton.onClick.AddListener(OnMineButtonClick);
        patrolButton.onClick.AddListener(OnPatrolButtonClick);
    }

    void OnMineButtonClick()
    {
        SetAction(ActionType.Mine);
    }

    void OnPatrolButtonClick()
    {
        // Toggle mining off when patrol button is clicked again
        playerController.ToggleMining();
        SetAction(ActionType.Patrol);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (currentAction == ActionType.Patrol && hit.collider.CompareTag("Ground"))
                {
                    playerController.SetPatrolDestination(hit.point);
                }
                else if (currentAction == ActionType.Mine && hit.collider.CompareTag("MineableRock"))
                {
                    playerController.MoveTowardsMineableRock(hit.collider.gameObject);
                }
                SetAction(ActionType.None); // Reset action after performing it
            }
        }
    }

    private void SetAction(ActionType action)
    {
        currentAction = action;
        switch (action)
        {
            case ActionType.None:
                Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
                break;
            case ActionType.Patrol:
                Cursor.SetCursor(patrolCursor, hotSpot, CursorMode.Auto);
                break;
            case ActionType.Mine:
                Cursor.SetCursor(mineCursor, hotSpot, CursorMode.Auto);
                break;
        }
    }

    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
