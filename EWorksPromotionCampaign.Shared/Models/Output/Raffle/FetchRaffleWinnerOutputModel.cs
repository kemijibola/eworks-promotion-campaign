using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Output.Raffle
{
    public class FetchRaffleWinnerOutputModel
    {
        private FetchRaffleWinnerOutputModel(IReadOnlyCollection<RaffleSubscriberViewModel> Subscribers)
        {
            EligibleWinners = Subscribers?.Select(x => new WinnerData { FullName = x.FullName, RewardCode = x.PlainCode, PhoneNumber = HelperModel.MaskPhone(x.Phone, 6, 10, '*') });
        }
        public IEnumerable<WinnerData> EligibleWinners { get; set; }

        public static FetchRaffleWinnerOutputModel FromRaffleWinners(IReadOnlyCollection<RaffleSubscriberViewModel> Subscribers)
        {
            return new FetchRaffleWinnerOutputModel(Subscribers);
        }
    }
}
