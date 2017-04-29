using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateRPS : MonoBehaviour {
	public ComputeShader m_RPS_compute;
	private RenderTexture m_texCurrentFrame;
	private RenderTexture m_texLastFrame;
	//private ComputeBuffer m_cbScores;
	private RenderTexture m_texScores;
	private int width = 1024;
	private int height = 1024;
	private float fDiffusionCoeff = 0.5f;
	private float fDiffusionPower = 1.01f;
	private float fNormPower = 0.9f;

	public RenderTexture GetCurrentFrameTexture()
	{
		return m_texCurrentFrame;
		//return m_texScores;
	}

	// Use this for initialization
	void Start () {
		m_texCurrentFrame = new RenderTexture (width,height,0);
		m_texCurrentFrame.format = RenderTextureFormat.ARGB32;
		m_texCurrentFrame.enableRandomWrite = true;
		m_texCurrentFrame.Create ();

		m_texLastFrame = new RenderTexture (width,height,0);
		m_texLastFrame.format = RenderTextureFormat.ARGB32;
		m_texLastFrame.enableRandomWrite = true;
		m_texLastFrame.Create ();

		//m_cbScores = new ComputeBuffer (width * height, sizeof(float));
		m_texScores = new RenderTexture(width,height,0);
		m_texScores.format = RenderTextureFormat.RFloat;
		m_texScores.enableRandomWrite = true;
		m_texScores.Create ();

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

	public void AddColor(Vector2 texCoords,Color color,float radius,float additionCoeff)
	{
		int setColKernel = m_RPS_compute.FindKernel ("SetColor");
		m_RPS_compute.SetFloat ("fSetColorRadius", radius);
		m_RPS_compute.SetFloat ("fSetColorCoeff", additionCoeff);
		m_RPS_compute.SetFloats ("colSetColor", new float[4] {color.r,color.g,color.b,color.a});
		m_RPS_compute.SetFloats ("vecSetColorCenterUV", new float[2] {texCoords.x,texCoords.y});
		m_RPS_compute.SetTexture (setColKernel, "Target", m_texCurrentFrame);
		m_RPS_compute.Dispatch (setColKernel,width / 8,height / 8,1);
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
		m_RPS_compute.SetTexture (scoresKernel, "Scores", m_texScores);
		//m_RPS_compute.SetBuffer (scoresKernel, "cbScores", m_cbScores);
		m_RPS_compute.Dispatch(scoresKernel,width / 8,height / 8,1);

		int nextFrameKernel = m_RPS_compute.FindKernel ("CalcNextFrame");
		m_RPS_compute.SetTexture (nextFrameKernel, "LastFrame", m_texCurrentFrame);
		m_RPS_compute.SetTexture (nextFrameKernel, "NextFrame", m_texLastFrame);
		m_RPS_compute.SetTexture(nextFrameKernel,"Scores", m_texScores);
		m_RPS_compute.Dispatch(nextFrameKernel,width / 8,height / 8,1);

		var temp = m_texLastFrame;
		m_texLastFrame = m_texCurrentFrame;
		m_texCurrentFrame = temp;
	}

}
