using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public class SpecDto
    {
        public BudgetDirectItems BudgetDirectData { get; set; }
        public string Description { get; set; }
        public SpecDataImage Image { get; set; }
        public List<SpecItems> Items { get; set; }
        public PriceNew PriceNew { get; set; }
        public string PricePrivate { get; set; }
        public string PriceTradeIn { get; set; }
        public StrattonItems StrattonData { get; set; }
        public string Title { get; set; }
    }

    public class SpecItems
    {
        public SpecItems(string title, string value)
        {
            Title = title;
            Value = value;
        }

        public string Title { get; set; }
        public string Value { get; set; }
    }

    public class PriceNew
    {
        public string DisclaimerTitle { get; set; }
        public string DisclaimerText { get; set; }
        public string Price { get; set; }
    }

    public class BudgetDirectItems
    {
        public string AnnualCost { get; set; }
        public string Disclaimer { get; set; }
        public string FormUrl { get; set; }
        public string LogoUrl { get; set; }
        public string Postcode { get; set; }
        public string QuoteType { get; set; }
        public string TermCondition { get; set; }
        public Heading Headings { get; set; }

        public class Heading
        {
            public string Title { get; set; }
            public string FrequentPayment { get; set; }
            public string GetQuote { get; set; }
        }
    }

    public class StrattonItems
    {
        public string Disclaimer { get; set; }
        public string FormUrl { get; set; }
        public string InterestRate { get; set; }
        public string LogoUrl { get; set; }
        public string MonthlyRepayments { get; set; }
        public string PurchasePrice { get; set; }
        public string ResidualAmount { get; set; }
        public string ResidualPercentage { get; set; }
        public string TermInMonths { get; set; }
        public string WeeklyRepayments { get; set; }
        public Heading Headings { get; set; }

        public class Heading
        {
            public string Title { get; set; }
            public string FrequentPayment { get; set; }
            public string GetQuote { get; set; }
        }
    }

    public class SpecDataImage
    {
        public string Url { get; set; }
        public string AlternateText { get; set; }
    }
}