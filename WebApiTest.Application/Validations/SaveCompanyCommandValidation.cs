using FluentValidation;
using WebApiTest.Application.Commands;

namespace WebApiTest.Application.Validations
{
    public class SaveCompanyCommandValidation : AbstractValidator<SaveCompanyCommand>
    {
        public SaveCompanyCommandValidation()
        {
            RuleFor(m => m.Code)
                .NotEmpty().WithMessage("公司编号不能为空");

            RuleFor(m => m.Name)
                .Must(m => !string.IsNullOrWhiteSpace(m)).WithMessage("公司名称不能为空");

            RuleFor(m => m.Name)
                .Must(UserDefined).WithMessage("测试验证自定义方法");
        }

        private bool UserDefined(SaveCompanyCommand model, string name)
        {
            return model.Name == name;
        }
    }
}
