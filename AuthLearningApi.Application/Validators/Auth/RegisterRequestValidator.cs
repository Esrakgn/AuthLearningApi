using System;
using System.Collections.Generic;
using System.Text;
using AuthLearningApi.Application.DTOs.Auth;
using FluentValidation;

namespace AuthLearningApi.Application.Validators.Auth
{
    //FluentValidation kütüphanesinden gelen AbstractValidator<T> sınıfından kalıtım alıyor
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.FullName)
                .NotEmpty().WithMessage("Full name is required");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Please enter a valid email address");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(5).WithMessage("Password must be at least 5 characters long.");
        }

    }
   
}
//FluentValidation’ın hazır fonksiyonuyla email formata uyup uymadığını kontrol ediyoruz 
