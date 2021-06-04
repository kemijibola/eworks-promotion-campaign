using EWorksPromotionCampaign.Service.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Data
{
    public class AddResult<T>
    {
        public AddResult(ValidationResult validationResult, bool isDuplicate, T data)
        {
            ValidationResult = validationResult;
            IsDuplicate = isDuplicate;
            Data = data;
        }
        public bool IsSuccess => IsValid && !IsDuplicate;
        public ValidationResult ValidationResult { get; }
        public bool IsValid => ValidationResult.IsValid;
        public bool IsDuplicate { get; }
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
