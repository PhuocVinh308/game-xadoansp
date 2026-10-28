using UnityEngine;

namespace CaiSonHistoryGame.Gameplay
{
    /// <summary>
    /// Điều khiển từng xe trong đoàn quân sự (Xe Jeep chỉ huy hoặc Xe tải hộ tống).
    /// </summary>
    public class ConvoyVehicle : MonoBehaviour
    {
        [Header("Thuộc tính xe")]
        public bool isLeadVehicle = false; // Đánh dấu là xe Jeep của Khưu Văn Ba
        public float normalSpeed = 6.0f;
        public float currentSpeed;

        [Header("Hiệu ứng & Trạng thái")]
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject fireSmokePrefab;
        [SerializeField] private AudioSource engineAudio;
        [SerializeField] private AudioSource hitAudio;

        [Header("Lắc lư địa hình (Suspension Bobbing)")]
        [SerializeField] private float bobbingFrequency = 4.0f;
        [SerializeField] private float bobbingAmplitude = 0.05f;

        private Vector3 initialLocalPos;
        private bool isDestroyed = false;
        private bool isPanicking = false;
        private float panicTimer = 0f;
        private Vector3 panicSwerveDirection;

        private void Start()
        {
            currentSpeed = normalSpeed;
            initialLocalPos = transform.localPosition;
            if (engineAudio != null && !engineAudio.isPlaying)
            {
                engineAudio.loop = true;
                engineAudio.Play();
            }
        }

        private void Update()
        {
            if (isDestroyed) return;

            // Xử lý khi xe hộ tống bị hoảng loạn
            if (isPanicking)
            {
                HandlePanicState();
                return;
            }

            // Chạy bình thường theo trục Z (tiến về phía trước)
            transform.Translate(Vector3.forward * (currentSpeed * Time.deltaTime), Space.Self);

            // Giả lập rung lắc đường đất gồ ghề
            float bob = Mathf.Sin(Time.time * bobbingFrequency) * bobbingAmplitude;
            transform.localPosition = new Vector3(transform.localPosition.x, initialLocalPos.y + bob, transform.localPosition.z);
        }

        /// <summary>
        /// Kích hoạt khi trúng đòn phục kích
        /// </summary>
        public void ApplyDirectHit()
        {
            isDestroyed = true;
            currentSpeed = 0f;

            if (engineAudio != null) engineAudio.Stop();
            if (hitAudio != null) hitAudio.Play();

            // Hiệu ứng nổ tung
            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            // Khói lửa bốc lên
            if (fireSmokePrefab != null)
            {
                GameObject fire = Instantiate(fireSmokePrefab, transform.position, Quaternion.identity);
                fire.transform.parent = transform;
            }

            // Xe bị lật hoặc nghiêng do trúng đạn mìn/súng trường
            transform.rotation *= Quaternion.Euler(UnityEngine.Random.Range(5f, 15f), 0, UnityEngine.Random.Range(10f, 25f));
            transform.position += Vector3.down * 0.15f;
        }

        /// <summary>
        /// Kích hoạt trạng thái hoảng loạn của xe vận tải hộ tống
        /// </summary>
        public void TriggerChaos()
        {
            if (isLeadVehicle || isDestroyed) return;

            isPanicking = true;
            panicTimer = UnityEngine.Random.Range(1.5f, 3.0f);
            // Lạng lách dạt sang lề đường / mép kênh rạch
            panicSwerveDirection = new Vector3(UnityEngine.Random.Range(-1f, 1f) > 0 ? 0.6f : -0.6f, 0, 0.4f).normalized;
        }

        private void HandlePanicState()
        {
            panicTimer -= Time.deltaTime;
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * 1.5f);

            // Lao vào lề đường rồi dừng hẳn
            transform.Translate(panicSwerveDirection * (currentSpeed * Time.deltaTime), Space.World);
            transform.Rotate(Vector3.up, UnityEngine.Random.Range(-15f, 15f) * Time.deltaTime);

            if (panicTimer <= 0f)
            {
                currentSpeed = 0f;
            }
        }
    }
}
