using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VirtueSky.Core;
using VirtueSky.ObjectPooling;

namespace VirtueSky.Misc
{
    public class GenerateCoin : BaseMono
    {
        [SerializeField] private GameObject overlay;
        [SerializeField] private GameObject coinPrefab;
        [SerializeField] private Vector3? from;
        [SerializeField] private GameObject to;
        [SerializeField] private int numberCoin;
        [SerializeField] private int delay;
        [SerializeField] private float durationNear;
        [SerializeField] private float durationTarget;
        [SerializeField] private Ease easeNear;
        [SerializeField] private Ease easeTarget;
        [SerializeField] private float scale = 1;
        [SerializeField] private Pools pools;
        private System.Action moveOneCoinDone;
        private bool isScaleIconTo = false;

        private List<GameObject> coinsActive = new List<GameObject>();

        public void SetFrom(Vector3 from)
        {
            this.from = from;
        }

        public void SetToGameObject(GameObject to)
        {
            this.to = to;
        }

        private void Start()
        {
            overlay.SetActive(false);
            pools.Initialize();
        }

        public async void Generate(System.Action moveOneCoinDone, System.Action moveAllCoinDone, GameObject to = null,
            int numberCoin = -1)
        {
            isScaleIconTo = false;
            this.moveOneCoinDone = moveOneCoinDone;
            //this.moveAllCoinDone = moveAllCoinDone;
            this.to = to == null ? this.to : to;
            this.numberCoin = numberCoin < 0 ? this.numberCoin : numberCoin;
            overlay.SetActive(true);
            for (int i = 0; i < this.numberCoin; i++)
            {
                await Task.Delay(Random.Range(0, delay));
                GameObject coin = pools.Spawn(coinPrefab, transform);
                coin.transform.localScale = Vector3.one * scale;
                coinsActive.Add(coin);
                if (from != null)
                {
                    coin.transform.position = from.Value;
                }
                else
                {
                    coin.transform.localPosition = Vector3.zero;
                }

                MoveCoin(coin, moveAllCoinDone);
                // if (i == numberCoin - 1)
                // {
                //     Observer.CoinMove?.Invoke();
                // }
            }
        }

        private void MoveCoin(GameObject coin, System.Action moveAllCoinDone)
        {
            MoveToNear(coin).OnComplete(() =>
            {
                MoveToTarget(coin).OnComplete(() =>
                {
                    coinsActive.Remove(coin);
                    pools.Despawn(coin);
                    if (!isScaleIconTo)
                    {
                        isScaleIconTo = true;
                        ScaleIconTo();
                    }

                    moveOneCoinDone?.Invoke();
                    if (coinsActive.Count == 0)
                    {
                        moveAllCoinDone?.Invoke();
                        overlay.SetActive(false);
                        from = null;
                    }
                });
            });
        }

        private DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> MoveTo(
            Vector3 endValue, GameObject coin, float duration, Ease ease)
        {
            return coin.transform.DOMove(endValue, duration).SetEase(ease);
        }

        private DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> MoveToNear(
            GameObject coin)
        {
            return MoveTo(coin.transform.position + (Vector3)Random.insideUnitCircle * 1.3f, coin, durationNear,
                easeNear);
        }

        private DG.Tweening.Core.TweenerCore<Vector3, Vector3, DG.Tweening.Plugins.Options.VectorOptions> MoveToTarget(
            GameObject coin)
        {
            return MoveTo(to.transform.position, coin, durationTarget, easeTarget);
        }

        public void SetNumberCoin(int _numberCoin)
        {
            numberCoin = _numberCoin;
        }

        private void ScaleIconTo()
        {
            Vector3 currentScale = Vector3.one;
            Vector3 nextScale = currentScale + new Vector3(.1f, .1f, .1f);
            to.transform.DOScale(nextScale, durationTarget).SetEase(Ease.OutBack)
                .OnComplete((() => { to.transform.DOScale(currentScale, durationTarget / 2).SetEase(Ease.InBack); }));
        }
    }
}