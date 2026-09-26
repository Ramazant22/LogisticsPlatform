using System;
namespace LogisticsPlatform.Modules.Routes.Domain;
public class Route {
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public decimal DistanceKm { get; set; }
    public TimeSpan EstimatedDuration { get; set; }

    public Route() {}
    public Route(string name, string origin, string destination, decimal distanceKm, TimeSpan estimatedDuration) {
        Id = Guid.NewGuid();
        Name = name;
        Origin = origin;
        Destination = destination;
        DistanceKm = distanceKm;
        EstimatedDuration = estimatedDuration;
    }
}
