using System.Linq;
using UnityEngine;

public class TenantGenerator : MonoBehaviour
{
	public TenantGeneration tenantGeneration;

	public GeneratedTenant Generate(TenantData.TenantItem tenantData)
	{
		var tenant = new GeneratedTenant();
		for (var i = 0; i < 2; i++)
		{
			TenantTrait randomTrait = null;
			do
			{
				randomTrait = tenantGeneration.traits.Random();
				// Continue if we have the trait already, or the trait that we have has the selected random one as a dislike
				// (avoid weird combos)
			} while (tenant.traits.Contains(randomTrait) || tenant.traits.Any(t => t.dislikes.Contains(randomTrait.id)));

			tenant.traits.Add(randomTrait);
		}
		tenant.data = tenantData;
		return tenant;
	}
}