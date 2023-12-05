using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityVolumeRendering;
using System.IO;
using System.Linq;
using TMPro;

public class ImSeqImporter : MonoBehaviour{
    [SerializeField] private GameObject outerObject;
    [SerializeField] private VolumeRenderedObject volObj;
    [SerializeField] private GameObject meshContainer;
    private string modelRelativePath = @"D:\Foldernya Diablo\XR\FP\final-project-wfa\Assets\Model\DICOM\DCM";
    [SerializeField] private VolumeRenderedObject volumeRenderedObject;
    private string appPath;

    private void Start() {
        //appPath = Application.dataPath;
        appPath = modelRelativePath;
    }

    public void CallImport(){
        ModelImport(appPath);
    }

    public async void ModelImport(string dir){
        // loadingUI.SetActive(true);
        List<string> filePaths = Directory.GetFiles(dir).ToList();
        // Create importer
        IImageSequenceImporter importer = ImporterFactory.CreateImageSequenceImporter(ImageSequenceFormat.DICOM);
        // Load list of DICOM series (normally just one series)
        IEnumerable<IImageSequenceSeries> seriesList = await importer.LoadSeriesAsync(filePaths); // takes a long of time

        VolumeDataset dataset = await importer.ImportSeriesAsync(seriesList.First());

        MeshRenderer meshRenderer = meshContainer.GetComponent<MeshRenderer>();

        CreateObjectInternal(dataset, meshContainer, meshRenderer, volObj, outerObject);

        meshRenderer.sharedMaterial.SetTexture("_DataTex", dataset.GetDataTexture());
        //foreach(GameObject sliceRenderingPlane in sliceRenderingPlanes){
          //  SlicePlaneMat(sliceRenderingPlane, dataset);
        //}

        outerObject.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        meshContainer.transform.localScale = new Vector3(1f, 1f, 1f);
        // loadingUI.SetActive(false);
        Debug.Log("FINISH");
    }


    private static void CreateObjectInternal(VolumeDataset dataset, GameObject meshContainer, MeshRenderer meshRenderer, VolumeRenderedObject volObj, GameObject outerObject, IProgressHandler progressHandler = null)
    {            
        meshContainer.transform.parent = outerObject.transform;
        meshContainer.transform.localScale = Vector3.one;
        meshContainer.transform.localPosition = Vector3.zero;
        meshContainer.transform.parent = outerObject.transform;
        outerObject.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        meshRenderer.sharedMaterial = new Material(meshRenderer.sharedMaterial);
        volObj.meshRenderer = meshRenderer;
        volObj.dataset = dataset;

        const int noiseDimX = 512;
        const int noiseDimY = 512;
        Texture2D noiseTexture = NoiseTextureGenerator.GenerateNoiseTexture(noiseDimX, noiseDimY);

        UnityVolumeRendering.TransferFunction tf = TransferFunctionDatabase.CreateTransferFunction();
        Texture2D tfTexture = tf.GetTexture();
        volObj.transferFunction = tf;

        TransferFunction2D tf2D = TransferFunctionDatabase.CreateTransferFunction2D();
        volObj.transferFunction2D = tf2D;

        meshRenderer.sharedMaterial.SetTexture("_GradientTex", null);
        meshRenderer.sharedMaterial.SetTexture("_NoiseTex", noiseTexture);
        meshRenderer.sharedMaterial.SetTexture("_TFTex", tfTexture);

        meshRenderer.sharedMaterial.EnableKeyword("MODE_DVR");
        meshRenderer.sharedMaterial.DisableKeyword("MODE_MIP");
        meshRenderer.sharedMaterial.DisableKeyword("MODE_SURF");

        meshContainer.transform.localScale = dataset.scale;
        meshContainer.transform.localRotation = dataset.rotation;

        // Normalise size (TODO: Add setting for diabling this?)
        float maxScale = Mathf.Max(dataset.scale.x, dataset.scale.y, dataset.scale.z);
        volObj.transform.localScale = Vector3.one / maxScale;
    }

    private void SlicePlaneMat(GameObject sliceRenderingPlane, VolumeDataset dataset){
        Material sliceMat = sliceRenderingPlane.GetComponent<MeshRenderer>().sharedMaterial;
        sliceMat.SetTexture("_DataTex", dataset.GetDataTexture());
        sliceMat.SetTexture("_TFTex", volumeRenderedObject.transferFunction.GetTexture());
        sliceMat.SetMatrix("_parentInverseMat", volumeRenderedObject.transform.worldToLocalMatrix);
        sliceMat.SetMatrix("_planeMat", Matrix4x4.TRS(sliceRenderingPlane.transform.position, sliceRenderingPlane.transform.rotation, transform.lossyScale)); // TODO: allow changing scale

        SlicingPlane slicingPlaneComp = sliceRenderingPlane.GetComponent<SlicingPlane>();
        slicingPlaneComp.targetObject = volumeRenderedObject;
    }
}
