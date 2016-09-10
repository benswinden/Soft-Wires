using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Crosshair : MonoBehaviour {

    public bool activated;

    public float moveSpeed;
    public float _MINMOVEDISTANCE;

    public float distanceFromCamera = 1000;

    public Material matBlack;
    public Material matGreen;

    public bool topDown;

    public GameObject normalMesh;
    public GameObject selectorMesh;


    float minMoveDistance;
    new Rigidbody rigidbody;
    Vector3 lastMousePosition;

    bool selectorActivated;

    public bool mouseDown { get; set; }    



    void Awake() {

        //CreateLineMaterial();

        rigidbody = GetComponent<Rigidbody>();
        minMoveDistance = _MINMOVEDISTANCE;
    }

    void Start() {

        if (activated) {
            activated = false;
            activate();
        }
        else {
            activated = true;
            deactivate();
        }
    }

    void Update() {

        if (activated) {
            if (Input.GetMouseButtonDown(0)) {

                foreach (MeshRenderer ren in normalMesh.GetComponentsInChildren<MeshRenderer>()) {
                    ren.material = matGreen;
                }
                mouseDown = true;
            }
            if (Input.GetMouseButtonUp(0)) {

                foreach (MeshRenderer ren in normalMesh.GetComponentsInChildren<MeshRenderer>()) {
                    ren.material = matBlack;
                }
                mouseDown = false;
            }
        }
    }

    void FixedUpdate() {

        if (activated && !selectorActivated) {

            var mousePos = Input.mousePosition;
                        
            mousePos.z = distanceFromCamera;         
            
            mousePos = Manager.currentCamera.ScreenToWorldPoint(mousePos);            

            if (Vector3.Distance(mousePos, lastMousePosition) > 12) {
                minMoveDistance = _MINMOVEDISTANCE;
            }
            else {
                if (minMoveDistance >= 10)
                    minMoveDistance -= 1;
            }


            lastMousePosition = mousePos;

            if (Vector3.Distance(mousePos, transform.position) > minMoveDistance) {

                rigidbody.AddForce((mousePos - transform.position).normalized * moveSpeed);
            }

            if (!topDown) {

                transform.rotation = Manager.rea.transform.rotation;
            }
        }
        else if (activated && selectorActivated) {

            transform.position = hoveringFollower.transform.position;
        }

    }

    public void activate() {

        if (!activated) {

            Manager.currentCrosshair = this;

            activated = true;

            foreach (MeshRenderer ren in GetComponentsInChildren<MeshRenderer>()) {
                ren.enabled = true;
            }

            var mousePos = Input.mousePosition;
            mousePos.z = distanceFromCamera;
            mousePos = Manager.currentCamera.ScreenToWorldPoint(mousePos);    

            transform.position = mousePos;
        }
    }

    public void deactivate() {

        if (activated) {

            activated = false;
            foreach (MeshRenderer ren in GetComponentsInChildren<MeshRenderer>()) {
                ren.enabled = false;
            }
        }
    }

    Vector3 lastPosition;
    GameObject hoveringFollower;

    public void deactivateCursor() {

        activated = false;        
        normalMesh.SetActive(false);
    }

    public void reactivateCursor() {

        activated = true;
        normalMesh.SetActive(true);
    }

    public void selectorActive(GameObject follower) {

        hoveringFollower = follower;
        rigidbody.velocity = Vector3.zero;

        lastPosition = transform.position;
        transform.position = follower.transform.position;
        selectorActivated = true;
        normalMesh.SetActive(false);
        selectorMesh.SetActive(true);
    }

    public void selectorInactive() {

        //transform.position = lastPosition;
        selectorActivated = false;
        normalMesh.SetActive(true);
        selectorMesh.SetActive(false);
    }

    public Material lineMaterial;

    void CreateLineMaterial() {

        if (!lineMaterial) {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    void DrawConnectingLines() {

        if (!lineMaterial) {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }

        GL.Begin(GL.LINES);
        lineMaterial.SetPass(0);
        GL.Color(Color.black);
        GL.Vertex3(transform.position.x, transform.position.y, transform.position.z);
        GL.Vertex3(Manager.rea.transform.position.x, Manager.rea.transform.position.y, Manager.rea.transform.position.z);
        GL.End();
        
    }

    void OnPostRender() {
        
        if (mouseDown)
            DrawConnectingLines();
    }

    // To show the lines in the editor
    void OnDrawGizmos() {
        
        if (mouseDown)
            DrawConnectingLines();
    }

}