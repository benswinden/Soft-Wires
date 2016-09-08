using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class Crosshair : MonoBehaviour {

    public float moveSpeed;

    new Rigidbody rigidbody;

    public Material matBlack;
    public Material matGreen;


    public bool mouseDown { get; set; }

    void Awake() {

        //CreateLineMaterial();

        rigidbody = GetComponent<Rigidbody>();

    }

    void Start() {

        var mousePos = Input.mousePosition;
        mousePos.z = 1000.0f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = mousePos;
    }

    void Update() {

        if (Input.GetMouseButtonDown(0)) {

            GetComponent<MeshRenderer>().material = matGreen;
            mouseDown = true;
        }
        if (Input.GetMouseButtonUp(0)) {

            GetComponent<MeshRenderer>().material = matBlack;
            mouseDown = false;
        }
    }

    void FixedUpdate() {

        var mousePos = Input.mousePosition;
        mousePos.z = 1010.0f;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        if (Vector3.Distance(mousePos, transform.position) > 100) {

            rigidbody.AddForce((mousePos - transform.position).normalized * moveSpeed);
        }

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