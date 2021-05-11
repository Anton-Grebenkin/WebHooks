using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebHooks.Worker.Models;
using WebHooks.DAL.Models;
using WebHooks.DAL.Interfaces;
using WebHooks.DAL.EF;
using Microsoft.EntityFrameworkCore;
using WebHooks.DAL.Repositories;
using BonusPlus.WebHook.Client;
using System.Net;
using System.Threading;
using System.Net.Http;


namespace WebHooks.Worker
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "";
            var contextOptions = new DbContextOptionsBuilder<WebHooksContext>()
            .UseSqlServer(connectionString)
            .Options;
            WebHooksContext context;

            //WebHooksRepository webHooksRepository = new WebHooksRepository(connectionString);

            List<EventHook> eventHookList = new List<EventHook>();
            List<string> accountList = new List<string>();
            List<AccountTask> accountTaskList = new List<AccountTask>();
            AccountTask accountTask;
            
            while (true)
            {
                Console.WriteLine(String.Format("{0}: Проверяем кол-во задач", DateTime.Now.ToString("T")));
                if (accountTaskList.Count() < 30)
                {
                    using(context = new WebHooksContext(contextOptions))
                    {
                        IRepository<EventHook> eventHookRepository = new GenericRepository<EventHook>(context);
                        try
                        {
                            Console.WriteLine(String.Format("{0}: Выпоняем основной запрос", DateTime.Now.ToString("T")));
                            accountList = eventHookRepository.GetQuery(e => e.Subscription)
                                                       .AsNoTracking()
                                                       .Where(e => e.SendTime <= DateTime.UtcNow && e.SendTime != null && e.IsActual && e.Subscription.IsActual)
                                                       .Select(e => e.Subscription.ExternalAccountId)
                                                       .Distinct()
                                                       .ToList();
                            Console.WriteLine(String.Format("{0}: Основной запрос выполнился ", DateTime.Now.ToString("T")));
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(String.Format("{0}: Ошибка! Не удалось выпонить основной запрос. {1} ", DateTime.Now.ToString("T"), ex));
                        }
                    }
                }

                if (accountList.Count == 0)
                {
                    Thread.Sleep(10000);
                    continue;
                }

                string account;
                while (accountList.Count() > 0)
                {
                    account = accountList[0];
                    bool taskExists;
                    lock (accountTaskList)
                    {
                        taskExists = accountTaskList.Where(a => a.ExternalAccountId == account).Any();
                    }

                    if (!taskExists && accountTaskList.Count() < 30)
                    {
                        accountTask = new AccountTask();
                        accountTask.ExternalAccountId = account;
                        accountList.RemoveAt(0);
                        lock (accountTaskList)
                        {
                            accountTaskList.Add(accountTask);
                        }
                        accountTask.Task = SendHooks(account, accountTaskList);
                    } 
                    else if (taskExists)
                    {
                        accountList.RemoveAt(0);
                    }
                }
            }
        }

        private static Task SendHooks(string accountId, List<AccountTask> accountTaskList)
        {
            return Task.Run(async() =>
            {
                Console.WriteLine(String.Format("{0}: Поток {1} запустился", DateTime.Now.ToString("T"), Thread.CurrentThread.ManagedThreadId));
                var contextOptions = new DbContextOptionsBuilder<WebHooksContext>()
                .UseSqlServer("")
                .Options;

                ApiClient apiClient = new ApiClient();

                List<EventHook> eventHooks = new List<EventHook>();
                EventHook hook = new EventHook();
                while (true)
                {
                    using (var context = new WebHooksContext(contextOptions))
                    {
                        context.Database.SetCommandTimeout(30);
                        IRepository<EventHook> eventHookRepository = new GenericRepository<EventHook>(context);
                        //IRepository<HookTry> hookTryRepository = new GenericRepository<HookTry>(context);

                        try
                        {
                            eventHooks = await eventHookRepository
                                .GetQuery(e => e.Subscription)
                                .AsNoTracking()
                                .Where(e => e.IsActual && e.Subscription.ExternalAccountId == accountId && e.Subscription.IsActual && e.SendTime <= DateTime.UtcNow && e.SendTime != null)
                                .OrderBy(e => e.SendTime)
                                .Take(50)
                                .ToListAsync();

                            Console.WriteLine(String.Format("{0}: Поток {1} получил записи из базы данных", DateTime.Now.ToString("T"), Thread.CurrentThread.ManagedThreadId));
                        }

                        catch (Exception ex)
                        {
                            Console.WriteLine(String.Format("{0}: Ошибка! {1}", DateTime.Now.ToString("T"), ex));
                            lock (accountTaskList)
                            {
                                accountTaskList.Remove(accountTaskList.Where(a => a.ExternalAccountId == accountId).FirstOrDefault());
                                Console.WriteLine(String.Format("{0}: В Потоке {1} удалена задача", DateTime.Now.ToString("T"), Thread.CurrentThread.ManagedThreadId));
                            }
                            return;
                        }
                    }

                    if (eventHooks.Count == 0)
                    {
                        Console.WriteLine(String.Format("{0}: В Потоке {1} нет записей, пытаемся удалить задачу", DateTime.Now.ToString("T"), Thread.CurrentThread.ManagedThreadId));

                        lock (accountTaskList)
                        {
                            accountTaskList.Remove(accountTaskList.Where(a => a.ExternalAccountId == accountId).FirstOrDefault());
                            Console.WriteLine(String.Format("{0}: В Потоке {1} удалена задача", DateTime.Now.ToString("T"), Thread.CurrentThread.ManagedThreadId));
                        }
                        return;
                    }

                    using (var context = new WebHooksContext(contextOptions))
                    {
                        context.Database.SetCommandTimeout(30);
                        IRepository<EventHook> eventHookRepository = new GenericRepository<EventHook>(context);

                        List<HookTry> hookTryes = new List<HookTry>();

                        for (int i = 0; i < eventHooks.Count(); i++)
                        {
                            HookTry hookTry = new HookTry();

                            hook = eventHooks[i];
                            hookTry.EventHookId = hook.Id;
                            hookTry.CreateTime = DateTime.UtcNow;
                            hookTry.IsActual = true;

                            HttpResponseMessage response = new HttpResponseMessage();
                            try
                            {
                                response = await apiClient.SendRequest(hook.Subscription.Url, hook.Subscription.HttpMethod, hook.Subscription.SecretKey, hook.Subscription.ContentType, hook.Event);
                                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.Created && response.StatusCode != HttpStatusCode.NoContent)
                                {
                                    hook.TryCount += 1;

                                    if (hook.TryCount >= 3)
                                    {
                                        hook.Status = "FAILURE";
                                        hook.SendTime = null;
                                    }
                                    else
                                    {
                                        hook.Status = "REPEAT";
                                        if (hook.SendTime != null)
                                        {
                                            hook.SendTime = DateTime.UtcNow.AddMinutes(5.0);
                                        }
                                    }
                                }
                                else
                                {
                                    hook.TryCount += 1;
                                    hook.SendTime = null;
                                    hook.Status = "DONE";
                                }

                                hookTry.Request = String.Format("Method: {0} {1}\nContent-Type: {2}\nParams: {3}", hook.Subscription.HttpMethod, hook.Subscription.Url, hook.Subscription.ContentType, hook.Event);
                                hookTry.Response = apiClient.GetResponseContent(response);
                                hookTry.HttpStatus = ((int)response.StatusCode).ToString();
                                hookTryes.Add(hookTry);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(String.Format("{0}: Ошибка! {1}", DateTime.Now.ToString("T"), ex));
                                continue;
                            }
                            finally
                            {
                                response.Dispose();
                            }

                            try
                            {
                                eventHookRepository.ExecuteQuery("UPDATE EventHooks set Status = {0}, TryCount = {1}, SendTime = {3} where Id = {2}", hook.Status, hook.TryCount, hook.Id, hook.SendTime);   
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(String.Format("{0}: Ошибка! {1}", DateTime.Now.ToString("T"), ex));
                                context.Dispose();
                                lock (accountTaskList)
                                {
                                    accountTaskList.Remove(accountTaskList.Where(a => a.ExternalAccountId == accountId).FirstOrDefault());
                                    Console.WriteLine(String.Format("{0}: В Потоке {1} удалена задача", DateTime.Now.ToString("T"), Thread.CurrentThread.ManagedThreadId));
                                }
                                return;
                            }
                        }
                        try
                        {
                            context.ChangeTracker.AutoDetectChangesEnabled = false;
                            context.HookTryes.AddRange(hookTryes.ToArray());
                            await context.SaveChangesAsync();
                            context.ChangeTracker.AutoDetectChangesEnabled = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(String.Format("{0}: Ошибка! {1}", DateTime.Now.ToString("T"), ex));
                            context.Dispose();
                            lock (accountTaskList)
                            {
                                accountTaskList.Remove(accountTaskList.Where(a => a.ExternalAccountId == accountId).FirstOrDefault());
                                Console.WriteLine(String.Format("{0}: В Потоке {1} удалена задача", DateTime.Now.ToString("T"), Thread.CurrentThread.ManagedThreadId));
                            }
                            return;
                        }
                        Console.WriteLine(String.Format("{0}: Поток {1} обновил {2} записей", DateTime.Now.ToString("T"), Thread.CurrentThread.ManagedThreadId, eventHooks.Count()));
                    }
                }
            });
        }
    }
}

