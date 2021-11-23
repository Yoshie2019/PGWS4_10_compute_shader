using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class MyParticleBehaviourScript : MonoBehaviour
{
    [SerializeField] ComputeShader MyCompute_shader;
    [SerializeField] Transform MoveObject;

    //GameObject Sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

    private ComputeBuffer This_Buffer;
    private Vector3 This_Center = Vector3.zero; 

    // Start is called before the first frame update
    void Start()
    {
        This_Buffer = new ComputeBuffer(1,Marshal.SizeOf(typeof(Vector2))); 
        MyCompute_shader.SetBuffer(MyCompute_shader.FindKernel("CSMain"), "ResultMyBuffer", This_Buffer); 
    }

    // Update is called once per frame
    void Update()
    {
        MyCompute_shader.SetFloats("ShaderSidePosition", This_Center.x, This_Center.y);
        MyCompute_shader.SetFloat("ShaderSideTime", Time.deltaTime);
        MyCompute_shader.Dispatch(0,8,8,1);

        var This_Date = new float[2];
        This_Buffer.GetData(This_Date);

        Vector3 This_Pos = MoveObject.transform.localPosition;
        This_Pos.x = This_Date[0];
        This_Pos.y = This_Date[1];

        MoveObject.transform.localPosition = This_Pos;
    }

    private void OnDestroy()
    {
        This_Buffer.Release();
    }

}
