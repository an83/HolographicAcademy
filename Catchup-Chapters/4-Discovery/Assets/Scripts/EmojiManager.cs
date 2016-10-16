using HoloToolkit.Unity;
using UnityEngine;

namespace Assets.Scripts
{
    public class EmojiManager : MonoBehaviour
    {
        public static EmojiManager Instance { get; private set; }

        public GameObject EmojiTemplate;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CreateEmoji(GazeManager.Instance.Position);
            }
        }

        void OnMouseDown()
        {   
        }

        void Start()
        {
            Debug.Log("emoji started");
        }

        public void CreateEmoji(Vector3 position)
        {
            Debug.Log("CreateEmoji: " + position);

            var emoji = Instantiate(EmojiTemplate);
            emoji.transform.position = position;
            emoji.SetActive(true);
        }
    }
}
