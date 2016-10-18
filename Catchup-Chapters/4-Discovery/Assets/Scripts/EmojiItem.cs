using UnityEngine;

namespace Assets.Scripts
{
    public class EmojiItem : MonoBehaviour
    {

        public Vector3 targetPosition;
        public float speed;

        // Use this for initialization
        void Start () {
	
        }
	
        // Update is called once per frame
        void Update () {
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
        }
    }
}
