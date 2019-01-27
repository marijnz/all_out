using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class TenantEvaluator : MonoBehaviour
{
	private struct CheckedTenants
	{
		public Tenant first;
		public Tenant second;
		public int tick;
		public bool bothWereHappy;

		public CheckedTenants(Tenant first, Tenant second, int tick, bool bothWereHappy)
		{
			this.first = first;
			this.second = second;
			this.tick = tick;
			this.bothWereHappy = bothWereHappy;
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
				(second == other.first && first == other.second)) && tick == other.tick && bothWereHappy == other.bothWereHappy;
		}

		// override object.GetHashCode
		public override int GetHashCode()
		{
			return first.GetHashCode() * second.GetHashCode() * tick.GetHashCode() * bothWereHappy.GetHashCode();
		}
	}

	public List<Tenant> _allTenants = new List<Tenant>();
	private List<CheckedTenants> _checkedTenants = new List<CheckedTenants>();
	[SerializeField] private float _evaluationDistance = 1.0f;
	[SerializeField] private int _ticksToSleepForACouple = 500;
	[SerializeField] private int _maximumNumberOfChecks = 100;

	void Start()
	{
		FindObjectOfType<SimulateTickRate>().OnTickUpdate.AddListener(OnTick);
	}

	void OnTick(int tick)
	{
		int totallyHappinessCount = 0;
		for (int i = 0; i < _allTenants.Count; i++)
		{
			Tenant first = _allTenants[i];
			for (int j = i + 1; j < _allTenants.Count; j++)
			{
				Tenant second = _allTenants[j];

				if (Vector3.Distance(first.transform.position, second.transform.position) <= _evaluationDistance)
				{
					if (_checkedTenants.Any(couple => couple.tick > tick - _ticksToSleepForACouple && ((couple.first == first && couple.second == second) || (couple.second == first && couple.first == second)))) continue;

					first.transform.DOPunchScale(Vector3.one * 0.15f, .4f, 6, 1f);
					second.transform.DOPunchScale(Vector3.one * 0.15f, .4f, 6, 1f);
					first.StopMoving();
					second.StopMoving();
					var firstIsHappy = IsHappyWith(first.generatedTenant, second.generatedTenant);

					Emotion.Show(first.transform, firstIsHappy);

					var secondIsHappy = IsHappyWith(second.generatedTenant, first.generatedTenant);

					Emotion.Show(second.transform, secondIsHappy);

					first.happiness += firstIsHappy ? 1 : -1;
					second.happiness += secondIsHappy ? 1 : -1;

					if (firstIsHappy && secondIsHappy)
					{
						AudioPlayer.Play("meetLikeLike");
					}
					else if (!firstIsHappy && !secondIsHappy)
					{
						AudioPlayer.Play("meetHateHate");
					}
					else
					{
						AudioPlayer.Play("meetLikeHate");
					}

					List<Tenant> unhappyTenants = new List<Tenant>();
					if (first.happiness <= 0)
					{
						first.lastFrustration = second;
						unhappyTenants.Add(first);
					}
					if (second.happiness <= 0)
					{
						second.lastFrustration = first;
						unhappyTenants.Add(second);
					}

					if (unhappyTenants.Count > 0)
					{
						FindObjectOfType<Setup>().RequestNewTenants(unhappyTenants);
						totallyHappinessCount = int.MinValue;
					}

					_checkedTenants.Add(new CheckedTenants(first, second, tick, firstIsHappy && secondIsHappy));
				}

				if (_checkedTenants.Where(couple => (couple.first == first && couple.second == second) || (couple.second == first && couple.first == second)).Any(couple => couple.bothWereHappy))
				{
					totallyHappinessCount++;
				}
			}
		}

		if ((_allTenants.Count >= 3 && totallyHappinessCount >= _allTenants.Count * (_allTenants.Count - 1) * 0.5f) ||
			_checkedTenants.Count >= _maximumNumberOfChecks)
		{
			Debug.LogFormat("Happiness: {0} // Tenants: {1}", totallyHappinessCount, _allTenants.Count);
			// TODO: for Marijn, add visually feedback
		}
	}

	public static bool IsHappyWith(GeneratedTenant a, GeneratedTenant b)
	{
		return !a.traits.Any(traitOfFirst =>
			b.traits.Any(traitOfSecond =>
				traitOfFirst.dislikes.Contains(traitOfSecond.id)));
	}

	public void Add(Tenant tenant)
	{
		_allTenants.Add(tenant);
	}

	public void Remove(Tenant tenant)
	{
		if (_allTenants.Contains(tenant))
			_allTenants.Remove(tenant);

		_checkedTenants = _checkedTenants.Except(_checkedTenants.Where(couple => couple.first == tenant || couple.second == tenant)).ToList();
	}
}