using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private float _jumpForce = 5.0f;

    private Rigidbody rb;
    private PlayerInputs _playerInputs;
    private Vector2 _moveInputValue;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        // Actionスクリプトのインスタンス生成
        _playerInputs = new PlayerInputs();

        // Actionイベント登録
        _playerInputs.Player.Move.started += OnMove;
        _playerInputs.Player.Move.performed += OnMove;
        _playerInputs.Player.Move.canceled += OnMove;
        _playerInputs.Player.Jump.performed += OnJump;

        // Input Actionを機能させるためには、有効化する必要がある
        _playerInputs.Enable();
    }

    private void OnDestroy()
    {
        // 自身でインスタンス化したActionクラスはIDisposableを実装しているので、
        // 必ずDisposeする必要がある
        _playerInputs?.Dispose();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Moveアクションの入力取得
        _moveInputValue = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        // ジャンプする力を与える
        rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
    }

    private void FixedUpdate()
    {
        // 一定速度で移動
        Vector3 movement = new Vector3(_moveInputValue.x, 0, _moveInputValue.y);
        rb.linearVelocity = new Vector3(movement.x * speed, rb.linearVelocity.y, movement.z * speed);
    }
}
