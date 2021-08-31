using System;
using System.Collections.Generic;
using System.Linq;

namespace BigHelp.Lottery
{
    public sealed class Lottery<T>
    {
        public enum DrawResponse
        {
            Default,
            DoNothing,
            RemovePrize,
            RemoveTicket
        }

        private readonly DrawResponse _defaultDrawResponse;
        static readonly Random _randon = new Random();

        public Lottery(DrawResponse defaultDrawResponse = DrawResponse.DoNothing)
            => _defaultDrawResponse = defaultDrawResponse == DrawResponse.Default
                ? DrawResponse.DoNothing
                : defaultDrawResponse;


        private readonly Dictionary<string, PrizeBag<T>> _tickets = new Dictionary<string, PrizeBag<T>>();

        public void AddTickets(T prize, int numberOfTickets)
        {
            if (prize == null) throw new ArgumentException($"'{nameof(prize)}' cannot be null");

            var entryName = prize.ToString().ToLowerInvariant();

            if (_tickets.ContainsKey(entryName)) _tickets[entryName].NumberOfTickets += numberOfTickets;
            else _tickets.Add(entryName, new PrizeBag<T>(prize, numberOfTickets));
        }

        public int NumberOfPrizes => _tickets.Count;
        public int NumberOfTickets => _tickets.Sum(t => t.Value.NumberOfTickets);

        public T Draw(DrawResponse drawResponse = DrawResponse.Default)
        {
            if (drawResponse == DrawResponse.Default) drawResponse = _defaultDrawResponse;

            if (_tickets.Count == 0) throw new InvalidOperationException("There are no prizes left.");

            var elegibleTickets = _tickets.Select(kv => kv.Value).Where(v => v.NumberOfTickets > 0).ToArray();

            int totalOfTickets = elegibleTickets.Sum(t => t.NumberOfTickets);
            if (totalOfTickets == 0) throw new InvalidOperationException("There are no tickets left.");

            int winnerNumber = Convert.ToInt32(_randon.NextDouble() * totalOfTickets);
            double previousSum = 0;
            double sum = 0;
            PrizeBag<T> winner = null;
            foreach (var ticket in elegibleTickets)
            {
                sum += ticket.NumberOfTickets;
                if (winnerNumber >= previousSum && (winnerNumber < sum || winnerNumber == totalOfTickets))
                {
                    winner = ticket;
                    break;
                }
                previousSum = sum;
            }
            if (winner == null) throw new SystemException("Unexpected result. No winner was found, even with tickets avaliable.");
            if (drawResponse == DrawResponse.RemovePrize) _tickets.Remove(winner.Prize.ToString().ToLowerInvariant());
            if (drawResponse == DrawResponse.RemoveTicket) _tickets[winner.Prize.ToString().ToLowerInvariant()].NumberOfTickets -= 1;
            return winner.Prize;
        }
    }

    internal class PrizeBag<T>
    {
        private int _numberOfTickets;

        internal PrizeBag(T prize, int numberOfTickets)
        {
            Prize = prize;
            NumberOfTickets = numberOfTickets;
        }
        public T Prize { get; }

        public int NumberOfTickets
        {
            get => _numberOfTickets;
            internal set
            {
                if (value < 0) throw new InvalidOperationException("INTERNAL ERROR: Number of tickets cannot be less than zero");
                _numberOfTickets = value;
            }
        }

        public override string ToString()
        {
            return $"{Prize} = {NumberOfTickets}";
        }
    }
}
