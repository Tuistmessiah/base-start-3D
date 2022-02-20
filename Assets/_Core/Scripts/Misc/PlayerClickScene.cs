using UnityEngine;

public class PlayerClickScene : MonoBehaviour
{

    // > Inputs
    int _mouseLeft = 0, _mouseRight = 1, _mouseMiddle = 2;

    // > State
    public bool _isGoingToCast = true;
    int _clickLeftIteration = 0;
    int _clickRightIteration = 0;
    int _clickMiddleIteration = 0;

    // > Refs
    Camera _mainCam = null;
    public GameObject _triggerObject;
    public GameObject _otherTriggerObject;
    public GameObject _GroundSlamBlue;
    public GameObject _StormNovaGreen;
    public GameObject _BlackHoleExplosionYellow;
    public GameObject _ChargeSphereBlue;


    void Start()
    {
        if (!_mainCam) _mainCam = Camera.main;
    }

    void Update()
    {
        CheckInputs();
    }

    /** Check mouse inputs and add iterative calls */
    void CheckInputs()
    {
        if (_isGoingToCast)
        {

            // Pressed Left Mouse
            if (Input.GetMouseButtonDown(_mouseLeft))
            {
                Ray mouseRay = _mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(mouseRay, out hit, 30f))
                {
                    switch (_clickLeftIteration)
                    {
                        case 0:
                            // Test 1
                            break;
                        case 1:
                            // Test 2
                            _clickLeftIteration = 0;
                            break;
                    }

                    _clickLeftIteration++;
                }
            }

            // Pressed Right Mouse
            if (Input.GetMouseButtonDown(_mouseRight))
            {
                Ray mouseRay = _mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(mouseRay, out hit, 30f))
                {
                    switch (_clickRightIteration)
                    {
                        case 0:
                            break;
                        case 1:
                            _clickRightIteration = 0;
                            break;
                    }
                    
                    _clickRightIteration++;
                }
            }

            // Pressed Middle Mouse
            if (Input.GetMouseButtonDown(_mouseMiddle))
            {
                Ray mouseRay = _mainCam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(mouseRay, out hit, 30f))
                {
                    switch (_clickMiddleIteration)
                    {
                        case 0:
                            break;
                        case 1:
                            _clickMiddleIteration = 0;
                            break;
                    }

                    _clickMiddleIteration++;
                }
            }
        }

    }
}