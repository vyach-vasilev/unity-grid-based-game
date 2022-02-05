using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlinePass : ScriptableRenderPass
{
    private const RenderTextureFormat RenderTextureFormat = UnityEngine.RenderTextureFormat.R8;

    private readonly string _profilerTag = "Outline Pass";
    private readonly List<ShaderTagId> _shaderTagIdList = new();
    
    private readonly int _mainTexId = Shader.PropertyToID("_MainTex");
    private readonly int _maskTexId = Shader.PropertyToID("_MaskTex");
    private readonly int _tempTexId = Shader.PropertyToID("_TempTex");
    private readonly int _colorId = Shader.PropertyToID("_Color");
    private readonly int _widthId = Shader.PropertyToID("_Width");
    private readonly int _gaussSamplesId = Shader.PropertyToID("_GaussSamples");
    
    private readonly RenderTargetIdentifier _maskTex = new("_MaskTex");
    private readonly RenderTargetIdentifier _tempTex = new("_TempTex");

    private readonly LayerMask _layerMask;
    private readonly Color _color;
    private readonly int _width;
    private readonly Material _colorMaterial;
    private readonly Material _outlineMaterial;
    
    private MaterialPropertyBlock _block;
    private float[][] _gaussSamples;
    private ScriptableRenderer _renderer;
    
    public OutlinePass(LayerMask layerMask, Color color, int width, Material colorMaterial, Material outlineMaterial, string[] shaderTags)
    {
        _layerMask = layerMask;
        _color = color;
        _width = width;
        _colorMaterial = colorMaterial;
        _outlineMaterial = outlineMaterial;
        
        if (shaderTags != null && shaderTags.Length > 0)
            foreach (var passName in shaderTags)
                _shaderTagIdList.Add(new ShaderTagId(passName));
        else
            _shaderTagIdList.Add(new ShaderTagId("UniversalForward"));
    }

    public void Setup(ScriptableRenderer renderer)
    {
        _renderer = renderer;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var camData = renderingData.cameraData;

        if (_layerMask != 0)
        {
            var buffer = CommandBufferPool.Get(_profilerTag);
            var filteringSettings = new FilteringSettings(RenderQueueRange.all, _layerMask);
            var renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);
            var sortingCriteria = camData.defaultOpaqueSortFlags;
            var drawingSettings = CreateDrawingSettings(_shaderTagIdList, ref renderingData, sortingCriteria);
            var depthTexture = new RenderTargetIdentifier("_CameraDepthTexture");

            drawingSettings.enableDynamicBatching = true;
            drawingSettings.overrideMaterial = _colorMaterial;
            drawingSettings.overrideMaterialPassIndex = 0;

            using (new ProfilingScope(buffer, new ProfilingSampler(_profilerTag)))
            {
                if (camData.cameraTargetDescriptor.width <= 0) camData.cameraTargetDescriptor.width = -1;
                if (camData.cameraTargetDescriptor.height <= 0) camData.cameraTargetDescriptor.height = -1;

                if (camData.cameraTargetDescriptor.dimension == TextureDimension.None ||
                    camData.cameraTargetDescriptor.dimension == TextureDimension.Unknown)
                    camData.cameraTargetDescriptor.dimension = TextureDimension.Tex2D;

                camData.cameraTargetDescriptor.shadowSamplingMode = ShadowSamplingMode.None;
                camData.cameraTargetDescriptor.depthBufferBits = 0;
                camData.cameraTargetDescriptor.colorFormat = RenderTextureFormat;
                camData.cameraTargetDescriptor.msaaSamples = 1;

                buffer.GetTemporaryRT(_maskTexId, camData.cameraTargetDescriptor, FilterMode.Bilinear);
                buffer.GetTemporaryRT(_tempTexId, camData.cameraTargetDescriptor, FilterMode.Bilinear);

                buffer.SetRenderTarget(_maskTex, depthTexture, 0, CubemapFace.Unknown, -1);
                buffer.ClearRenderTarget(false, true, Color.clear);
                context.ExecuteCommandBuffer(buffer);
                context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings, ref renderStateBlock);
                buffer.Clear();

                var propertyBlock = GetProperties();

                buffer.SetGlobalFloatArray(_gaussSamplesId, GetGaussSamples(_width));
                buffer.SetRenderTarget(_tempTex, 0, CubemapFace.Unknown, -1);
                buffer.SetGlobalTexture(_mainTexId, _maskTex);
                buffer.DrawProcedural(Matrix4x4.identity, _outlineMaterial, 0, MeshTopology.Triangles, 3, 1, propertyBlock);

                buffer.SetRenderTarget(_renderer.cameraColorTarget, 0, CubemapFace.Unknown, -1);
                buffer.SetGlobalTexture(_mainTexId, _tempTex);
                buffer.DrawProcedural(Matrix4x4.identity, _outlineMaterial, 1, MeshTopology.Triangles, 3, 1, propertyBlock);
            }

            context.ExecuteCommandBuffer(buffer);
            CommandBufferPool.Release(buffer);
        }
    }

    public override void FrameCleanup(CommandBuffer buffer)
    {
        buffer.ReleaseTemporaryRT(_tempTexId);
        buffer.ReleaseTemporaryRT(_maskTexId);
    }
    
    private MaterialPropertyBlock GetProperties()
    {
        if (_block is null) _block = new MaterialPropertyBlock();
        _block.SetFloat(_widthId, _width);
        _block.SetColor(_colorId, _color);
        return _block;
    }

    private float[] GetGaussSamples(int width)
    {
        var index = Mathf.Clamp(width, 1, 32) - 1;
        if (_gaussSamples is null) _gaussSamples = new float[32][];
        if (_gaussSamples[index] is null) _gaussSamples[index] = GetGaussSamples(width, null);
        return _gaussSamples[index];
    }
    
    private float[] GetGaussSamples(int width, float[] samples)
    {
        var stdDev = width * 0.5f;
        if (samples is null) samples = new float[32];
        for (var i = 0; i < width; i++) samples[i] = Gauss(i, stdDev);
        return samples;
    }

    private float Gauss(float x, float stdDev)
    {
        var stdDev2 = stdDev * stdDev * 2;
        var a = 1 / Mathf.Sqrt(Mathf.PI * stdDev2);
        var gauss = a * Mathf.Pow((float)Math.E, -x * x / stdDev2);
        return gauss;
    }
}