using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices; // MarShalを使う

struct Particle
{
    public Vector3 pos;
    public Vector3 vel;
    public Vector3 col;
}

public class ParticleBehaviourScript : MonoBehaviour
{
    [SerializeField]
    private Material material;// 描画用マテリアル

    [SerializeField]
    private ComputeShader computeShader;

    private int updateKernel;
    private ComputeBuffer buffer;// Particle配列

    static int THREAD_NUM = 64;
    static int PARTICLE_NUM = ((65536 + THREAD_NUM - 1) / THREAD_NUM) * THREAD_NUM;// THREAD_NUMの倍数にする

    void OnEnable() // オブジェクトが有効になったときに呼ばれる
    {
        // パーティクルの情報を格納するバッファ
        buffer = new ComputeBuffer(
            PARTICLE_NUM,
            Marshal.SizeOf(typeof(Particle)),
            ComputeBufferType.Default);

        // 初期化
        var initKernel = computeShader.FindKernel("initialize");
        computeShader.SetBuffer(initKernel, "Particles", buffer);
        computeShader.Dispatch(initKernel, PARTICLE_NUM / THREAD_NUM, 1, 1);


        // 更新関数の設定
        updateKernel = computeShader.FindKernel("update");
        computeShader.SetBuffer(updateKernel, "Particles", buffer);

        // 描画用マテリアルの設定
        material.SetBuffer("Particles", buffer);
    }

    void OnDisable() // オブジェクトが無効になったときに呼ばれる
    {
        buffer.Release(); // バッファ解放
    }

    // Update is called once per frame
    void Update()
    {
        // 動かす
        computeShader.SetFloat("deltaTime", Time.deltaTime);
        computeShader.Dispatch(updateKernel, PARTICLE_NUM / THREAD_NUM, 1, 1);
    }

    void OnRenderObject() 
    {
        // 描画
        material.SetPass(0);
        Graphics.DrawProceduralNow(MeshTopology.Points, PARTICLE_NUM);
    }

}