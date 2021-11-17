using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeShaderScript : MonoBehaviour
{
    [SerializeField]
    private ComputeShader compute_shader;

    // Start is called before the first frame update
    void Start()
    {

        int x = 8;
        int y = 8;
        ComputeBuffer buffer = new ComputeBuffer(x * y,sizeof(float) * 4); //�o�b�t�@�̊m��

        int kernel = compute_shader.FindKernel("CSMain"); // �v���O�����̔ԍ��̎擾
        compute_shader.SetBuffer(kernel, "Result", buffer); // �o�b�t�@�̔ԍ�
        compute_shader.Dispatch(kernel, x / 8, y / 8, 1); // Compute shader�𓮂���

        float[] date = new float[4 * x * y];
        buffer.GetData(date); // GPU����GPU�Ƀf�[�^�������Ă���


        buffer.Release(); // �o�b�t�@�̉��U

        // �m�F�p�ɗ����̂�\�����Ă݂�
        for (int i = 0; i < x * y; i++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            var c0 = date[4 * i + 0];
            var c1 = date[4 * i + 1] * 10.0f;
            var c2 = date[4 * i + 2] * 10.0f;
            Debug.Log(c0 + " " + c1 + " " + c2);
            cube.transform.Translate(c0, c1, c2);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
