using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    // 인스펙터에서 할당 (부트스트랩 씬의 UI 컴포넌트)
    public Slider loadingSlider;
    public string sceneToLoad = "GameScene"; // 로드할 실제 게임 씬 이름

    private void Start()
    {
        // 🌟 이 시점에는 모든 매니저(PlayerManager, InputManager 등)의 
        // Awake()와 Start()가 완료된 상태입니다.
        StartCoroutine(LoadAsyncScene());
    }

    private IEnumerator LoadAsyncScene()
    {
        // 1. 비동기 로드 작업 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        
        // 씬 로드가 완료되기 직전 (0.9)에 멈추도록 설정 (선택 사항, 로딩 바 제어에 유용)
        // operation.allowSceneActivation = false; 

        // 2. 진행률(progress) 계산 및 UI 업데이트
        while (!operation.isDone)
        {
            // Unity의 progress는 0.9에서 멈추므로, 0.9로 나누어 0~1 값으로 정규화합니다.
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            
            // 로딩 슬라이더 값 업데이트
            loadingSlider.value = progress;
            
            // Debug.Log($"Loading Progress: {progress * 100:F0}%");

            yield return null; // 다음 프레임까지 대기
        }

        // 3. 로드 완료
        // 만약 allowSceneActivation = false를 사용했다면, 여기서 true로 설정하여 씬을 활성화합니다.
        // operation.allowSceneActivation = true;
    }
}