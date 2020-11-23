using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Helpers
{
    public class EnumTypes
    {
        public enum Role { User, Admin };

        public enum AuctionStatus { Active, Completed, Pending, Stopped }

        public enum CoinsMaturityStatus { Pending, Matured}

        public enum CoinsMaturityType { HalfMaturation, FullMaturation }

        public enum BidRequestStatus { Approved, Rejected, Cancelled, Pending, InProgress }

        public static string GetCoinsMaturityTypeString(string type)
        {
            if (type == CoinsMaturityType.FullMaturation.ToString())
                return "5 days (100%)";

            else if(type == CoinsMaturityType.HalfMaturation.ToString())
                return "3 days (50%)";

            return "n/a";
        }

        public static string GetBackgroundColorStatus(string status)
        {
            string green = "bg-success text-white";
            string yellow = "bg-warning";
            string blue = "bg-primary text-white";
            string red = "bg-danger text-white";
            string gray = "bg-secondary text-white";

            bool Greens = status == BidRequestStatus.Approved.ToString() || status == AuctionStatus.Active.ToString() || status == CoinsMaturityStatus.Matured.ToString();
            bool Yellows = status == BidRequestStatus.InProgress.ToString() || status == AuctionStatus.Pending.ToString() || status == CoinsMaturityStatus.Pending.ToString();
            bool Blues = status == AuctionStatus.Completed.ToString();
            bool Reds = status == BidRequestStatus.Rejected.ToString() || status == AuctionStatus.Stopped.ToString();
            bool Grays = status == BidRequestStatus.Cancelled.ToString();

            if (Greens) return green;
            if (Yellows) return yellow;
            if (Blues) return blue;
            if (Reds) return red;
            if (Grays) return gray;

            return "";
        }

        public static string GetCancelledBidCoinsColor(string status)
        {
            string blue = "text-primary";
            string red = "text-danger";
            string green = "text-success";

            if (status == BidRequestStatus.InProgress.ToString()) 
                return blue;

            if (status == BidRequestStatus.Cancelled.ToString())
                return red;

            return "";
        }
    }
}
