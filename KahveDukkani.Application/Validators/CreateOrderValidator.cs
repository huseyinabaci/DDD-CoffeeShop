using FluentValidation;
using KahveDukkani.Application.DTOs;

namespace KahveDukkani.Application.Validators;

public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
{
    public CreateOrderValidator()
    {
        // Şehir kuralı
        RuleFor(x => x.City)
            .NotEmpty().WithMessage("Şehir bilgisi boş olamaz.")
            .MaximumLength(50).WithMessage("Şehir adı 50 karakterden uzun olamaz.");

        // Sokak kuralı
        RuleFor(x => x.Street)
            .NotEmpty().WithMessage("Sokak/Adres bilgisi şart.");

        // Müşteri adı kuralı
        RuleFor(x => x.CustomerName)
            .NotEmpty().WithMessage("Müşteri adı girmelisiniz.");

        // Sipariş listesi kuralı
        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("Sepet boş olamaz en az 1 ürün seçin.");

        // Listenin içindeki her bir ürün için kural
        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(x => x.ProductName).NotEmpty().WithMessage("Ürün adı boş olamaz.");
            items.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Adet 0'dan büyük olmalı.");
        });
    }
}