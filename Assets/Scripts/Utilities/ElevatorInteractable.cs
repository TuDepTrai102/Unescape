using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NT
{
    public class ElevatorInteractable : Interactable
    {
        [Header("INTERACTABLE COLLIDER")]
        [SerializeField] Collider interactableCollider;

        [Header("DESTINATION")]
        [SerializeField] Vector3 destinationHigh; //HIGHEST POINT ELEVATOR WILL TRAVEL
        [SerializeField] Vector3 destinationLow; //LOWEST POINT ELEVATOR WILL TRAVEL
        [SerializeField] bool isTraveling = false;
        [SerializeField] bool buttonIsReleased = true;

        [Header("ANIMATOR")]
        [SerializeField] Animator elevatorAnimator;
        [SerializeField] string buttonPressAnimaton = "Elevator_Button_Press_01";
        [SerializeField] List<CharacterManager> charactersOnButton;
        //IF DOING MULTIPLAYER, KEEP A LIST OF EVERY CHARACTER ON THE ELEVATOR, NOT JUST THE BUTTON PRESSER

        private void OnTriggerEnter(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                AddCharacterToListOfCharactersStandingOnButton(character);

                if (!isTraveling && buttonIsReleased)
                {
                    ActivateElevator();
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            CharacterManager character = other.GetComponent<CharacterManager>();

            if (character != null)
            {
                StartCoroutine(ReleaseButton(character));
            }
        }

        private void ActivateElevator()
        {
            elevatorAnimator.SetBool("isPressed", true);
            buttonIsReleased = false;
            elevatorAnimator.Play(buttonPressAnimaton);

            if (transform.position == destinationHigh)
            {
                StartCoroutine(MoveElevator(destinationLow, 2.5f));
            }

            if (transform.position == destinationLow)
            {
                StartCoroutine(MoveElevator(destinationHigh, 5f));
            }
        }

        private IEnumerator MoveElevator(Vector3 finalPosition, float duration)
        {
            isTraveling = true;

            if (duration > 0)
            {
                float startTime = Time.time;
                float endTime = startTime + duration;
                yield return null;

                while (Time.time < endTime)
                {
                    transform.position = Vector3.Lerp(transform.position, finalPosition, duration * Time.deltaTime);
                    Vector3 movementVelocity = Vector3.Lerp(transform.position, finalPosition, duration * Time.deltaTime);
                    Vector3 characterMovementVelocity = new Vector3(0, movementVelocity.y, 0);

                    foreach (var character in charactersOnButton)
                    {
                        character.characterController.Move(characterMovementVelocity * Time.deltaTime);
                    }

                    yield return null;
                }

                transform.position = finalPosition;
                isTraveling = false;
            }
        }

        private IEnumerator ReleaseButton(CharacterManager character)
        {
            while(isTraveling)
                yield return null;

            yield return new WaitForSeconds(2);

            RemoveCharacterFromListOfCharactersStandingOnButton(character);

            if (charactersOnButton.Count == 0)
            {
                elevatorAnimator.SetBool("isPressed", false);
                buttonIsReleased = true;
            }
        }

        private void AddCharacterToListOfCharactersStandingOnButton(CharacterManager character)
        {
            if (charactersOnButton.Contains(character))
                return;

            charactersOnButton.Add(character);
        }

        private void RemoveCharacterFromListOfCharactersStandingOnButton(CharacterManager character)
        {
            charactersOnButton.Remove(character);

            //REMOVES NULL ENTRIES FROM LIST
            for (int i = charactersOnButton.Count - 1; i > -1; i--)
            {
                if (charactersOnButton[i] == null)
                {
                    charactersOnButton.RemoveAt(i);
                }
            }
        }
    }
}