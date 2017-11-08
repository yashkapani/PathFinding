using UnityEngine;
using System.Collections;
using System;

namespace AISandbox
{
    public class SeekDoor : StateMachine.State
    {
        public Pathfinding pathFinding;
        public GridNode destination;
        public GameObject door;
        public GameObject openDoor;

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
            GameObject newOpen = GameObject.Instantiate<GameObject>(openDoor);
            newOpen.transform.position = door.transform.position;
            newOpen.GetComponent<SpriteRenderer>().color = door.GetComponent<SpriteRenderer>().color;
            openDoor = newOpen;
            pathFinding.setDestination(destination);
        }

        public override void Exit()
        {
            GameObject.Destroy(door);
            openDoor.SetActive(true);
            destination.blocked = false;
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
