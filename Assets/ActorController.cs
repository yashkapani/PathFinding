using UnityEngine;
using System.Collections;

namespace AISandbox
{
    [RequireComponent(typeof(IActor))]
    public class ActorController : MonoBehaviour
    {
        [SerializeField]
        Grid grid;

        private IActor _actor;
        private Pathfinding pathfinding;
        // Use this for initialization
        void Start()
        {
            _actor = GetComponent<IActor>();
        }

        // Update is called once per frame
        void Update()
        {
            // Pass all parameters to the character control script.
            _actor.SetInput(0, 0);
        }
    }
}
