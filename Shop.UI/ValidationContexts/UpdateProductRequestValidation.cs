using FluentValidation;
using Shop.Application.Cart;
using Shop.Application.ProductsAdmin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.UI.ValidationContexts
{
  
    public class UpdateProductRequestValidation :
        AbstractValidator<UpdateProduct.Request>
    {
        public UpdateProductRequestValidation()
        {
            RuleFor(x => x.Name).NotNull();
            RuleFor(x => x.Description)
                .NotNull()
                .MaximumLength(130);


            RuleFor(x => x.Value)
                .NotNull()
                .Matches(@"\d+([,\.]\d+)?")
                .MaximumLength(30)
                .WithMessage("This price fromat is not supported.");
            
        }
    }
}
