using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice;
using UnityEngine.InputSystem;

public class SliceObject : MonoBehaviour
{
    public Transform startSlicePoint;
    public Transform endtSlicePoint;
    public LayerMask SliceableLayer;

    public Transform planeDebug;
    public GameObject target;
    public Material cuttedMaterial;
    public VelocityEstimator velocityEstimator;

    public float cutForce = 2000;
    private CutCupGameController gameController;

    private void Awake()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<CutCupGameController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        bool hasHit = Physics.Linecast(startSlicePoint.position, endtSlicePoint.position, out RaycastHit hit, SliceableLayer);
        if (hasHit)
        {
            Debug.Log("sliced");
            GameObject target = hit.transform.gameObject;
            SliceObj(target);
        }
    }

    public void SliceObj(GameObject target)
    {
        if (target.CompareTag("Obstacle"))
        {
            gameController.GameOver();
        }

        Vector3 vel = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endtSlicePoint.position - startSlicePoint.position, vel);
        planeNormal.Normalize();

        SlicedHull hull = target.Slice(endtSlicePoint.position, planeNormal);

        if(hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, cuttedMaterial);
            SetupSlicedComponent(upperHull);
            GameObject lowerHull = hull.CreateLowerHull(target, cuttedMaterial);
            SetupSlicedComponent(lowerHull);


            Destroy(target);
        }
    }

    public void SetupSlicedComponent(GameObject slicedObj)
    {
        Rigidbody rb = slicedObj.AddComponent<Rigidbody>();
        MeshCollider collider = slicedObj.AddComponent<MeshCollider>();

        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObj.transform.position, 1);
    }

    public void DebugSlice()
    {
        SliceObj(target);

    }
}
