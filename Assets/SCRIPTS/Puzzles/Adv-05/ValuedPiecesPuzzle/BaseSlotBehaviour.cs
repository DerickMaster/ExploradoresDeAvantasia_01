using UnityEngine;
using System.Linq;

public class BaseSlotBehaviour : InteractableObject
{
    [SerializeField] private int baseSize;
    [SerializeField] private int heightSize;
    [SerializeField] private LineSize[] slotsMatrix;
    [SerializeField] private int[] expectedIds;
    private int curBase = 0;
    private int startingPoint;

    private new void Start()
    {
        base.Start();
        Quaternion finalRotation = transform.rotation;
        transform.rotation = Quaternion.identity;
        GenerateSlots();
        transform.rotation = finalRotation;
    }

    [ContextMenu("CreateSlots")]
    public void GenerateSlots()
    {
        slotsMatrix = new LineSize[baseSize];
        for (int i = 0; i < slotsMatrix.Length; i++)
        {
            slotsMatrix[i] = new LineSize { line = new bool[heightSize] };
        }
        CreateSlotsMatrix();
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractionController interactor = other.GetComponent<InteractionController>();
        if(interactor && interactor.HoldingObject)
        {
            if (expectedIds.Contains<int>(interactor.heldObject.GetComponent<GrabbableObject>().objectID) && CheckPiece(interactor))
            {
                Debug.Log("entrou");
                previewObj.SetActive(true);
                previewObj.GetComponentInChildren<MeshFilter>().sharedMesh = interactor.heldObject.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        previewObj.SetActive(false);
    }

    private bool CheckPiece(InteractionController interactor)
    {
        int startingPoint = -1;
        this.startingPoint = -1;
        LineSize[] temp = interactor.heldObject.GetComponent<PieceBehaviour>().GetMatrixSize();
        if(CheckPieceFit(temp, out startingPoint))
        {
            ShowPiecePreview(startingPoint, temp.Length);
            this.startingPoint = startingPoint;
            return true;
        }
        return false;
    }

    [SerializeField] private GameObject previewObj;
    private void ShowPiecePreview(int startingPoint, int baseSize)
    {
        physicalSlots[curBase, startingPoint].transform.parent = previewObj.transform;
        previewObj.transform.localPosition = Vector3.zero;
        //previewObj.transform.position = physicalSlots[curBase, startingPoint].transform.position + (Vector3.up * 0.75f);
        previewObj.transform.rotation = gameObject.transform.rotation;
    }

    public override void Interact(InteractionController interactor)
    {
        if (startingPoint >= 0)
        {
            LineSize[] matrix = interactor.heldObject.GetComponent<PieceBehaviour>().GetMatrixSize();
            FillMatrixSlots(matrix, startingPoint);

            Vector3 tempOffset = new Vector3();
            if (matrix.Length % 2 == 0) tempOffset.x = offsetX/2; 
            GrabbableObjectInteractions.ReceiveItem(interactor, physicalSlots[curBase, startingPoint], tempOffset).transform.rotation = gameObject.transform.rotation;
            CheckLineFilled();
        }
    }

    [SerializeField] float offsetX;
    [SerializeField] float offsetY;
    [SerializeField] Vector3 originOffset;
    [SerializeField] GameObject slotPrefab;
    private GameObject[,] physicalSlots;

    
    private void CreateSlotsMatrix()
    {
        physicalSlots = new GameObject[baseSize, baseSize];
        Vector3 initialPos = new Vector3(0, 0, 0);

        for (int i = 0; i < slotsMatrix[0].Length; i++)
        {
            initialPos.x = 0f;
            for (int j = 0; j < slotsMatrix.Length; j++)
            {
                physicalSlots[i,j] = Instantiate(slotPrefab, (transform.position + initialPos) + originOffset, Quaternion.identity , transform);
                initialPos.x += offsetX;
            }
            initialPos.y += offsetY;
        }
    }


    public int ReceivePiece(PieceBehaviour piece)
    {
        int startingPoint = 0;
        if (CheckPieceFit(piece.GetMatrixSize(), out startingPoint))
        {
            FillMatrixSlots(piece.GetMatrixSize(), startingPoint);
            return startingPoint;
        }
        else return -1;
    }

    private bool CheckPieceFit(LineSize[] matrix, out int startingPoint)
    {
        startingPoint = 0;
        int straightEmptySlots = 0;

        if (slotsMatrix.Length < matrix.Length)
        {
            return false;
        }
        
        //check if the new matrix fits horizontally
        for (int i = 0; i < slotsMatrix.Length; i++)
        {
            if (slotsMatrix[i].line[curBase])
                straightEmptySlots = 0;
            else
            {
                if (straightEmptySlots == 0)
                    startingPoint = i;
                straightEmptySlots++;
                if (straightEmptySlots >= matrix.Length) break;
            }
        }
        if (straightEmptySlots < matrix.Length)
        {
            return false;
        }

        straightEmptySlots = 0;
        //check if the new matrix fits vertically
        for (int j = curBase; j < slotsMatrix[startingPoint].line.Length; j++)
        {
            if (slotsMatrix[startingPoint].line[j])
                straightEmptySlots = 0;
            else
            {
                straightEmptySlots++;
            }
        }
        if (straightEmptySlots < matrix[0].line.Length)
        {
            Debug.Log("menor");
            return false;
        }

        return true;
    }

    private void FillMatrixSlots(LineSize[] matrixToPut, int startingPoint)
    {
        for (int i = 0; i < matrixToPut.Length; i++)
        {
            for (int j = 0; j < matrixToPut[0].line.Length; j++)
            {
                slotsMatrix[startingPoint + i].line[curBase + j] = true;
            }
        }
    }

    private void CheckLineFilled()
    {
        if (curBase >= slotsMatrix[0].line.Length) 
        {
            AllLinesFilled();
            return;
        } 

        for (int i = 0; i < slotsMatrix.Length; i++)
        {
            if (!slotsMatrix[i].line[curBase]) return;
        }
        curBase++;
        CheckLineFilled();
    }

    private void AllLinesFilled()
    {
        DeactivateInteractable();
        Debug.Log("Completed");
    }
}