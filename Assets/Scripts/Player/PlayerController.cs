using System;
using System.Collections;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public float moveSpeed;//Velocidad del personaje
    public LayerMask solidObjectsLayer;//Objetos solidos
    public LayerMask grassLayer;//Pasto

    private bool isMoving;//Boolean si el personaje esta en movimiento
    private Vector2 input;//Input para mover al personaje

    private Animator animator;//Animaciones de movimiento del personaje

    private void Awake() // Genera la animaciones al caminar del personaje
    {
        animator = GetComponent<Animator>();
    }
    
    void Update()// Genera el moviento de forma horizontal y vertical del personaje
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;

            if (input != Vector2.zero)
            {
                animator.SetFloat("moveX", input.x); //Genera las animaciones segun el movimiento en x
                animator.SetFloat("moveY", input.y); //Genera las animaciones segun el movimiento en y
                var targetPos = transform.position;
                targetPos.x += input.x;
                targetPos.y += input.y;
                
                if(IsWalkable(targetPos)) // Comprueba si esta cerca del contorno de un objeto solido
                    StartCoroutine(Move(targetPos));
            }
        }
        animator.SetBool("isMoving", isMoving);
    }
    
    IEnumerator Move(Vector3 targetPos)// Crea el movimiento del personaje en la grilla
    {
        isMoving = true;

        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;

        isMoving = false;

        CheckForEncounters();
    }

    private bool IsWalkable(Vector3 targetPos)//Crea el limite entre los objetos solidos y el personaje
    {
        if(Physics2D.OverlapCircle(targetPos, 0.2f, solidObjectsLayer) != null )
        {
            return false;
        }
        return true;
    }

    private void CheckForEncounters()//Genera los encuentros con los pokemones
    {
        if(Physics2D.OverlapCircle(transform.position, 0.2f, grassLayer) != null )
        {
            if(UnityEngine.Random.Range(1,101) <= 10)
            {
                Debug.Log("Encontraste un pokemon");
            }
        }
    }
}