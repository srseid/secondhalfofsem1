using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer bodyRenderer;
    public PlayerController playerController;

    private readonly int isWalkingHash = Animator.StringToHash("IsWalking");
    private readonly int isGroundedHash = Animator.StringToHash("IsGrounded");
    private readonly int dieTriggerHash = Animator.StringToHash("Die");
    private readonly int jumpTriggerHash = Animator.StringToHash("Jump");
    private readonly int deadStateHash = Animator.StringToHash("Dead");

    void Update()
    {
        animator.SetBool(isWalkingHash, playerController.IsWalking());
        animator.SetBool(isGroundedHash, playerController.IsGrounded());

        switch (playerController.GetFacingDirection())
        {
            case PlayerController.FacingDirection.left:
                bodyRenderer.flipX = true;
                break;
            case PlayerController.FacingDirection.right:
                bodyRenderer.flipX = false;
                break;
        }


        switch(playerController.state)
        {
            case PlayerController.CharacterState.Walking:
                //animator.SetBool("IsWalking", true);
                break;
            case PlayerController.CharacterState.Idle:
               
                break;
            case PlayerController.CharacterState.Jumping:
                //animator.SetTrigger(jumpTriggerHash);
                break;
            case PlayerController.CharacterState.Falling:
                break;
            case PlayerController.CharacterState.Dead:
                //animator.Play(deadStateHash);
                break;
        }

        //debug only
        if (Input.GetKeyDown(KeyCode.X))
        {
            animator.SetTrigger(dieTriggerHash);
        }
    }
}
