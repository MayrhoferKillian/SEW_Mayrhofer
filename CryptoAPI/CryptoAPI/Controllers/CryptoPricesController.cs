using Microsoft.AspNetCore.Mvc;
using CryptoAPI.Models;
using System;
using System.Collections.Generic;

namespace CryptoAPI.Controllers
{
    // Die Route ist "api/CryptoPrices"
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoPricesController : ControllerBase
    {
        // Der Endpunkt ist "history", also insgesamt "api/CryptoPrices/history"
        [HttpGet("history")]
        public IActionResult GetHistory()
        {
            var history = new List<CryptoPrice>();
            var random = new Random();

            // Startpreis festlegen (z.B. 150 EUR)
            decimal currentPrice = 150.0m;

            // Startzeit festlegen (z.B. vor 30 Minuten)
            var currentTime = DateTime.Now.AddMinutes(-30);

            // Wir generieren 60 Datenpunkte (z.B. für jede halbe Minute einen)
            for (int i = 0; i < 60; i++)
            {
                // Zufällige Preisschwankung zwischen -5 und +5 EUR
                var fluctuation = (decimal)(random.NextDouble() * 10 - 5);
                currentPrice += fluctuation;

                // Verhindern, dass der Preis unter 0 fällt
                if (currentPrice < 10) currentPrice = 10;

                history.Add(new CryptoPrice
                {
                    CurrentPrice = Math.Round(currentPrice, 2), // Auf 2 Kommastellen runden
                    Timestamp = currentTime
                });

                // Zeit für den nächsten Punkt um 30 Sekunden erhöhen
                currentTime = currentTime.AddSeconds(30);
            }

            // Gibt die Liste als JSON zurück (HTTP 200 OK)
            return Ok(history);
        }
    }
}