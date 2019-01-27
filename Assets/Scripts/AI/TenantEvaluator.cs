using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TenantEvaluator : MonoBehaviour
{
	private struct CheckedTenants
	{
		public Tenant first;
		public Tenant second;
		public int tick;

		public CheckedTenants(Tenant first, Tenant second, int tick)
		{
			this.first = first;
			this.second = second;
			this.tick = tick;
		}

		// override object.Equals
		public override bool Equals(object obj)
		{
			//
			// See the full list of guidelines at
			//   http://go.microsoft.com/fwlink/?LinkID=85237
			// and also the guidance for operator== at
			//   http://go.microsoft.com/fwlink/?LinkId=85238
			//

			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			CheckedTenants other = (CheckedTenants) obj;

			return ((first == other.first && second == other.second) ||
				(second == other.first && first == other.second)) && tick == other.tick;
		}

		// override object.GetHashCode
		public override int GetHashCode()
		{
			return first.GetHashCode() * second.GetHashCode() * tick;
		}
	}
	private List<Tenant> _allTenants = new List<Tenant>();
	private List<CheckedTenants> _checkedTenants = new List<CheckedTenants>();
	[SerializeField] private float _evaluationDistance = 1.0f;
	[SerializeField] private int _ticksToSleepForACouple = 500;

	void Start()
	{
		FindObjectOfType<SimulateTickRate>().OnTickUpdate.AddListener(OnTick);
	}

	void OnTick(int tick)
	{
		for (int i = 0; i < _allTenants.Count; i++)
		{
			Tenant first = _allTenants[i];
			for (int j = i + 1; j < _allTenants.Count; j++)
			{
				Tenant second = _allTenants[j];

				if (Vector3.Distance(first.transform.position, second.transform.position) <= _evaluationDistance)
				{
					if (_checkedTenants.Any(couple => couple.tick > tick - _ticksToSleepForACouple && ((couple.first == first && couple.second == second) || (couple.second == first && couple.first == second)))) continue;

					first.StopMoving();
					second.StopMoving();
					var firstIsHappy = !first.Traits.Any(traitOfFirst =>
										second.Traits.Any(traitOfSecond =>
										traitOfFirst.dislikes.Contains(traitOfSecond.id)));

					Emotion.Show(first.transform, firstIsHappy);

					var secondIsHappy = !first.Traits.Any(traitOfFirst =>
										second.Traits.Any(traitOfSecond =>
										traitOfSecond.dislikes.Contains(traitOfFirst.id)));

					Emotion.Show(second.transform, secondIsHappy);

					first.happiness += firstIsHappy ? 1 : -1;
					second.happiness += secondIsHappy ? 1 : -1;

					List<Tenant> unhappyTenants = new List<Tenant>();
					if(first.happiness <= 0)
					{
						first.lastFrustration = second;
						unhappyTenants.Add(first);
					}
					if(second.happiness <= 0)
					{
						second.lastFrustration = first;
						unhappyTenants.Add(second);
					}

					if(unhappyTenants.Count > 0)
					{
						FindObjectOfType<Setup>().RequestNewTenants(unhappyTenants);
					}

					_checkedTenants.Add(new CheckedTenants(first, second, tick));
				}
			}
		}
	}

	public void Add(Tenant tenant)
	{
		_allTenants.Add(tenant);
	}

	public void Remove(Tenant tenant)
	{
		if (_allTenants.Contains(tenant))
			_allTenants.Remove(tenant);
	}
}