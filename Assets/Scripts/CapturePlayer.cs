using UnityEngine;
using System.Collections.Generic;

namespace AISandbox
{
    public class CapturePlayer : StateMachine.State
    {
        public Pathfinding pathFinding;
        public Stack<GridNode> enemyPath;
        public GameObject culprit;
        public GameObject pursuer;
        public override string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public override void Enter()
        {
            pathFinding.PathNodes = new Stack<GridNode>(enemyPath.ToArray());
            pathFinding.nodeToseek = pathFinding.PathNodes.Pop();
        }

        public override void Exit()
        {
        }

        public override void Update()
        {
            pathFinding.plotPath();
            GridNode pursuerNode = GameObject.Find("Grid").GetComponent<Grid>().GetNodeBasedOnPosition(pursuer.transform.position);
            if (culprit != null)
            {
                GridNode enemyNode = GameObject.Find("Grid").GetComponent<Grid>().GetNodeBasedOnPosition(culprit.transform.position);
                if (pursuerNode == enemyNode)
                {
                    culprit.GetComponent<ActorStateMachine>().Destroy();
                    GameObject.Destroy(culprit);
                    culprit = null;
                }
            }
        }
    }
}
