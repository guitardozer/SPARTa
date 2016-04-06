//-----------------------------------------------------------------------
// <copyright file="MeshBuilderWithColorGUIController.cs" company="Google">
//
// Copyright 2016 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------
using System.Collections;
using Tango;
using UnityEngine;

/// <summary>
/// Extra GUI controls.
/// </summary>
public class MeshBuilderWithColorGUIController : MonoBehaviour
{
    /// <summary>
    /// Debug info: If the mesh is being updated.
    /// </summary>
    private bool m_isEnabled = true;

    private TangoApplication m_tangoApplication;
    private TangoDynamicMesh m_dynamicMesh;
    private Exporter m_exporter;
    private TangoMultiCamera m_tangoCamera;
    public GameObject measurePointStart;
    public GameObject measurePointEnd;
    private Camera mainCamera;
    private Vector3 pos;
    private TangoDeltaPoseController m_poseController;
    //private TextMesh textMesh;
    private float dist;

    public Texture pause;
	public Texture resume;
	public Texture clear;
	public Texture save;
    public Texture circle;
	public GUIStyle material;
    public string distanceOut;
    //public Mesh totalMesh = null;
    //private string output = "Nothing happened.";

    /// <summary>
    /// Start is used to initialize.
    /// </summary>
    public void Start()
    {
        m_tangoApplication = FindObjectOfType<TangoApplication>();
        m_dynamicMesh = FindObjectOfType<TangoDynamicMesh>();
        m_exporter = new Exporter();
        m_tangoCamera = FindObjectOfType<TangoMultiCamera>();
        m_poseController = FindObjectOfType<TangoDeltaPoseController>();
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        //TextMesh textMesh = gameObject.AddComponent<TextMesh>();
        Material lineMat = Resources.Load("Line", typeof(Material)) as Material;
        lineRenderer.material = lineMat;
        lineRenderer.SetWidth(0.01F, 0.01F);
        lineRenderer.SetVertexCount(2);
        lineRenderer.SetPosition(0, measurePointStart.transform.position);
        lineRenderer.SetPosition(1, measurePointEnd.transform.position);
    }

    /// <summary>
    /// Updates UI and handles player input.
    /// </summary>
    public void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        //TextMesh textMesh = GetComponent<TextMesh>();
        if (measurePointStart.transform.position.y != -22 && measurePointEnd.transform.position.y != -22)
        {
            lineRenderer.SetPosition(0, measurePointStart.transform.position);
            lineRenderer.SetPosition(1, measurePointEnd.transform.position);
            //textMesh.transform.parent = lineRenderer.transform;
            //Vector3 pos = Camera.main.WorldToScreenPoint(lineRenderer.transform.position);
            //textMesh.text = distanceOut;
            //textMesh.fontSize = 20;
            //textMesh.transform.position = pos;
        }
    }

    /// <summary>
    /// Draws the Unity GUI.
    /// </summary>
    public void OnGUI()
    {
        /*GUI.color = Color.white;
        if (GUI.Button(new Rect(Screen.width - 160, 20, 140, 80), "<size=30>Clear</size>"))
        {
            m_dynamicMesh.Clear();
            m_tangoApplication.Tango3DRClear();
        }
        
        string text = m_isEnabled ? "Pause" : "Resume";
        if (GUI.Button(new Rect(Screen.width - 160, 120, 140, 80), "<size=30>" + text + "</size>"))
        {
            m_isEnabled = !m_isEnabled;
            m_tangoApplication.Set3DReconstructionEnabled(m_isEnabled);
        }*/
        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        //mainCamera = GetComponent<Camera>();
        if (GUI.Button(new Rect(60, Screen.height - 200, 140, 140), clear, material))
		{
			m_dynamicMesh.Clear();
			m_tangoApplication.Tango3DRClear();
            measurePointStart.transform.position = new Vector3(0, -22, 0);
            measurePointEnd.transform.position = new Vector3(0, -22, 0);
            lineRenderer.SetPosition(0, measurePointStart.transform.position);
            lineRenderer.SetPosition(1, measurePointEnd.transform.position);
            dist = 0;
            distanceOut= null;
            GUI.Label(new Rect(Screen.width - 360, 420, 340, 50), "");
        }

		Texture text = m_isEnabled ? pause : resume;
		if (GUI.Button(new Rect(Screen.width - 200, Screen.height - 200, 140, 140), text, material))
		{
			m_isEnabled = !m_isEnabled;
			m_tangoApplication.Set3DReconstructionEnabled(m_isEnabled);
		}
        
		if (!m_isEnabled)
        {
            GUI.Button(new Rect(Screen.width/2 - 10, Screen.height/2 - 10, 20, 20), circle, material);
            // Save Button
            if (GUI.Button(new Rect(Screen.width - 400, Screen.height - 200, 140, 140), save, material))
			{
                //m_tangoApplication.GetComponent < "gridComponent" >;
                //Mesh m1 = obj1.GetComponent<MeshFilter>().mesh;
                //AssetDatabase.CreateAsset(m_dynamicMesh.totalMesh, "Assets/" + "meshTrial" + ".asset");
                //totalMesh = m_dynamicMesh.totalMesh;
                m_exporter.DoExport(true, m_dynamicMesh.name);
			}

            // Start Point Button
            if (GUI.Button(new Rect(Screen.width - 160, 220, 140, 80), "<size=20>Start Point</size>"))
            {
                setStartPoint();
            }

            // End Point Button
            if (GUI.Button(new Rect(Screen.width - 160, 320, 140, 80), "<size=20>End Point</size>"))
            {
                setEndPoint();
            }
        }
        if (measurePointStart.transform.position.y != -22 && measurePointEnd.transform.position.y != -22)
        {

            dist = Vector3.Distance(measurePointStart.transform.position, measurePointEnd.transform.position);
            //distanceOut = GUI.TextField(new Rect(Screen.width - 360, 420, 340, 50), dist.ToString() + " meters");
            GUI.color = Color.black;
            GUI.contentColor = Color.gray;
            GUI.Label(new Rect(Screen.width - 360, 420, 340, 50), string.Format("<size=40>{0} meters</size>", dist.ToString()));
            //Vector3 startScreenPos = mainCamera.WorldToScreenPoint(measurePointStart.transform.position);
            //Vector3 endScreenPos = mainCamera.WorldToScreenPoint(measurePointEnd.transform.position);
            //distanceOut = GUI.TextField(new Rect(startScreenPos.x, startScreenPos.y, 340, 50), dist.ToString() + " meters");
            distanceOut = dist.ToString() + " meters";
            //GUI.skin.label.fontSize = 40;
        }
        
    }




    public void setStartPoint()
    {
        Vector3 startpoint = Vector3.zero;
        var startray = new Ray(m_poseController.m_tangoPosition, m_poseController.transform.forward);
        RaycastHit hit; // declare the RaycastHit variable
        if (Physics.Raycast(startray, out hit))
        {
            startpoint = hit.point;
            measurePointStart.transform.position = startpoint;
            measurePointStart.transform.rotation = m_poseController.m_tangoRotation;
        }
    }

    public void setEndPoint()
    {
        Vector3 endpoint = Vector3.zero;
        var endray = new Ray(m_poseController.m_tangoPosition, m_poseController.transform.forward);
        RaycastHit hit; // declare the RaycastHit variable
        if (Physics.Raycast(endray, out hit))
        {
            endpoint = hit.point;
            measurePointEnd.transform.position = endpoint;
            measurePointStart.transform.rotation = m_poseController.m_tangoRotation;
        }
    }

}
