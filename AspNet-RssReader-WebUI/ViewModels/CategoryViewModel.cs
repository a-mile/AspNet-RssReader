using System.Linq;
using System.Web;
using AspNet_RssReader_WebUI.Extensions;
using FluentValidation;
using FluentValidation.Attributes;

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
                                .Must(UniqueName).WithMessage("There is already category with this name");
        }

        private bool UniqueName(CategoryViewModel model, string name)
        {
            var currentUser = HttpContext.Current.GetCurrentUser();

            if (model is UpdateCategoryViewModel)
            {
                return !currentUser.Sources.Any(x => (x.Name == name) && (x.Id != (model as UpdateCategoryViewModel).Id));
            }

            return currentUser.Categories.All(x => x.Name != name);
        }
    }
}