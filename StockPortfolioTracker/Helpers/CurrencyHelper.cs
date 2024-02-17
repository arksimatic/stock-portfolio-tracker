using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;
using StockPortfolioTracker.Models;

namespace StockPortfolioTracker.Helpers
{
    public class CurrencyHelper
    {
        // this method takes array of currencies and should return array of curriencies that also have currencies of CurrencyCode that weren't in the input array
        public static Currency[] FillCurrencyArrayWithMissingCurrencies(Currency[] currencies)
        {
            currencies = currencies ?? new Currency[0];
            CurrencyCode[] currencyCodes = Enum.GetValues(typeof(CurrencyCode)).Cast<CurrencyCode>().ToArray();

            CurrencyCode[] currencyCodesNotInDb = currencyCodes.Where(currencyCode => !currencies.Select(currency => currency.Code).Contains(currencyCode)).ToArray();

            Currency[] currenciesNotInDb = currencyCodesNotInDb.Select(currencyCode => new Currency()
            {
                Code = currencyCode,
                LastUpdateDateTime = DateTime.MinValue,
                USDRatio = 0,
            }).ToArray();

            Currency[] allCurrencies = currencies.Concat(currenciesNotInDb).ToArray();
            return allCurrencies;
        }

        public static Decimal CalculateCurrencyValue(Decimal value, Currency fromCurrency, Currency toCurrency)
        {
            if (fromCurrency.Code == toCurrency.Code)
                return value;

            Decimal fromCurrencyUSDRatio = fromCurrency.Code == CurrencyCode.USD ? 1 : fromCurrency.USDRatio;
            Decimal toCurrencyUSDRatio = toCurrency.Code == CurrencyCode.USD ? 1 : toCurrency.USDRatio;

            Decimal valueInUSD = value / fromCurrencyUSDRatio;
            Decimal valueInToCurrency = valueInUSD * toCurrencyUSDRatio;

            return valueInToCurrency;
        }
    }
}
