using FluentValidation;

namespace LogisticsPlatform.Modules.Fleet.Application;

public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(x => x.LicensePlate)
            .NotEmpty().WithMessage("Plaka alaný boþ geçilemez.")
            .Length(5, 20).WithMessage("Plaka 5 ile 20 karakter arasýnda olmalýdýr.");

        RuleFor(x => x.Brand)
            .NotEmpty().WithMessage("Marka boþ geçilemez.");

        RuleFor(x => x.ModelYear)
            .GreaterThan(1990).WithMessage("Araç modeli 1990'dan eski olamaz.")
            .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage("Geçersiz model yýlý.");

        RuleFor(x => x.CapacityInKg)
            .GreaterThan(0).WithMessage("Kapasite sýfýrdan büyük olmalýdýr.");
    }
}
