using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeatherAndTime
{
	public class TimeManager : MonoBehaviour
	{
		#region Class
		[System.Serializable]
		class Timer
		{
			private readonly uint index;
			private event System.Action action;
			private float timerClock;
			private readonly float timer;
			public bool Destroy = false;
			private readonly bool destroySelf;
			private readonly object objectReference;

			public Timer(uint index, System.Action action, float timer, object objectReference, bool destroySelf = false)
			{
				this.index = index;
				this.action = action;
				this.timer = timer;
				this.timerClock = 0f;
				this.destroySelf = destroySelf;
				this.objectReference = objectReference;
			}

			public void Update(float deltaTime)
			{
				if(objectReference.Equals(null))
				{
					this.Destroy = true;
					return;
				}

				timerClock += deltaTime;
				if (this.timerClock >= this.timer)
				{
					timerClock = 0f;
					action?.Invoke();
					if (destroySelf)
					{
						this.Destroy = true;
					}
				}
			}
		}
		#endregion

		#region Fields
		private static TimeManager instance;
		private static TimeManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new GameObject("@TimeManager").AddComponent<TimeManager>();
				}
				return instance;
			}
		}

		public static GameObject GameObject
		{
			get => Instance.gameObject;
		}

		[SerializeField]
		List<AudioSource> sons;

		private uint timersIndex = 0;
		[SerializeField] private Dictionary<uint, Timer> timers = new Dictionary<uint, Timer>();
		#endregion

		#region Methods
		private void Awake()
		{
			if (instance == null || instance == this)
				instance = this;
			else
				Destroy(gameObject);

			CreateNewTimer(Audio, Random.Range(3f, 10f), instance, true);
		}

		private void LateUpdate()
		{
			UpdateTimers();
		}

		private void UpdateTimers()
		{
			float frameDeltaTime = Time.deltaTime;
			List<uint> toRemove = new List<uint>();

			foreach(KeyValuePair<uint, Timer> pairTimer in timers)
			{
				pairTimer.Value.Update(frameDeltaTime);
				if (pairTimer.Value.Destroy)
					toRemove.Add(pairTimer.Key);
			}

			foreach(uint index in toRemove)
			{
				timers.Remove(index);
			}
		}

		void Audio()
        {
			sons[Random.Range(0, sons.Count)].Play();
			CreateNewTimer(Audio, Random.Range(2f, 5f), instance, true);
        }

		public static uint CreateNewTimer(System.Action action, float timer, object objectReference, bool selfDestruct = false)
		{
			uint count = unchecked(Instance.timersIndex++);
			Instance.timers.Add(count, new Timer(count, action, timer, objectReference, selfDestruct));
			return count;
		}

		public static bool DestroyTimer(uint index) 
			=> Instance.timers.Remove(index);
		#endregion
	}
}