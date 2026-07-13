using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace IdleGame.Core.Bootstrap
{
    [DefaultExecutionOrder(-1000)]
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private bool initializeOnAwake = true;
        [SerializeField] private List<MonoBehaviour> services = new();

        public bool IsInitialized { get; private set; }
        public IReadOnlyList<MonoBehaviour> Services => services;

        private void Awake()
        {
            if (initializeOnAwake)
            {
                Initialize();
            }
        }

        public void Initialize()
        {
            if (IsInitialized)
            {
                return;
            }

            var orderedServices = new List<IGameService>();

            foreach (var serviceBehaviour in services.Where(service => service != null))
            {
                if (serviceBehaviour is not IGameService service)
                {
                    Debug.LogError(
                        $"{serviceBehaviour.name} is registered with {nameof(GameBootstrap)} but does not implement {nameof(IGameService)}.",
                        serviceBehaviour);
                    continue;
                }

                orderedServices.Add(service);
            }

            foreach (var service in orderedServices.OrderBy(service => service.InitializationOrder).ThenBy(service => service.GetType().Name))
            {
                try
                {
                    service.InitializeService();
                }
                catch (Exception exception)
                {
                    Debug.LogException(exception, service as UnityEngine.Object);
                    throw;
                }
            }

            IsInitialized = true;
        }

        public void ConfigureForEditor(IEnumerable<MonoBehaviour> orderedServices, bool shouldInitializeOnAwake = true)
        {
            services = orderedServices?.Where(service => service != null).ToList() ?? new List<MonoBehaviour>();
            initializeOnAwake = shouldInitializeOnAwake;
        }
    }
}
