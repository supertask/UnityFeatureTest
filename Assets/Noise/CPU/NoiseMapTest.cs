using System.Threading;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using nobnak.Gist;

using Cysharp.Threading.Tasks;

using CPUNoise.Data;

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


        public TsuruLandingData tsuruLandingData
        {
            get { return tsuruData.tsuruLandingData; }
        }
        public TsuruLandingMapData mapData
        {
            get { return tsuruData.tsuruLandingMapData; }
        }

        private Material unlitMaterial;
        private GameObject debugSpheresParent;
        private GameObject[] debugSpheres;
        private int _numTsurus = 10000;
        private List<Vector3> positions;
        private Vector2 nextPosition;
        private Vector2 randomWalkStepSize = new Vector2(0.15f, 0.25f);
        private TsuruData lastTsuruData;
        private Vector2[] previousPositions;


        private int randomSeed = 0;
        private int mapSeed = 0;

        void Start()
        {
            debounceTimer = null;
            lastTsuruData = _tsuruData;

            this.debugSpheresParent = new GameObject("DebugSpheresParent");
            this.debugSpheresParent.transform.parent = this.transform;

            this.unlitMaterial = new Material(Shader.Find("Unlit/Color"));
            this.unlitMaterial.color = Color.red;

            this.nextPosition = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));

            this.previousPositions = new Vector2[2];
            this.previousPositions[0] = new Vector2(
                mapData.area1EllipseCenter.x + Random.Range(mapData.area1EllipseRadius.x, mapData.area1EllipseRadius.y),
                mapData.area1EllipseCenter.y + Random.Range(mapData.area1EllipseRadius.x, mapData.area1EllipseRadius.y)
            );
            this.previousPositions[1] = new Vector2(
                mapData.area2EllipseCenter.x + Random.Range(mapData.area2EllipseRadius.x, mapData.area2EllipseRadius.y),
                mapData.area2EllipseCenter.y + Random.Range(mapData.area2EllipseRadius.x, mapData.area2EllipseRadius.y)
            );

            var mapSeedList = mapData.mapSeeds.Split(',').Select(int.Parse).ToList();
            var randomSeedList = tsuruLandingData.randomSeeds.Split(',').Select(int.Parse).ToList();
            this.mapSeed = mapSeedList[Random.Range(0, mapSeedList.Count)];
            this.randomSeed = randomSeedList[Random.Range(0, randomSeedList.Count)];

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


        private Vector3 GetRandomWalkPosition(int groupIndex)
        {
            float angle = Random.Range(0.0f, 360.0f) * Mathf.Deg2Rad;
            float deltaX = Mathf.Cos(angle) * Random.Range(mapData.randomWalkStepSize.x, mapData.randomWalkStepSize.y);
            float deltaY = Mathf.Sin(angle) * Random.Range(mapData.randomWalkStepSize.x, mapData.randomWalkStepSize.y);

            this.previousPositions[groupIndex] += new Vector2(deltaX, deltaY);

            // 1.0で剰余を取る前に、負の値を正の範囲に調整
            float wrapX = (this.previousPositions[groupIndex].x % 1.0f + 1.0f) % 1.0f;
            float wrapY = (this.previousPositions[groupIndex].y % 1.0f + 1.0f) % 1.0f;


            if (groupIndex == 0) {
                this.previousPositions[groupIndex].x = wrapX % 0.5f;
            } else if (groupIndex == 1) {
                this.previousPositions[groupIndex].x = 0.5f + (wrapX % 0.5f);
            }
            this.previousPositions[groupIndex].y = wrapY % 1.0f;
            return this.previousPositions[groupIndex];
        }


        void ShuffleTsuruPositions()
        {
            this.positions = new List<Vector3>();
            var mapData = tsuruData.tsuruLandingMapData;
            int index = 0;

            while (positions.Count < _numTsurus )
            {
                if (index > 100000) {
                    Debug.LogError("Infinite loop detected on TsuruLanding calculation.");
                    break;
                }

                //Vector2 nomalizedPos = new Vector2(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                Vector3 nextPosition;
                if (index % 2 == 0) {
                    nextPosition = GetRandomWalkPosition(0);
                } else {
                    nextPosition = GetRandomWalkPosition(1);
                }

                // この条件は noiseValue が高いほど true になる確率が高くなる
                if (IsInMap(positions.Count, nextPosition))
                {
                    float x = nextPosition.x * 10;
                    float z = nextPosition.y * 20;
                    var newPos = new Vector3(x, 0, z);
                    positions.Add(newPos);
                }

                index++;
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
        private bool IsInEllipseDissolve(Vector2 normalizedPos, Vector2 ellipseCenter, Vector2 ellipseRadius, float dissolveAmount, float mapRand)
        {
            float distance = Mathf.Sqrt(
                Mathf.Pow( (normalizedPos.x - ellipseCenter.x) / ellipseRadius.x, 2 ) +
                Mathf.Pow( (normalizedPos.y - ellipseCenter.y) / ellipseRadius.y, 2 )
            );
            float snoiseForDissolve = (1.0f + (float)SimplexNoise.Noise(
                (double)(normalizedPos.x * mapData.dissolveNoiseMapScaleAndOffset.x + mapData.dissolveNoiseMapScaleAndOffset.z),
                (double)(normalizedPos.y * mapData.dissolveNoiseMapScaleAndOffset.y + mapData.dissolveNoiseMapScaleAndOffset.w + mapRand)
            )) / 2.0f; // 0 ~ 1
            float dissolveFactor = 1 - distance + snoiseForDissolve * dissolveAmount; // 0.1はノイズの影響度合い
            return dissolveFactor > 0.0f;
        }


        private bool IsInMap(int tsuruIndex, Vector2 normalizedPos)
        {
            float mapRand = mapSeed.RandomValueRange(0f, 10f);
            float snoiseForMap = (1.0f + (float) SimplexNoise.Noise(
                (double) (normalizedPos.x * mapData.noiseMapScaleAndOffset.x + mapData.noiseMapScaleAndOffset.z),
                (double) (normalizedPos.y * mapData.noiseMapScaleAndOffset.y + mapData.noiseMapScaleAndOffset.w + mapRand)
            )) / 2.0f; // 0 ~ 1

            float progress = Mathf.Clamp(tsuruIndex, tsuruLandingData.phase1TsuruCount, _numTsurus) / (float)_numTsurus; //e.g. progress: 30体までは全て0, 30体からN体までの間は0.0 ~ 1.0
            Vector2 area1EllipseRadius = Vector2.Lerp(mapData.area1EllipseRadius * tsuruLandingData.phase1RadiusUsage, mapData.area1EllipseRadius, progress);
            Vector2 area2EllipseRadius = Vector2.Lerp(mapData.area2EllipseRadius * tsuruLandingData.phase1RadiusUsage, mapData.area2EllipseRadius, progress);

            bool isInArea1 = IsInEllipseDissolve(normalizedPos, mapData.area1EllipseCenter, area1EllipseRadius, mapData.dissolveAmount, mapRand);
            bool isInArea2 = IsInEllipseDissolve(normalizedPos, mapData.area2EllipseCenter, area2EllipseRadius, mapData.dissolveAmount, mapRand);

            bool isInMap = (isInArea1 || isInArea2) && snoiseForMap > mapData.noiseCutThreshold;

            return isInMap;
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
