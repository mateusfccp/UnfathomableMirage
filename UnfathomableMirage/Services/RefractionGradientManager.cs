using System;
using System.Collections.Generic;

namespace UnfathomableMirage.Services;

/// <summary>
/// Manages the refraction gradient for the skybox.
/// </summary>
public class RefractionGradientManager
{
    private double _minimumRefractionIndex;
    private double _maximumRefractionIndex;

    /// <summary>
    /// Gets or sets the minimum refraction index for the gradient.
    /// <br />
    /// Changing this value will recompute the layers.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is less than 1.0 or greater than or equal to the maximum refraction index.</exception>
    public double MinimumRefractionIndex
    {
        get => _minimumRefractionIndex;
        set
        {
            if (value < 1.0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Minimum refraction index must be at least 1.0.");
            }

            if (value >= MaximumRefractionIndex)
            {
                throw new ArgumentOutOfRangeException(nameof(value),
                    "Minimum refraction index must be less than the maximum refraction index.");
            }

            _minimumRefractionIndex = value;
            RecomputeLayers();
        }
    }

    /// <summary>
    /// Gets or sets the maximum refraction index for the gradient.
    /// <br />
    /// Changing this value will recompute the layers.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the value is less than or equal to the minimum refraction index.</exception>
    public double MaximumRefractionIndex
    {
        get => _maximumRefractionIndex;
        set
        {
            if (value <= MinimumRefractionIndex)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Maximum refraction index must be greater than the minimum refraction index."
                );
            }

            _maximumRefractionIndex = value;
            RecomputeLayers();
        }
    }

    private readonly List<double> _layers = [];

    /// <summary>
    /// Creates a new refraction gradient manager.
    /// </summary>
    /// <param name="minimumRefractionIndex">The minimum refraction index for the gradient.</param>
    /// <param name="maximumRefractionIndex">The maximum refraction index for the gradient.</param>
    /// <param name="layerCount">The number of layers in the gradient.</param>
    public RefractionGradientManager(
        double minimumRefractionIndex = 1.0,
        double maximumRefractionIndex = 1.5,
        int layerCount = 10
    )
    {
        _minimumRefractionIndex = minimumRefractionIndex;
        _maximumRefractionIndex = maximumRefractionIndex;
        RecomputeLayers(layerCount);
    }

    /// <summary>
    /// Gets or sets the number of layers in the refraction gradient.
    /// <br />
    /// Changing this value will recompute the layers.
    /// </summary>
    public int LayerCount
    {
        get => _layers.Count;
        set => RecomputeLayers(value);
    }
    
    /// <summary>
    /// Gets the refraction layers in the gradient.
    /// </summary>
    public IReadOnlyCollection<double> Layers => _layers.AsReadOnly();

    private void RecomputeLayers(int? layerCount = null)
    {
        layerCount ??= _layers.Count;

        _layers.Clear();
        var refractionIndexDelta = MaximumRefractionIndex - MinimumRefractionIndex;
        var step = refractionIndexDelta / layerCount.Value;

        for (var i = 0; i < layerCount.Value; i++)
        {
            var refractionIndex = MinimumRefractionIndex + i * step;
            _layers.Add(refractionIndex);
        }
    }
}
