﻿using EventFlow.Configuration;
using EventFlow.EntityFramework.Extensions;
using EventFlow.EntityFramework.Tests.InMemory;
using EventFlow.EntityFramework.Tests.Model;
using EventFlow.Extensions;
using EventFlow.TestHelpers.Aggregates.Entities;

namespace EventFlow.EntityFramework.Tests
{
    public static class EntityFrameworkTestExtensions
    {
        public static IEventFlowOptions ConfigureForEventStoreTest<TDbContextProvider>(this IEventFlowOptions options)
            where TDbContextProvider : class, IDbContextProvider<TestDbContext>
        {
            return options
                .ConfigureEntityFramework()
                .AddDbContextProvider<TestDbContext, TDbContextProvider>(Lifetime.Singleton)
                .UseEntityFrameworkEventStore<TestDbContext>();
        }

        public static IEventFlowOptions ConfigureForReadStoreTest<TDbContextProvider>(this IEventFlowOptions options)
            where TDbContextProvider : class, IDbContextProvider<TestDbContext>
        {
            return options
                .RegisterServices(sr => sr.RegisterType(typeof(ThingyMessageLocator)))
                .ConfigureEntityFramework()
                .AddDbContextProvider<TestDbContext, TDbContextProvider>(Lifetime.Singleton)
                .UseEntityFrameworkReadModel<ThingyReadModelEntity, TestDbContext>()
                .UseEntityFrameworkReadModel<ThingyMessageReadModelEntity, TestDbContext, ThingyMessageLocator>()
                .AddQueryHandlers(
                    typeof(EfThingyGetQueryHandler),
                    typeof(EfThingyGetVersionQueryHandler),
                    typeof(EfThingyGetMessagesQueryHandler));
        }
    }
}
