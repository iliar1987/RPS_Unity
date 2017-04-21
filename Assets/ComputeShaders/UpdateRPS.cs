using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRPS : MonoBehaviour {
	public ComputeShader m_RPS_compute;
	private RenderTexture m_texCurrentFrame;
	private RenderTexture m_texLastFrame;
	private int width = 256;
	private int height = 256;

	public RenderTexture GetCurrentFrameTexture()
	{
		return m_texCurrentFrame;
	}

	// Use this for initialization
	void Start () {
		m_texCurrentFrame = new RenderTexture (width,height,0);
		m_texCurrentFrame.enableRandomWrite = true;
		m_texCurrentFrame.Create ();

		m_texLastFrame = new RenderTexture (width,height,0);
		m_texLastFrame.enableRandomWrite = true;
		m_texLastFrame.Create ();

		Randomize ();
	}
	
	// Update is called once per frame
	void Update () {
//
//		if (!m_texCurrentFrame.IsCreated ()) {
//			m_texCurrentFrame.Create ();
//			RunShader ();
//		}
	}

	void Randomize()
	{
		int kernelHandle = m_RPS_compute.FindKernel ("Randomize");
		m_RPS_compute.SetTexture (kernelHandle, "Result", m_texCurrentFrame);
		m_RPS_compute.Dispatch (kernelHandle,width / 8,height / 8,1);

	}

}
