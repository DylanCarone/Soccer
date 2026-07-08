using UnityEngine;

public static class PassTargetFinder
{
    
    
    public static Player FindClosestPlayer(Vector2 origin, Vector2 direction, float maxLineDeviation,
        float maxSearchRadius, Player passer)
    {

        Collider2D[] candidates = Physics2D.OverlapCircleAll(origin, maxSearchRadius, LayerMask.GetMask("Player"));

        Player best = null;
        float bestDeviation = float.MaxValue;

        foreach (var col in candidates)
        {
            if (!col.TryGetComponent(out Player candidate)) continue;
            if(candidate == passer) continue;
            
            Vector2 toCandidate = (Vector2)candidate.transform.position - origin;
            float projection = Vector2.Dot(toCandidate, direction);
            if (projection <= 0) continue;
            
            Vector2 closestPointOnLine = origin + direction * projection;
            float deviation = Vector2.Distance(candidate.transform.position, closestPointOnLine);

            if (deviation > maxLineDeviation) continue;
            if (deviation < bestDeviation)
            {
                bestDeviation = deviation;
                best = candidate;
            }
        }

        return best;

    }
}
