using EWorksPromotionCampaign.Service.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Data
{
    public class Result<T>
    {
        public Result(ValidationResult validationResult, T data)
        {
            ValidationResult = validationResult;
            Data = data;
        }
        public ValidationResult ValidationResult { get; }
        public bool IsSuccess => IsValid;
        public bool IsValid => ValidationResult.IsValid;
        public T Data { get; }
        public string Description => StringifiedErrors();
        private string StringifiedErrors()
        {
            if (!IsValid)
                return string.Join("|", ValidationResult.Errors.Select(x => x.Value));
            return "";
        }
    }
}
