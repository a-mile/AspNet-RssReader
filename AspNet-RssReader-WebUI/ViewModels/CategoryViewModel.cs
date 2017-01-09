using System.Linq;
using System.Web;
using AspNet_RssReader_Domain.Concrete;
using AspNet_RssReader_WebUI.Extensions;
using FluentValidation;
using FluentValidation.Attributes;
using Microsoft.AspNet.Identity;

namespace AspNet_RssReader_WebUI.ViewModels
{   
    public class CategoryViewModel
    {       
        public string Name { get; set; }
    }

    [Validator(typeof(CategoryViewModelValidator))]
    public class CreateCategoryViewModel : CategoryViewModel { }

    [Validator(typeof(CategoryViewModelValidator))]
    public class UpdateCategoryViewModel : CategoryViewModel
    {
        public int Id { get; set; }
    }

    public class DeleteCategoryViewModel : CategoryViewModel
    {
        public int Id { get; set; }
    }

    public class CategoryViewModelValidator : AbstractValidator<CategoryViewModel>
    {
        public CategoryViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category name cannot be empty")
                                .Length(0, 15).WithMessage("Maximum length of category name is 15")
                                .Must(Unique).WithMessage("There is already category with this name");
        }

        private bool Unique(string name)
        {
            var currentUser = HttpContext.Current.GetCurrentUser();

            return currentUser.Categories.All(x => x.Name != name);
        }
    }
}