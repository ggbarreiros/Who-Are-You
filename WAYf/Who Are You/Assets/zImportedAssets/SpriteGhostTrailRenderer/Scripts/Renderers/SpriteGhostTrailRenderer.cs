using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;

namespace ActionCode.Renderers
{
    /// <summary>
    /// Renders ghosts trails for a Sprite.
    /// <para>When the ghosts are inicialized, they are placed inside a GameObject container.</para>
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class SpriteGhostTrailRenderer : MonoBehaviour
    {
        [SerializeField, Tooltip("The ghosts color. The alpha is set here.")]
        protected Color color = Color.white * 0.5f;
        [SerializeField, Tooltip("If enabled, the ghosts will start automatically on Awake function.")]
        protected bool enableOnAwake = true;
        [SerializeField, Tooltip("If enabled, the ghosts will be drawn using only one color (the one above).")]
        protected bool useSingleColor = true;
        [SerializeField, Range(1, 10), Tooltip("The frequency of updates per second.")]
        protected int updatesPerSecond = 4;
        [SerializeField, Range(1, 10), Tooltip("The number of ghosts.")]
        protected int ghostsNumber = 4;
        [SerializeField, Tooltip("Shader used in the materials to draw the ghosts.")]
        protected Shader shader;
        [SerializeField, Tooltip("The suffix of the GameObject container where the ghosts will be placed.")]
        protected string containerSuffix = "Ghosts";
        [SerializeField, Tooltip("The Sprite Renderer component used to create he ghosts. Automatically set when attached.")]
        protected SpriteRenderer spriteRenderer;

        public readonly string SPRITE_GHOST_SHADER_NAME = "Sprites/Ghost";
        public readonly int SHADER_SINGLE_COLOR_PROPERTY = Shader.PropertyToID("_SingleColor");

        protected float updateTime = 0f;
        protected int ghostIndex = 0;
        protected float frequencyTime;
        protected Material material;
        protected SpriteRenderer[] ghostRenderers;

        public GameObject player;


        /// <summary>
        /// The frequency of updates per second.
        /// Increase this number to get more updates.
        /// </summary>
        public int UpdatesPerSecond
        {
            get { return updatesPerSecond; }
            set
            {
                updatesPerSecond = Mathf.Clamp(value, 0, 10);
                frequencyTime = 1F / updatesPerSecond;
            }
        }

        /// <summary>
        /// The GameObject container where the ghosts will be placed.
        /// </summary>
        public Transform Container { get; protected set; }

        /// <summary>
        /// The ghosts color.
        /// </summary>
        public Color Color
        {
            get { return color; }
            set
            {
                color = value;
                for (int i = 0; i < ghostRenderers.Length; i++)
                {
                    ghostRenderers[i].color = color;
                }
            }
        }

        /// <summary>
        /// If enabled, the ghosts will be drawn using only one color.
        /// </summary>
        public bool UseSingleColor
        {
            get { return useSingleColor; }
            set
            {
                useSingleColor = value;
                if (material.HasProperty(SHADER_SINGLE_COLOR_PROPERTY))
                {
                    material.SetFloat(SHADER_SINGLE_COLOR_PROPERTY,
                        useSingleColor ? 1F : 0F);
                }
                else
                {
                    Debug.LogErrorFormat(
                        "{0} does not have a Single Color property. Use {1} shader.",
                        material.shader.name, SPRITE_GHOST_SHADER_NAME);
                }
            }
        }

        /// <summary>
        /// Shader used in the materials to draw the ghosts.
        /// </summary>
        public Shader Shader
        {
            get { return shader; }
            private set
            {
                if (value == null)
                {
                    bool validShadeProperty = shader == null;
                    Debug.LogError(validShadeProperty ?
                        "You should reference a valid shader on SpriteGhostTrailRenderer inspector!" :
                        "You are trying to set an invalid shader. Make sure the shader is not null!");

                    shader = Shader.Find(SPRITE_GHOST_SHADER_NAME);
                    if (shader == null)
                    {
                        Debug.LogErrorFormat("{0} not found. It has been deleted of it's not into your build.", 
                            SPRITE_GHOST_SHADER_NAME);
                        string defaultSpriteShader = "Sprites/Default";
                        Debug.LogWarningFormat("{0} shader will be used", defaultSpriteShader);
                        shader = Shader.Find(defaultSpriteShader);
                    }
                    else
                    {
                        Debug.LogWarningFormat("{0} shader will be used", SPRITE_GHOST_SHADER_NAME);
                    }
                }
                else shader = value;

                material = new Material(shader);
            }
        }

        #region Unity Callbacks
        private void Reset()
        {
            SetupComponents();
        }

        private void Awake()
        {
            UpdatesPerSecond = updatesPerSecond;
            Shader = shader;
            CreateContainer();
            CreateGhosts(ghostsNumber);
            enabled = enableOnAwake;
            UseSingleColor = useSingleColor;
        }

        private void Update()
        {
            UpdateGhosts();
        }

        private void OnEnable()
        {
            Container.parent = null;
            updateTime = 0f;
            ghostIndex = 0;

            foreach (SpriteRenderer ghost in ghostRenderers)
            {
                PlaceGhost(ghost);
            }
        }

        private void OnDisable()
        {
            if (!gameObject.activeInHierarchy) return;

            Container.parent = transform;
            foreach (SpriteRenderer ghost in ghostRenderers)
            {
                ghost.enabled = false;
            }
        }

        private void OnValidate()
        {
            UpdatesPerSecond = updatesPerSecond;
        }
        #endregion

        /// <summary>
        /// Shows the ghosts.
        /// </summary>
        public void Enable()
        {
            enabled = true;
        }

        /// <summary>
        /// Hides the ghosts.
        /// </summary>
        public void Disable()
        {
            enabled = false;
        }

        /// <summary>
        /// Shows or hides the ghosts.
        /// </summary>
        public void Toggle()
        {
            enabled = !enabled;
        }

        /// <summary>
        /// Enables or disables the use of single color shader property.
        /// </summary>
        public void ToggleSingleColor()
        {
            UseSingleColor = !UseSingleColor;
        }

        /// <summary>
        /// Set the use of the single color.
        /// </summary>
        /// <param name="color">The ghost color.</param>
        public void SetSingleColor(Color color)
        {
            UseSingleColor = true;
            Color = color;
        }

        /// <summary>
        /// Use this function to setup every component on Reset callback.
        /// </summary>
        protected virtual void SetupComponents()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        /// <summary>
        /// Creates the ghosts container GameObject at Zero position.
        /// </summary>
        protected virtual void CreateContainer()
        {
            string name = string.Format("{0}-{1}", gameObject.name, containerSuffix);
            Container = new GameObject(name).transform;
        }

        /// <summary>
        /// Updates the ghosts SpriteRenderer position and rotation.
        /// </summary>
        protected virtual void UpdateGhosts()
        {
            updateTime += Time.deltaTime;
            if (updateTime > frequencyTime)
            {
                updateTime = 0f;
                ghostIndex = (ghostIndex + 1) % ghostRenderers.Length;
                PlaceGhost(ghostRenderers[ghostIndex]);
            }
        }

        /// <summary>
        /// Creates the ghost GameObject, set its properties and place it inside the container.
        /// </summary>
        /// <param name="number">The number of the ghost. Used to named it.</param>
        protected virtual void CreateGhosts(int number)
        {
            Assert.IsTrue(number > -1, "Number cannot be negative.");
            ghostRenderers = new SpriteRenderer[number];

            GameObject baseGhost = new GameObject("Ghost-0");
            baseGhost.transform.parent = Container;
            baseGhost.layer = gameObject.layer;
            baseGhost.transform.SetPositionAndRotation(transform.position, transform.rotation);

            ghostRenderers[0] = baseGhost.AddComponent<SpriteRenderer>();
            ghostRenderers[0].material = material;
            ghostRenderers[0].sprite = spriteRenderer.sprite;
            ghostRenderers[0].color = color;
            ghostRenderers[0].sortingLayerID = spriteRenderer.sortingLayerID;
            ghostRenderers[0].sortingOrder = spriteRenderer.sortingOrder - 1;
            ghostRenderers[0].enabled = false;

            for (int i = 1; i < ghostRenderers.Length; i++)
            {
                Vector3 startScale;

                GameObject ghost = Instantiate(baseGhost, Container);
                startScale = ghost.transform.localScale;
                if (player.transform.localScale.x > 0)
                {
                    ghost.transform.localScale = startScale;
                    Debug.Log("Não foi");
                }
                else
                {
                    ghost.transform.localScale = new Vector3(ghost.transform.localScale.x * -1, ghost.transform.localScale.y, ghost.transform.localScale.z);
                    Debug.Log("Foi");
                }
                ghost.name = "Ghost-" + i;
                ghostRenderers[i] = ghost.GetComponent<SpriteRenderer>();
                ghostRenderers[i].enabled = false;

            }
        }

        /// <summary>
        /// Places the given SpriteRenderer position, rotation, sprite, and scale.
        /// </summary>
        /// <param name="ghost">The ghost SpriteRenderer.</param>
        protected virtual void PlaceGhost(SpriteRenderer ghost)
        {
            ghost.transform.SetPositionAndRotation(transform.position, transform.rotation);
            ghost.transform.localScale = transform.localScale;
            ghost.flipX = spriteRenderer.flipX;
            ghost.flipY = spriteRenderer.flipY;
            ghost.sprite = spriteRenderer.sprite;
            ghost.enabled = true;
        }
    }
}