using HoloToolkit.Unity;
using UnityEngine;

namespace Assets.Scripts
{
    public class EmojiManager : MonoBehaviour
    {
        public static EmojiManager Instance { get; private set; }

        
        public GameObject EmojiOn3D;
        public GameObject EmojiOnCanvas;
        public GameObject Canvas;

        private bool _onCanvas = false;
        private RectTransform _canvasTransform;
        private float _emojiCanvasScale = 5;
        private GameObject _target;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                CreateEmojiAndSend(GazeManager.Instance.Position);
            }
        }

        void OnMouseDown()
        {   
        }

        void Start()
        {
            _canvasTransform = Canvas.GetComponent<RectTransform>();

            _target = GameObject.Find("targetObject");


            var w = _canvasTransform.rect.width;
            var h = _canvasTransform.rect.height;

            _target.transform.localPosition= new Vector3(
                    -w / 2,
                    -h / 3 + _emojiCanvasScale / 2,
                    0);

            Debug.Log("emoji started");
        }

        public void CreateEmojiAndSend(Vector3 position)
        {
            Debug.Log("CreateEmojiAndSend: " + position);

            CreateEmoji(position);

            CustomMessages.Instance.SendEmoji(position);
        }

        private void CreateEmoji(Vector3 position)
        {
            var emoji = _onCanvas
                ? CreateEmojiOnCanvas()
                : CreateEmojiIn3D(position);

            emoji.SetActive(true);
        }

        private GameObject CreateEmojiIn3D(Vector3 position)
        {
            var emoji = Instantiate(EmojiOn3D);
            emoji.transform.position = position;
            return emoji;
        }

        private GameObject CreateEmojiOnCanvas()
        {
            var emoji = (GameObject) Instantiate(EmojiOn3D, Canvas.transform);
            emoji.transform.localScale = new Vector3(5f, 5f, 5f);

            var w = _canvasTransform.rect.width;
            var h = _canvasTransform.rect.height;

            var from = new Vector3(
                w/2 - _emojiCanvasScale/2,
                -h/3 + _emojiCanvasScale/2,
                0);
            var to = _target.transform.position;

            emoji.transform.localPosition = @from;

            var emojiItem = emoji.AddComponent<EmojiItem>();
            emojiItem.targetPosition = to;
            emojiItem.speed = 1;
            return emoji;
        }

        public void AddEmojiFromRemote(Vector3 position)
        {
            Debug.Log("AddEmojiFromRemote: " + position);
            CreateEmoji(position);
        }
    }
}
