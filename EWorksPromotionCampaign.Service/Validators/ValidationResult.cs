using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Service.Validators
{
    public class ValidationResult
    {
        public bool IsValid => !Errors.Any();
        public ErrorsList Errors { get; set; } = new ErrorsList();
    }
}
