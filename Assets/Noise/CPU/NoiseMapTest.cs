using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using nobnak.Gist;
using CPUNoise.Data;
using Cysharp.Threading.Tasks;

namespace CPUNoise
{
    public class NoiseMapTest : MonoBehaviour
    {
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


        [SerializeField] private TsuruData _tsuruData;
        private System.Threading.Timer debounceTimer;

        public TsuruData tsuruData
        {
            get => _tsuruData;
            set
            {
                if (_tsuruData != value)
                {
                    _tsuruData = value;
                }
            }
        }

        private Material unlitMaterial;
        private GameObject debugSpheresParent;
        private GameObject[] debugSpheres;
        private int _numTsurus = 10000;
        private List<Vector3> positions;
        private Vector2 normalizedPos;
        private Vector2 randomWalkStepSize = new Vector2(0.15f, 0.25f);
        private TsuruData lastTsuruData;


        void Start()
        {
            debounceTimer = null;
            lastTsuruData = _tsuruData;

            this.debugSpheresParent = new GameObject("DebugSpheresParent");
            this.debugSpheresParent.transform.parent = this.transform;

            this.unlitMaterial = new Material(Shader.Find("Unlit/Color"));
            this.unlitMaterial.color = Color.red;

            this.normalizedPos = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

            CreateSphere();
            ShuffleTsuruPositions();
        }


        void Update()
        {
            // キー入力を検出してShuffleTsuruPositionsを実行
            //if (Input.GetKeyDown(KeyCode.R))
            //{
            //    ShuffleTsuruPositions();
            //}
        }


        void OnValidate()
        {
            DebounceShufflePositions(1000);
            lastTsuruData = _tsuruData; // 最新の状態を保存
        }

        void CreateSphere()
        {
            var mapData = tsuruData.tsuruLandingMapData;
            for (int i = 0; i < _numTsurus; i++)
            {
                if (mapData.showMap)
                {
                    if (debugSpheres == null)
                        this.debugSpheres = new GameObject[_numTsurus];
                    debugSpheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    debugSpheres[i].transform.parent = debugSpheresParent.transform;
                }
            }
        }


        private void CalcRandomWalkPosition()
        {
            float angle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
            float deltaX = Mathf.Cos(angle) * Random.Range(randomWalkStepSize.x, randomWalkStepSize.y);
            float deltaY = Mathf.Sin(angle) * Random.Range(randomWalkStepSize.x, randomWalkStepSize.y);

            normalizedPos += new Vector2(deltaX, deltaY);
            normalizedPos.x = Mathf.Clamp(normalizedPos.x, 0.0f, 1.0f);
            normalizedPos.y = Mathf.Clamp(normalizedPos.y, 0.0f, 1.0f);
        }

        void ShuffleTsuruPositions()
        {
            this.positions = new List<Vector3>();
            var mapData = tsuruData.tsuruLandingMapData;
            int j = 0;

            while (positions.Count < _numTsurus )
            {
                if (j > 100000) {
                    Debug.LogError("Infinite loop detected on TsuruLanding calculation.");
                    break;
                }

                //Vector2 nomalizedPos = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                CalcRandomWalkPosition();

                // この条件は noiseValue が高いほど true になる確率が高くなる
                if (IsInMap(normalizedPos))
                {
                    float x = normalizedPos.x * 10;
                    float z = normalizedPos.y * 10;
                    var newPos = new Vector3(x, 0, z);
                    positions.Add(newPos);
                }

                j++;
            }

            // リストの位置にツルを配置
            for (int i = 0; i < _numTsurus; i++)
            {
                if (mapData.showMap && i < positions.Count)
                {
                    debugSpheres[i].transform.position = positions[i];
                    debugSpheres[i].transform.localScale = mapData.debugSphereSize * Vector3.one;
                    debugSpheres[i].GetComponent<Renderer>().sharedMaterial = unlitMaterial;
                }
            }
        }



        //楕円の形のディゾルブでフィルタリング
        private bool IsInEllipseDissolve(Vector2 nomalizedPos, Vector2 ellipseCenter, Vector2 ellipseRadius, float dissolveAmount)
        {
            var mapData = tsuruData.tsuruLandingMapData;
            float distance = Mathf.Sqrt(
                Mathf.Pow((nomalizedPos.x - ellipseCenter.x) / ellipseRadius.x, 2) +
                Mathf.Pow((nomalizedPos.y - ellipseCenter.y) / ellipseRadius.y, 2)
            );
            float snoiseForDissolve = (1.0f + (float)SimplexNoise.Noise(
                (double)(nomalizedPos.x * mapData.dissolveNoiseMapScaleAndOffset.x + mapData.dissolveNoiseMapScaleAndOffset.z),
                (double)(nomalizedPos.y * mapData.dissolveNoiseMapScaleAndOffset.y + mapData.dissolveNoiseMapScaleAndOffset.w)
            )) / 2.0f; // 0 ~ 1
            float dissolveFactor = 1 - distance + (snoiseForDissolve * distance * dissolveAmount); // 0.1はノイズの影響度合い
            return dissolveFactor > 0.0f;
        }


        private bool IsInMap(Vector2 nomalizedPos)
        {
            var mapData = tsuruData.tsuruLandingMapData;

            float snoiseForMap = (1.0f + (float)SimplexNoise.Noise(
                (double)(nomalizedPos.x * mapData.noiseMapScaleAndOffset.x + mapData.noiseMapScaleAndOffset.z),
                (double)(nomalizedPos.y * mapData.noiseMapScaleAndOffset.y + mapData.noiseMapScaleAndOffset.w)
            )) / 2.0f; // 0 ~ 1

            bool isInArea1 = IsInEllipseDissolve(nomalizedPos, mapData.area1EllipseCenter, mapData.area1EllipseRadius, mapData.dissolveAmount);
            bool isInArea2 = IsInEllipseDissolve(nomalizedPos, mapData.area2EllipseCenter, mapData.area2EllipseRadius, mapData.dissolveAmount);

            //return (snoiseForMap > mapData.noiseCutThreshold && isInArea1);
            return (isInArea1 || isInArea2) && snoiseForMap > mapData.noiseCutThreshold;

            //Debug.LogFormat("nomalizedPos.x: {0}, nomalizedPos.z: {2}", nomalizedPos.x, nomalizedPos.y);
            //Debug.LogFormat("noiseCutThreshold = {0}, snoiseForMap = {1}", mapData.noiseCutThreshold, snoiseForMap);
        }

        private async UniTaskVoid DebounceShufflePositions(int delay)
        {
            cancellationTokenSource.Cancel(); // 既存のタスクがあればキャンセル
            cancellationTokenSource.Dispose(); // 既存のCancellationTokenSourceを破棄
            cancellationTokenSource = new CancellationTokenSource(); // 新しいCancellationTokenSourceを作成

            try
            {
                await UniTask.Delay(delay, cancellationToken: cancellationTokenSource.Token); // 新しいトークンで非同期待機
                ShuffleTsuruPositions(); // タイマーが完了した後に実行
            }
            catch (System.OperationCanceledException)
            {
                Debug.Log("ShuffleTsuruPositions was cancelled.");
                // キャンセルされた場合の処理
            }
        }

    void OnDestroy()
    {
        cancellationTokenSource.Cancel(); // コンポーネントが破棄されるときにタスクをキャンセル
        cancellationTokenSource.Dispose(); // リソースのクリーンアップ
    }

    }
}
