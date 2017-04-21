using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRPS : MonoBehaviour {
	public ComputeShader m_RPS_compute;
	private RenderTexture m_texCurrentFrame;
	private RenderTexture m_texLastFrame;
	private ComputeBuffer m_cbScores;
	private int width = 1024;
	private int height = 1024;
	private float fDiffusionCoeff = 0.5f;
	private float fDiffusionPower = 1.1f;
	private float fNormPower = 0.8f;

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

		m_cbScores = new ComputeBuffer (width * height, sizeof(float));

		m_RPS_compute.SetInt ("width", width);
		m_RPS_compute.SetInt ("height", height);

		Randomize ();
	}
	
	// Update is called once per frame
	void Update () {
//
//		if (!m_texCurrentFrame.IsCreated ()) {
//			m_texCurrentFrame.Create ();
//			RunShader ();
//		}

		MakeOneStep ();
	}

	void Randomize()
	{
		int kernelHandle = m_RPS_compute.FindKernel ("Randomize");
		m_RPS_compute.SetTexture (kernelHandle, "Result", m_texCurrentFrame);
		m_RPS_compute.Dispatch (kernelHandle,width / 8,height / 8,1);

	}

	void MakeOneStep()
	{
		m_RPS_compute.SetFloat ("fDiffusionPower", fDiffusionPower);
		m_RPS_compute.SetFloat ("fDiffusionCoeff", fDiffusionCoeff);
		m_RPS_compute.SetFloat ("fNormPower", fNormPower);

		int scoresKernel = m_RPS_compute.FindKernel ("CalcScores");
		m_RPS_compute.SetTexture (scoresKernel, "LastFrame", m_texCurrentFrame);
		m_RPS_compute.SetBuffer (scoresKernel, "cbScores", m_cbScores);
		m_RPS_compute.Dispatch(scoresKernel,width / 8,height / 8,1);

		int nextFrameKernel = m_RPS_compute.FindKernel ("CalcNextFrame");
		m_RPS_compute.SetTexture (nextFrameKernel, "LastFrame", m_texCurrentFrame);
		m_RPS_compute.SetTexture (nextFrameKernel, "NextFrame", m_texLastFrame);
		m_RPS_compute.SetBuffer(nextFrameKernel,"cbScores", m_cbScores);
		m_RPS_compute.Dispatch(nextFrameKernel,width / 8,height / 8,1);

		var temp = m_texLastFrame;
		m_texLastFrame = m_texCurrentFrame;
		m_texCurrentFrame = temp;
	}

}
