using System;
using System.Collections.Generic;
using Xunit;

namespace BigHelp.Lottery.Tests
{
    public class LotteryTests
    {
        [Fact]
        public void TicketLottery()
        {
            var rnd = new Random();
            var lottery = new Lottery<Prizes>(Lottery<Prizes>.DrawResponse.RemoveTicket);

            for (int i = 0; i < 10000; i++)
            {
                var answers = new Dictionary<Prizes, int>
                {
                    {Prizes.Prize1, 0},
                    {Prizes.Prize2, 0},
                    {Prizes.Prize3, 0},
                    {Prizes.Prize4, 0}
                };

                int prize1 = rnd.Next(1, 50);
                int prize2 = rnd.Next(1, 50);
                int prize3 = rnd.Next(1, 50);
                int prize4 = rnd.Next(1, 50);

                lottery.AddTickets(Prizes.Prize1, prize1);
                lottery.AddTickets(Prizes.Prize2, prize2);
                lottery.AddTickets(Prizes.Prize3, prize3);
                lottery.AddTickets(Prizes.Prize4, prize4);

                do
                {
                    var winner = lottery.Draw();
                    answers[winner]++;
                } while (lottery.NumberOfTickets > 0);

                Assert.Equal(prize1, answers[Prizes.Prize1]);
                Assert.Equal(prize2, answers[Prizes.Prize2]);
                Assert.Equal(prize3, answers[Prizes.Prize3]);
                Assert.Equal(prize4, answers[Prizes.Prize4]);

            }
        }

        [Fact]
        public void PrizeLottery()
        {
            var rnd = new Random();
            var lottery = new Lottery<Prizes>(Lottery<Prizes>.DrawResponse.RemovePrize);

            for (int i = 0; i < 10000; i++)
            {
                var dicResposta = new Dictionary<Prizes, int>
                {
                    {Prizes.Prize1, 0},
                    {Prizes.Prize2, 0},
                    {Prizes.Prize3, 0},
                    {Prizes.Prize4, 0}
                };

                int moedas1 = rnd.Next(1, 50);
                int moedas2 = rnd.Next(1, 50);
                int powerUp = rnd.Next(1, 50);
                int powerUpeMoedas = rnd.Next(1, 50);

                lottery.AddTickets(Prizes.Prize1, moedas1);
                lottery.AddTickets(Prizes.Prize2, moedas2);
                lottery.AddTickets(Prizes.Prize3, powerUp);
                lottery.AddTickets(Prizes.Prize4, powerUpeMoedas);

                do
                {
                    var winner = lottery.Draw();
                    dicResposta[winner]++;
                } while (lottery.NumberOfPrizes > 0);

                Assert.Equal(1, dicResposta[Prizes.Prize1]);
                Assert.Equal(1, dicResposta[Prizes.Prize2]);
                Assert.Equal(1, dicResposta[Prizes.Prize3]);
                Assert.Equal(1, dicResposta[Prizes.Prize4]);
            }
        }


        public enum Prizes
        {
            Prize1,
            Prize2,
            Prize3,
            Prize4
        }
    }
}