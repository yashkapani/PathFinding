using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace AISandbox
{
    public class CaptureTheFlag : MonoBehaviour
    {
        private const float SPAWN_RANGE = 10.0f;

        [SerializeField]
        Grid grid;
        
        [SerializeField]
        GameObject _triangleActorPrefab;

        [SerializeField]
        GameObject _treasurePrefab;

        [SerializeField]
        GameObject _redeventPrefab;

        [SerializeField]
        GameObject _blueeventPrefab;

        [SerializeField]
        GameObject _jailPrefab;

        static public GridNode redTreasureNode;
        static public GridNode blueTreasureNode;

        private GameObject CreateActor()
        {
            GameObject newActor = Instantiate<GameObject>(_triangleActorPrefab);
            newActor.gameObject.name = "Actor";
            newActor.GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
            newActor.transform.position = new Vector3(Random.Range(-SPAWN_RANGE, SPAWN_RANGE), Random.Range(-SPAWN_RANGE, SPAWN_RANGE), -0.2f);
            newActor.GetComponent<OrientedActor>().initialVelocity = Random.onUnitSphere * Random.Range(0.0f, newActor.GetComponent<IActor>().MaxSpeed);
            return newActor;
        }

        public void ShowEvent(Color color, string message, string tag)
        {
            if (tag == "Red")
            {
                GameObject newEvent = Instantiate<GameObject>(_redeventPrefab);
                newEvent.transform.parent = _redeventPrefab.transform.parent;
                newEvent.transform.GetChild(0).GetComponent<Image>().color = color;
                newEvent.transform.GetChild(1).GetComponent<Text>().text = message;
                Vector3 position = _redeventPrefab.GetComponent<RectTransform>().anchoredPosition;
                int events = GameObject.FindGameObjectsWithTag("RedEvent").Length;
                position.y -= 20 * events;
                newEvent.GetComponent<RectTransform>().anchoredPosition = position;
                newEvent.SetActive(true);
            }
            else
            {
                GameObject newEvent = Instantiate<GameObject>(_blueeventPrefab);
                newEvent.transform.parent = _blueeventPrefab.transform.parent;
                newEvent.transform.GetChild(0).GetComponent<Image>().color = color;
                newEvent.transform.GetChild(1).GetComponent<Text>().text = message;
                Vector3 position = _blueeventPrefab.GetComponent<RectTransform>().anchoredPosition;
                int events = GameObject.FindGameObjectsWithTag("BlueEvent").Length;
                position.y -= 20 * events;
                newEvent.GetComponent<RectTransform>().anchoredPosition = position;
                newEvent.SetActive(true);
            }
        }

        private void CreateFlags()
        {
            redTreasureNode = grid.GetNode(15, 2);
            GameObject redTreasure = Instantiate<GameObject>(_treasurePrefab);
            redTreasure.GetComponent<SpriteRenderer>().color = Color.red;
            redTreasure.name = "RedTreasure";
            redTreasure.transform.position = new Vector3(redTreasureNode.transform.position.x, redTreasureNode.transform.position.y, -0.2f);
            redTreasure.SetActive(true);

            blueTreasureNode = grid.GetNode(15, 27);
            GameObject blueTreasure = Instantiate<GameObject>(_treasurePrefab);
            blueTreasure.GetComponent<SpriteRenderer>().color = Color.blue;
            blueTreasure.name = "BlueTreasure";
            blueTreasure.transform.position = new Vector3(blueTreasureNode.transform.position.x, blueTreasureNode.transform.position.y, -0.2f);
            blueTreasure.SetActive(true);
        }

        // Use this for initialization
        void Start()
        {
            grid.Create(30, 30);
            Vector2 gridSize = grid.size;
            Vector2 gridPos = new Vector2(gridSize.x * -0.5f, gridSize.y * 0.5f);
            grid.transform.position = gridPos;

            for (int i = 0; i < 3; i++)
            {
                GameObject newActor;
                newActor = CreateActor();
                Vector3 spawnPoint = grid.GetNode(Random.Range(0, 29), Random.Range(0, 14)).transform.position;
                spawnPoint.z = -0.2f;
                newActor.transform.position = spawnPoint;
                newActor.tag = "Red";
                newActor.name = "Red" + i;

                newActor.SetActive(true);
            }

            for (int i = 0; i < 3; i++)
            {
                GameObject newActor;
                newActor = CreateActor();
                Vector3 spawnPoint = grid.GetNode(Random.Range(0, 29), Random.Range(15, 29)).transform.position;
                spawnPoint.z = -0.2f;
                newActor.transform.position = spawnPoint;
                newActor.tag = "Blue";
                newActor.name = "Blue" + i;
                newActor.SetActive(true);
            }

            CreateFlags();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
