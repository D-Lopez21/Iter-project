using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] CinemachineVirtualCamera[] allVirtualCameras;
    private CinemachineVirtualCamera currentCamera;
    private CinemachineFramingTransposer framingTransposer;
    public static CameraManager Instance;



    private void Awake(){
        if(Instance == null){
            Instance = this;
        }

        for(int i = 0; i < allVirtualCameras.Length; i++){

            if(allVirtualCameras[i].enabled){

                currentCamera = allVirtualCameras[i];
                framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
            }
        }
    }

    private void Start(){
        
        for(int i = 0; i < allVirtualCameras.Length; i++){

            allVirtualCameras[i].Follow = PlayerController.Instance.transform;
        }
    }

    public void SwapCamera(CinemachineVirtualCamera _newCam){

        currentCamera.enabled = false;
        currentCamera = _newCam;
        currentCamera.enabled = true;
    }
}
