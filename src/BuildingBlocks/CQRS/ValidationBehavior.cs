using FluentValidation;
using MediatR;

namespace LogisticsPlatform.BuildingBlocks.CQRS;

// Bu sýnýf, MediatR üzerinden geçen HER Command/Query için araya girer
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            // Ýstek için yazýlmýþ bir kural yoksa direkt bir sonraki adýma (Handler'a) geç
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        // Tüm doðrulama kurallarýný çalýþtýr
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        
        // Baþarýsýz olanlarý (hatalarý) filtrele
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Count != 0)
        {
            // Hata varsa özel bir ValidationException fýrlat, süreci kes!
            // Ýleride bu exception'ý Global Error Handler'da yakalayýp 400 Bad Request döneceðiz.
            throw new ValidationException(failures);
        }

        // Hata yoksa iþlemi sürdür
        return await next();
    }
}
