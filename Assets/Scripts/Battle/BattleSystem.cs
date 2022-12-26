using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BattleState { Start, PlayerAction, PlayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit playerUnit; 
    [SerializeField] BattleUnit enemyUnit; 
    [SerializeField] BattleHud playerHud; //Informacion del pokemon del jugador
    [SerializeField] BattleHud enemyHud; //Informacion del pokemon del enemigo
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;
    int currentMove;

    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle() //Actualiza la informacion del jugador y del enemigo
    {
        playerUnit.Setup();
        enemyUnit.Setup();
        playerHud.SetData(playerUnit.Pokemon);
        enemyHud.SetData(enemyUnit.Pokemon);

        dialogBox.SetMoveNames(playerUnit.Pokemon.Moves);

        yield return dialogBox.TypeDialog($"¡Un {enemyUnit.Pokemon.Base.Name} salvaje!"); //Muestra el texto animado
        yield return new WaitForSeconds(0.5f);

        yield return dialogBox.TypeDialog($"¡Adelante, {playerUnit.Pokemon.Base.Name}!"); //Muestra el texto animado
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    void PlayerAction()//Muestra las opciones para que el jugador pueda elegir si pelear o huir
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog($"¿Qué debería hacer {playerUnit.Pokemon.Base.Name}?"));
        dialogBox.EnableActionSelector(true);

    }

    void PlayerMove()
    {
        state = BattleState.PlayerMove;
        dialogBox.EnableActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnableMoveSelector(true);
    }

    private void Update()
    {
        if (state == BattleState.PlayerAction) 
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PlayerMove)
        {
            HandleMoveSelection();
        }
    }

    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow)) 
        {
            if(currentAction < 1)
                currentAction++;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentAction > 0)
                currentAction--;
        }
        dialogBox.UpdateActionSelection(currentAction);

        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (currentAction.Equals(0))
            {
                //Pelear
                PlayerMove();
            }
            else if (currentAction.Equals(1))
            {
                //Huir
            }
        }
    }

    void HandleMoveSelection()
    {
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(currentMove < playerUnit.Pokemon.Moves.Count -1)
                ++currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(currentMove > 0)
                --currentMove;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(currentMove < playerUnit.Pokemon.Moves.Count - 2)
                currentMove += 2;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if(currentMove > 1)
                currentMove -= 2;
        }

        dialogBox.UpdateMoveSelection(currentMove, playerUnit.Pokemon.Moves[currentMove]);
    }
}
