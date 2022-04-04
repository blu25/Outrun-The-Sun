using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenerator : MonoBehaviour
{
    public Transform player;

    public int seed;

    public GameObject ramp;
    public GameObject[] crater;
    public GameObject[] rock;
    public GameObject battery;

    public GameObject[] wall;
    public GameObject[] terrain;

    public float chunkSize;
    public float mapWidth;

    int curBiome = 0;

    // Start is called before the first frame update
    void Start()
    {
        //Random.InitState(seed);
        //GenerateBackWall();
        GenerateChunk();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z - player.position.z < 500f)
            GenerateChunk();
    }

    void GenerateChunk() {
        curBiome = Random.Range(0, 4);
        GenerateTerrain();

        Vector3 minPos = transform.position + (Vector3.left * mapWidth);
        Vector3 maxPos = transform.position + (Vector3.right * mapWidth) + Vector3.forward * chunkSize;

        AddInGrid(crater[curBiome], 0.5f, 3f, 0f, 360f, 35, 35, 1f);

        AddInGrid(ramp, 0.5f, 2f, -30f, 30f, 50, 50, 0.6f);

        AddInGrid(rock[curBiome], 0.5f, 1f, 0f, 360f, 15, 15, 0.8f);

        AddInGrid(battery, 1f, 1f, 0f, 360f, 30, 30, 0.6f);

        GenerateWall();

        EatTerrain();

        transform.Translate(Vector3.forward * chunkSize);
    }

    void GenerateBackWall() {
        const int xStep = 8;
        const float coverage = 0.4f;
        for (int i = (int)-mapWidth; i <= mapWidth; i += xStep) {
            Vector3 minPos = transform.position + (Vector3.left * i);
            Vector3 maxPos = minPos + (Vector3.forward * (xStep * coverage)) + (Vector3.right * (xStep * coverage));
            AddNew(wall[curBiome], minPos, maxPos, 1f, 2.5f, 0, 360f, true);
        }
    }

    void GenerateWall() {
        const int zStep = 100;
        const int xStep = 30;
        const float coverage = 0.4f;
        for (int i = 0; i <= chunkSize; i+= zStep) {
            for (int j = (int)-mapWidth; j <= mapWidth; j += (int)mapWidth * 2) {
                Vector3 minPos = transform.position + (Vector3.left * j) + (Vector3.forward * i);
                Vector3 maxPos = minPos + (Vector3.forward * (zStep * coverage)) + (Vector3.right * (xStep * coverage));
                AddNew(wall[curBiome], minPos, maxPos, 1.5f, 2.5f, 80, 100, true);
            }
        }
    }

    void AddInGrid(GameObject obj, float scaleMin, float scaleMax, float rotateMin, float rotateMax,
        int xStep, int zStep, float coverage) {
        for (int i = 0; i <= chunkSize; i += zStep) {
            for (int j = -(int)mapWidth; j < (int)mapWidth; j += xStep) {
                Vector3 minPos = transform.position + (Vector3.left * j) + (Vector3.forward * i);
                Vector3 maxPos = minPos + (Vector3.forward * (zStep * coverage)) + (Vector3.right * (xStep * coverage));
                AddNew(obj, minPos, maxPos, scaleMin, scaleMax, rotateMin, rotateMax);
            }
        }
    }

    void AddNew(GameObject obj, Vector3 minPos, Vector3 maxPos, float scaleMin, float scaleMax, float rotateMin, float rotateMax, bool collisionsAllowed=false) {
        float offsetAmount = Random.Range(-100f, 100f);
        Vector3 newPos = Vector3.one * -100;

        //while (newPos.y <= -99) {
        newPos = new Vector3(
                Random.Range(minPos.x, maxPos.x),
                Random.Range(minPos.y, maxPos.y),
                Random.Range(minPos.z, maxPos.z)
            );
        newPos.y = GetYPos(newPos, collisionsAllowed);

        if (newPos.y < -98)
            return;
        //}

        GameObject new_obj = Instantiate(
            obj,
            newPos,
            Quaternion.Euler(-90, 180, 0),
            transform.parent);

        new_obj.transform.localScale = Vector3.one * Random.Range(scaleMin, scaleMax);

        new_obj.transform.Rotate(0, 0, Random.Range(rotateMin, rotateMax));
    }

    float GetYPos(Vector3 position, bool collisionsAllowed) {
        RaycastHit hit;
        if (Physics.Raycast(position + (Vector3.up * 1000f), Vector3.down, out hit, 2000f, 1 << 6)) {
            if (hit.transform.tag == "Floor" || collisionsAllowed)
                return hit.point.y;
        }
        return -99f;
    }

    void GenerateTerrain() {
        GameObject terrain_obj = Instantiate(terrain[curBiome], transform.position + (Vector3.forward * chunkSize / 2f), transform.rotation, transform.parent);
        terrain_obj.transform.localScale = new Vector3(
            chunkSize / 10f,
            1,
            chunkSize / 10f
        );
        //MeshFilter meshFilter = terrain_obj.GetComponent<MeshFilter>();
        //Mesh originalMesh = meshFilter.sharedMesh; //1
        //Mesh clonedMesh = new Mesh(); //2
        //clonedMesh.name = "clone";
        //clonedMesh.vertices = originalMesh.vertices;
        //clonedMesh.triangles = originalMesh.triangles;
        //clonedMesh.normals = originalMesh.normals;
        //clonedMesh.uv = originalMesh.uv;
        

        //Vector3[] verts = clonedMesh.vertices;

        ////Vector3[] vertices = m.vertices;
        ////Debug.Log(vertices);
        //for (int i = 0; i < verts.Length; i++) //3
        //{
        //    //Debug.Log(clonedMesh.vertices[i]);
        //    verts[i] = new Vector3(
        //        verts[i].x,
        //        verts[i].y + GetHeight(verts[i].x, verts[i].z),
        //        verts[i].z
        //    );
        //}
        //clonedMesh.SetVertices(verts);

        //meshFilter.mesh = clonedMesh;
        //terrain_obj.GetComponent<MeshCollider>().sharedMesh = clonedMesh;
    }

    float GetHeight(float x, float y) {
        float miniNoise = Mathf.PerlinNoise(x, y) * 2f;
        float biggerNoise = Mathf.PerlinNoise(x / 10f, y / 10f) * 5f;
        return miniNoise + biggerNoise;
    }

    void EatTerrain() {
        foreach (Transform sibling in transform.parent) {
            if (sibling == transform)
                continue;

            if (sibling.position.z <= transform.position.z - 1000f) {
                Destroy(sibling.gameObject);
            }
        }
    }
}
