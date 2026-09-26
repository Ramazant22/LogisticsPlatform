using FluentValidation;

namespace LogisticsPlatform.Modules.Shipment.Application;

public class AssignShipmentToVehicleCommandValidator : AbstractValidator<AssignShipmentToVehicleCommand>
{
    public AssignShipmentToVehicleCommandValidator()
    {
        RuleFor(x => x.ShipmentId).NotEmpty().WithMessage("Kargo ID boþ olamaz.");
        RuleFor(x => x.VehicleId).NotEmpty().WithMessage("Araç ID boþ olamaz.");
    }
}
