using gamesdatabasetwo.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gamesdatabasetwo.Other
{
    public class Repository
    {
        public void DoRefillDatabase(ApplicationDbContext context)
        {
            context.ClearAllDatabases();
            context.AddPublisher(new Publisher { Name = "Electronic Arts" });
            context.AddPublisher(new Publisher { Name = "Sausage Party" });
            context.AddPublisher(new Publisher { Name = "Inferior Software" });
            context.AddPublisher(new Publisher { Name = "Melissa McGregor Inc." });
            context.AddDeveloper(new Developer { Name = "Microsoft Studios" });
            context.AddDeveloper(new Developer { Name = "Playhouse Productions" });
            context.AddDeveloper(new Developer { Name = "Studio Sentinel" });
            context.AddDeveloper(new Developer { Name = "Assignation Assimilation" });

            var addPublishers = context.AllPublishers();
            var addDevelopers = context.AllDevelopers();

            int loopNumber = 1;
            int year = 1999;
            for (int i = 0; i < 10; i++)
            {
                if (loopNumber == 4)
                {
                    loopNumber = 0;
                }

                int randomDeveloper = RandomGenerator(addDevelopers.First().Id, addDevelopers.Last().Id);
                int randomPublisher = RandomGenerator(addPublishers.First().Id, addPublishers.Last().Id);

                var developer = addDevelopers.Single(o => o.Id == randomDeveloper);
                var publisher = addPublishers.Single(o => o.Id == randomPublisher);

                //context.AddGame(new Game { Name = $"Test{i}", Genre = $"TestGenre{i}", Platforms = $"TestPlatform{i}", ReleasedWhere = $"TestWhere{i}", Theme = $"TestTheme{i}", Year = year, Developer = developer, Publisher = publisher });
                year++;
                loopNumber++;
            }
        }

        public int RandomGenerator(int firstNumber, int secondNumber)
        {
            var random = new Random();
            return (random.Next(firstNumber, secondNumber + 1));

        }
    }
}
