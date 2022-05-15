using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EWorksPromotionCampaign.Shared.Models.Output.Raffle
{
    public class FetchRaffleRewardWinnerOutputModel
    {
        private FetchRaffleRewardWinnerOutputModel(IReadOnlyCollection<RaffleSubscriberViewModel> Subscribers)
        {
            Winners = Subscribers;
        }
        public IReadOnlyCollection<RaffleSubscriberViewModel> Winners { get; set; }
        public static FetchRaffleRewardWinnerOutputModel FromRaffleWinners(IReadOnlyCollection<RaffleSubscriberViewModel> Subscribers)
        {
            return new FetchRaffleRewardWinnerOutputModel(Subscribers);
        }
    }
}
