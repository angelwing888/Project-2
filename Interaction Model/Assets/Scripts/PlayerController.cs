using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public LayerMask solidObjectsLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    /*
        updated version below for newer Unity's built in Player Input Actions

        private void Update()
        {
            if (!isMoving)
            {
                input.x = Input.GetAxisRaw("Horizontal");
                input.y = Input.GetAxisRaw("Vertical");

                if (input != Vector2.zero)
                {
                    var targetPos = transform.position;
                    targetPos.x += input.x;
                    targetPos.y += input.y;

                    StartCoroutine(Move(targetPos));
                }
            }
        }
    */

    public void HandleMove(Vector2 direction)
{
    input = direction;
}


    // adjusted from tutorial to use Invoke Unity Events
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();

        //Debug.Log("This is input.x " + input.x);
        //Debug.Log("This is input.x " + input.y);

    }

    private void Update()
    {
        animator.SetBool("isMoving", input != Vector2.zero);

        if (!isMoving && input != Vector2.zero)
        {
            animator.SetFloat("moveX", input.x);
            animator.SetFloat("moveY", input.y);

            var targetPos = transform.position + new Vector3(input.x, input.y, 0);
            if (IsWalkable(targetPos))
            {
                StartCoroutine(Move(targetPos));
            }
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;
    }

    private bool IsWalkable(Vector3 targetPos)
    {
        if (Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null)
        {
            return false;
        }
        return true;
    }

}
