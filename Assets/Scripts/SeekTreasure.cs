using UnityEngine;
using System.Collections;
using System;

namespace AISandbox
{
    public class SeekTreasure : StateMachine.State
    {
        public Pathfinding pathFinding;
        public GridNode destination;
        public GameObject treasure;
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
            destination.blocked = false;
            pathFinding.setDestination(destination);
        }

        public override void Exit()
        {
            GameObject.Destroy(treasure);
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
