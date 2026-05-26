using System;
using System.Numerics;

namespace UnfathomableMirage.Utilities
{
    /// <summary>
    /// Provides static optic and physics calculations for the simulation.
    /// </summary>
    public static class OpticsPhysics
    {
        /// <summary>
        /// Calculates the angle of refraction using Snell's Law.
        /// </summary>
        /// <param name="n1">Refractive index of the incident medium.</param>
        /// <param name="n2">Refractive index of the refractive medium.</param>
        /// <param name="incidentAngle">Angle of incidence in radians.</param>
        /// <returns>The angle of refraction in radians, or double.NaN if Total Internal Reflection occurs.</returns>
        public static double CalculateRefractionAngle(double n1, double n2, double incidentAngle)
        {
            double refractionRatio = n1 / n2;
            double sinTheta2 = refractionRatio * Math.Sin(incidentAngle);

            // Check for Total Internal Reflection (TIR)
            if (sinTheta2 > 1.0 || sinTheta2 < -1.0)
            {
                return double.NaN;
            }

            return Math.Asin(sinTheta2);
        }

        /// <summary>
        /// Calculates the refracted direction vector using Snell's Law, optimized with Vector2.
        /// </summary>
        /// <param name="incidentDirection">Normalized directional vector of the incoming light ray.</param>
        /// <param name="surfaceNormal">Normalized normal vector of the surface boundary.</param>
        /// <param name="n1">Refractive index of the incident medium.</param>
        /// <param name="n2">Refractive index of the refractive medium.</param>
        /// <returns>The normalized refracted vector, or Vector2.Zero if Total Internal Reflection occurs.</returns>
        public static Vector2 Refract(Vector2 incidentDirection, Vector2 surfaceNormal, float n1, float n2)
        {
            float ratio = n1 / n2;
            float cosThetaI = -Vector2.Dot(incidentDirection, surfaceNormal);
            float sin2ThetaT = ratio * ratio * (1.0f - cosThetaI * cosThetaI);

            // Check for Total Internal Reflection (TIR)
            if (sin2ThetaT > 1.0f)
            {
                return Vector2.Zero; // Represents absolute reflection
            }

            float cosThetaT = (float)Math.Sqrt(1.0f - sin2ThetaT);

            // Compute and return the new directional vector
            return incidentDirection * ratio + surfaceNormal * (ratio * cosThetaI - cosThetaT);
        }
    }
}
