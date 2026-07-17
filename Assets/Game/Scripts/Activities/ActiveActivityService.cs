using System;
using IdleGame.Core.Bootstrap;
using IdleGame.Save;
using UnityEngine;

namespace IdleGame.Activities
{
    public sealed class ActiveActivityService : MonoBehaviour, IGameService
    {
        public event Action ActiveActivityChanged;

        public int InitializationOrder => 40;
        public string ActiveActivityId { get; private set; } = string.Empty;
        public string ActiveActivityName { get; private set; } = "No Active Activity";
        public string ActiveTargetName { get; private set; } = string.Empty;
        public bool HasActiveActivity => !string.IsNullOrWhiteSpace(ActiveActivityId);

        public void InitializeService()
        {
        }

        public bool RequestStartPrimary(string activityId, string activityName, string targetName)
        {
            if (!CanStartPrimary(activityId))
            {
                return false;
            }

            ActiveActivityId = activityId;
            ActiveActivityName = activityName;
            ActiveTargetName = targetName;
            ActiveActivityChanged?.Invoke();
            SaveManager.Instance?.SaveNow();
            return true;
        }

        public bool CanStartPrimary(string activityId)
        {
            return string.IsNullOrWhiteSpace(ActiveActivityId) ||
                   string.Equals(ActiveActivityId, activityId, StringComparison.Ordinal);
        }

        public void Stop(string activityId)
        {
            if (!string.Equals(ActiveActivityId, activityId, StringComparison.Ordinal))
            {
                return;
            }

            Clear();
        }

        public void Clear()
        {
            ActiveActivityId = string.Empty;
            ActiveActivityName = "No Active Activity";
            ActiveTargetName = string.Empty;
            ActiveActivityChanged?.Invoke();
            SaveManager.Instance?.SaveNow();
        }

        public void UpdateTarget(string targetName)
        {
            ActiveTargetName = targetName;
            ActiveActivityChanged?.Invoke();
        }
    }
}
