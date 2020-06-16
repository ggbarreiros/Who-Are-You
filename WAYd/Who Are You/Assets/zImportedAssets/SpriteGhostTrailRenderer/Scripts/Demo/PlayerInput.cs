using ActionCode.Renderers;
using UnityEngine;

namespace ActionCode.Demo
{
    [DefaultExecutionOrder(-100)]
    [RequireComponent(typeof(PlayerMotor))]
    [RequireComponent(typeof(SizeEffector))]
    [RequireComponent(typeof(SpriteGhostTrailRenderer))]
    public sealed class PlayerInput : MonoBehaviour
    {
        [SerializeField] private string horizontalAxis = "Horizontal";
        [SerializeField] private string verticalAxis = "Vertical";
        [SerializeField] private string sizeAxis = "Size";
        [SerializeField] private string resetSizeButton = "ResetSize";
        [SerializeField] private string jumpButton = "Jump";
        [SerializeField] private string attackButton = "Fire1";
        [SerializeField] private string toggleGhostButton = "ToogleGhost";
        [SerializeField] private string cycleColorButton = "CycleColor";
        [SerializeField] private string singleColorButton = "ToggleSingleColor";

        [SerializeField] private PlayerMotor motor;
        [SerializeField] private SizeEffector sizeEffector;
        [SerializeField] private SpriteGhostTrailRenderer trailRenderer;

        [SerializeField]
        private Color[] ghostColors = new Color[5]
        {
            Color.red,
            Color.blue,
            Color.yellow,
            Color.gray,
            Color.green
        };

        private int colorIndex = 0;

        private void Reset()
        {
            motor = GetComponent<PlayerMotor>();
            sizeEffector = GetComponent<SizeEffector>();
            trailRenderer = GetComponent<SpriteGhostTrailRenderer>();
        }

        private void Update()
        {
            bool hasJumped = Input.GetButtonDown(jumpButton);
            bool hasAttacked = Input.GetButtonDown(attackButton);
            bool toggleGhost = Input.GetButtonDown(toggleGhostButton);
            bool cycleColor = Input.GetButtonDown(cycleColorButton);
            bool singleColor = Input.GetButtonDown(singleColorButton);
            bool resetSize = Input.GetButtonDown(resetSizeButton);
            float crouching = -Input.GetAxisRaw(verticalAxis);
            float horInput = Input.GetAxisRaw(horizontalAxis);
            float size = Input.GetAxisRaw(sizeAxis);

            if (hasJumped) motor.Jump();
            if (hasAttacked) motor.Attack();
            motor.Crouch(crouching);
            motor.Move(horInput);

            if (toggleGhost) trailRenderer.Toggle();
            if (cycleColor)
            {
                colorIndex = (colorIndex + 1) % ghostColors.Length;
                trailRenderer.Color = ghostColors[colorIndex];
            }
            if (singleColor) trailRenderer.ToggleSingleColor();

            if (size > 0.5f) sizeEffector.IncreaseSize();
            else if (size < -0.5f) sizeEffector.DecreaseSize();
            if (resetSize) sizeEffector.ResetSize();
        }
    }
}