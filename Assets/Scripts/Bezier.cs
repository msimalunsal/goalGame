using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bezier : MonoBehaviour
{
    public LineRenderer lineRenderer; 
    private bool isPlane , isWin = false; //isPlane yere çarpma ile controlPlane'e çarpma arasındaki farkı ayırmak için kullanılır.
    public GameObject player;//ball
    private int numPoints = 50; //line'daki point sayisi tanimlanir.
    private Vector3[] positions = new Vector3[50];
    public Transform pointer; //lineRenderer pointerlardan biri
    public Camera cam;
    Vector3 point, currentPos;
    Vector2 mousePos;
    private AudioSource source;
    public static bool click = false;
    public bool winControl;
    [SerializeField]
    public AudioClip loseClip, kickClip , winClip;
    [SerializeField]
    private float moveSpeed = 4f;
    private int wayPointIndex = 0;
    public GameObject animation;
    public GameObject winLevel , loseLevel , winControlPanel;
    private Animator animator;

    void Start()
    {

        click = false; 
        lineRenderer.enabled = true;
        animator = animation.GetComponent<Animator>();
        cam = Camera.main;
        lineRenderer.positionCount = numPoints;
        source = GetComponent<AudioSource>();

    }

    void Update()
    {
        if (!click) //eger hala parmak kaldirilmadiysa curve cizmeye devam et
        {
            DrawQuadraticCurve();
        }
        if (Input.GetMouseButtonUp(0))//eger parmak kaldirilirsa topu hareket ettir vurma sesi cikar
        {
            source.clip = kickClip;
            source.Play();
            click = true; //atış kontrolü yapıyoruz.
            lineRenderer.enabled = false;
    
        }
        if (click)
        {
            Move();
        }
    }


    void OnCollisionEnter(Collision col)
    {
        

        if (col.gameObject.tag == "Goal" && !isPlane) //eger kaleye top girdiyse kazandi
        {
            isWin = true;
            Win();
        }
        else if (col.gameObject.tag == "Enemy") //eger canavarlara carparsa lose
        {
            Lose();
        }
        else if(col.gameObject.name == "Plane" && !isWin) //eger kaleye değil plane'e duserse lose
        {
            isPlane = true;
            Lose();
        }
    }

    public void Win()
    {
        winLevel.GetComponent<Canvas>().enabled = true;
        winControl = true;
        animator.SetBool("isWin", true);
        source.clip = winClip;
        source.Play();
    }

    public void Lose()
    {
        loseLevel.GetComponent<Canvas>().enabled = true;
        animator.SetBool("isLose", true);
        winControlPanel.GetComponent<Collider>().enabled = false;
        source.clip = loseClip;
        source.Play();

    }
    private void DrawQuadraticCurve()
    {
        for (int i = 1; i < numPoints + 1; i++)
        {
            float t = i / (float)numPoints;
            positions[i - 1] = CalculateQuadraticBezierPoint(t, player.transform.position, checkMouse() ,pointer.position);
        }
        lineRenderer.SetPositions(positions);
      
     
    }

    private void Move()
    {

        if (wayPointIndex <= positions.Length - 1)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, positions[wayPointIndex], moveSpeed * Time.deltaTime);
            if (player.transform.position == positions[wayPointIndex])
            {
                wayPointIndex += 1;
            }
        }
    }

    //mouse hareketini kamera açısı ile normal hale getiriyoruz
    private Vector3 checkMouse() //fare acisini normale uyarliyoruz
    {
 
        mousePos.x = Input.mousePosition.x ;
        mousePos.y = cam.pixelHeight - Input.mousePosition.y;
        currentPos.x = mousePos.x;
        currentPos.y = mousePos.y ;
        currentPos.z = cam.nearClipPlane *20 ;
        point = cam.ScreenToWorldPoint(currentPos);
        point.y = 1;
       // if(currentPos.x > )

        return point;
    }


    private Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        p2.x =  p1.x ;
        //B(t) = (1-t)2P0 + 2(1-t)tP1 + t2P2
        //     = uu*p0    + 2*u*t*p1  + tt*p2
        float u = (1 - t);
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0 + 2 * u * t * p1 + tt * p2;
        return p;
    }
}
