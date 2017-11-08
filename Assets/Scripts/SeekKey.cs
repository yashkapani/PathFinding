using UnityEngine;
using System.Collections;
using System;

namespace AISandbox
{
    public class SeekKey : StateMachine.State
    {
        public Pathfinding pathFinding;
        public GridNode destination;
        public GameObject key;
        public GridNode door;

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
            pathFinding.setDestination(destination);
        }

        public override void Exit()
        {
            GameObject.Destroy(key);
            door.blocked = false;
        }

        public override void Update()
        {
            if (!pathFinding.pathFound)
            {
                pathFinding.doAstar();
            }
            else
            {
                if (!pathFinding.pathFollowing)
                {
                    pathFinding.nodeToseek = pathFinding.PathNodes.Pop();
                    pathFinding.pathFollowing = true;
                }
                if (pathFinding.plotPath())
                    transition = true;
            }
        }
    }
}
